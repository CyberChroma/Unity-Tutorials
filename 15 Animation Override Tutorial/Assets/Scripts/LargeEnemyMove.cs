using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeEnemyMove : MonoBehaviour
{
    public int height = 3;

    private Animator anim;
    private GameObject body;
    private Direction facingDirection = Direction.Down;
    private Direction movingDirection = Direction.Down;
    private RestartManager restartManager;
    private TurnManager turnManager;
    private Vector2Int nextPosition;

    private void Awake() {
        Vector2Int positon = GridManager.GetGridPosition(gameObject);
        GridManager.AddToGrid(gameObject, positon);
        GridManager.AddToGrid(gameObject, new Vector2Int(positon.x + 1, positon.y));
        GridManager.AddToGrid(gameObject, new Vector2Int(positon.x, positon.y + 1));
        GridManager.AddToGrid(gameObject, new Vector2Int(positon.x + 1, positon.y + 1));
    }

    // Start is called before the first frame update
    void Start() {
        anim = GetComponentInChildren<Animator>();
        body = anim.gameObject;
        restartManager = GetComponentInParent<RestartManager>();
        turnManager = restartManager.GetComponent<TurnManager>();
        turnManager.AddLargeEnemy(this);
        nextPosition = GridManager.GetGridPosition(gameObject);
        CalculateLookDirection();
    }

    private void LookUp() {
        facingDirection = Direction.Up;
        anim.SetTrigger("LookUp");
    }

    private void LookDown() {
        facingDirection = Direction.Down;
        anim.SetTrigger("LookDown");
    }

    private void LookLeft() {
        facingDirection = Direction.Left;
        anim.SetTrigger("LookLeft");
    }

    private void LookRight() {
        facingDirection = Direction.Right;
        anim.SetTrigger("LookRight");
    }

    private void CalculateLookDirection() {
        Vector2Int ourPositon = nextPosition;
        Vector2Int playerPosition = GridManager.GetPlayerPosition();
        Vector2Int playerDistance = playerPosition - ourPositon;

        // If Player Is To The Right, Use Right Side Of Enemy As Reference Point
        if (playerDistance.x > 0) {
            playerDistance.x--;
        }
        // If Player Is To Up, Use Top Side Of Enemy As Reference Point
        if (playerDistance.y > 0) {
            playerDistance.y--;
        }
        // If Enemy Is On Player,
        if (playerDistance == Vector2Int.zero) {
            return;
        }
        // If Only Vertical Distance To Player,
        if (playerDistance.x == 0) {
            MoveOnY(playerDistance);
        // If Only Horizontal Distance To Player,
        } else if (playerDistance.y == 0) {
            MoveOnX(playerDistance);
        // If Player Is Diagonal Relative To Us,
        } else {
            // If Player Is Further Vertically, Or On Perfect Diagonal
            if (Mathf.Abs(playerDistance.y) >= Mathf.Abs(playerDistance.x)) {
                // See If We Something Is Blocking Us Moving Vertically
                bool success = TryMoveOnY(playerDistance);
                if (success) {
                    MoveOnY(playerDistance);
                } else {
                    // See If We Something Is Blocking Us Moving Horizontally
                    success = TryMoveOnX(playerDistance);
                    if (success) {
                        MoveOnX(playerDistance);
                    } else {
                        // Both Ways Are Blocked, So Just Move Vertically
                        MoveOnY(playerDistance);
                    }
                }
            // If Player Is Further Horizontally,
            } else {
                // See If We Something Is Blocking Us Moving Horizontally
                bool success = TryMoveOnX(playerDistance);
                if (success) {
                    MoveOnX(playerDistance);
                } else {
                    // See If We Something Is Blocking Us Moving Vertically
                    success = TryMoveOnY(playerDistance);
                    if (success) {
                        MoveOnY(playerDistance);
                    } else {
                        // Both Ways Are Blocked, So Just Move Horizontally
                        MoveOnX(playerDistance);
                    }
                }
            }
        }
    }

    private bool TryMoveOnY(Vector2Int playerDistance) {
        Vector2Int gridMovePosition = nextPosition;
        if (playerDistance.y > 0) {
            gridMovePosition.y+=2;
        } else {
            gridMovePosition.y--;
        }

        return TryMoveCommon(gridMovePosition, true);
    }

    private bool TryMoveOnX(Vector2Int playerDistance) {
        Vector2Int gridMovePosition = nextPosition;
        if (playerDistance.x > 0) {
            gridMovePosition.x+=2;
        } else {
            gridMovePosition.x--;
        }

        return TryMoveCommon(gridMovePosition, false);
    }

    private bool TryMoveCommon(Vector2Int gridMovePosition, bool testingVertical) {
        List<GameObject> thingsAtPoint = GridManager.LookInGrid(gridMovePosition);
        if (thingsAtPoint == null) {
            return TryMoveNext(gridMovePosition, testingVertical);
        } else if (thingsAtPoint[0].CompareTag("Wall") || thingsAtPoint[0].CompareTag("Hole")) {
            return false;
        } else {
            return TryMoveNext(gridMovePosition, testingVertical);
        }
    }

    private bool TryMoveNext(Vector2Int gridMovePosition, bool testingVertical) {
        if (testingVertical) {
            gridMovePosition.x++;
        } else {
            gridMovePosition.y++;
        }
        List<GameObject> thingsAtPoint = GridManager.LookInGrid(gridMovePosition);
        if (thingsAtPoint == null) {
            return true;
        } else if (thingsAtPoint[0].CompareTag("Wall") || thingsAtPoint[0].CompareTag("Hole")) {
            return false;
        } else {
            return true;
        }
    }

    private void MoveOnX(Vector2Int playerDistance) {
        if (playerDistance.x > 0) {
            LookRight();
        } else {
            LookLeft();
        }
    }

    private void MoveOnY(Vector2Int playerDistance) {
        if (playerDistance.y > 0) {
            LookUp();
        } else {
            LookDown();
        }
    }

    private void LateUpdate() {
        Vector3 position = body.transform.localPosition;
        Vector3 scale = body.transform.localScale;
        scale.x = ((scale.x - 1) / 2) + 1;
        scale.y = ((scale.y - 1) / 2) + 1;
        scale.z = ((scale.z - 1) / 2) + 1;
        float swap;
        position /= 2;
        switch (movingDirection) {
            case Direction.Up:
                break;
            case Direction.Down:
                position.z *= -1;
                break;
            case Direction.Left:
                swap = position.z;
                position.z = position.x;
                position.x = swap;
                position.x *= -1;

                swap = scale.z;
                scale.z = scale.x;
                scale.x = swap;
                break;
            case Direction.Right:
                swap = position.z;
                position.z = position.x;
                position.x = swap;

                swap = scale.z;
                scale.z = scale.x;
                scale.x = swap;
                break;
        }
        body.transform.localPosition = position;
        body.transform.localScale = scale;

        AnimatorStateInfo animStateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (animStateInfo.IsName("No Motion")) {
            transform.position = new Vector3(nextPosition.x, height, nextPosition.y);
        }
    }

    public void Move() {
        transform.position = new Vector3(nextPosition.x, height, nextPosition.y);
        Vector2Int gridPosition = GridManager.GetGridPosition(gameObject);
        Vector2Int gridMovePosition = gridPosition;
        Vector2Int gridLookPosition = gridPosition;
        Vector2Int gridLookSecondPosition = gridPosition;
        switch (facingDirection) {
            case Direction.Up:
                movingDirection = Direction.Up;
                gridMovePosition.y++;
                gridLookPosition.y += 2;
                gridLookSecondPosition = gridLookPosition;
                gridLookSecondPosition.x++;
                break;
            case Direction.Down:
                movingDirection = Direction.Down;
                gridMovePosition.y--;
                gridLookPosition.y--;
                gridLookSecondPosition = gridLookPosition;
                gridLookSecondPosition.x++;
                break;
            case Direction.Left:
                movingDirection = Direction.Left;
                gridMovePosition.x--;
                gridLookPosition.x--;
                gridLookSecondPosition = gridLookPosition;
                gridLookSecondPosition.y++;
                break;
            case Direction.Right:
                movingDirection = Direction.Right;
                gridMovePosition.x++;
                gridLookPosition.x += 2;
                gridLookSecondPosition = gridLookPosition;
                gridLookSecondPosition.y++;
                break;
        }
        List<GameObject> thingsAtPoint = GridManager.LookInGrid(gridLookPosition);
        List<GameObject> thingsAtPoint2 = GridManager.LookInGrid(gridLookSecondPosition);

        List<List<GameObject>> allPoints = new List<List<GameObject>> {
            thingsAtPoint,
            thingsAtPoint2
        };

        bool shouldMove = true;
        bool shouldDie = false;
        bool shouldKill = false;
        if (thingsAtPoint == null && thingsAtPoint2 == null) {
            // Do Nothing
        } else {
            foreach (List<GameObject> currentPoint in allPoints) {
                if (currentPoint == null) {
                    continue;
                }

                if (currentPoint[0].CompareTag("Wall") || currentPoint[0].CompareTag("Enemy") || currentPoint[0].CompareTag("LargeEnemy") || currentPoint[0].CompareTag("Goal")) {
                    anim.SetTrigger("BumpWall");
                    shouldKill = false;
                    shouldDie = false;
                    shouldMove = false;
                    break;
                }

                if (currentPoint[0].CompareTag("Fire")) {
                    shouldDie = true;
                }

                if (currentPoint[0].CompareTag("Cracked")) {
                    if (currentPoint[0].GetComponent<FallingDebris>().isDangerous) {
                        shouldDie = true;
                    }
                }

                if (currentPoint[0].CompareTag("Hole")) {
                    // Do Nothing
                }
                if (currentPoint[0].CompareTag("FadedArrow")) {
                    // Do Nothing
                }
                if (currentPoint[0].CompareTag("Arrow")) {
                    // Do Nothing
                }
                if (currentPoint[0].CompareTag("Spring")) {
                    // Do Nothing
                }

                foreach (GameObject thingAtPoint in currentPoint) {
                    if (thingAtPoint.CompareTag("Player")) {
                        shouldKill = true;
                    }
                    if (thingAtPoint.CompareTag("Enemy") || thingAtPoint.CompareTag("LargeEnemy")) {
                        shouldMove = false;
                        shouldKill = false;
                        anim.SetTrigger("BumpWall");
                    }
                    if (thingAtPoint.CompareTag("Boulder")) {
                        Direction invertedDirection = facingDirection;
                        switch (facingDirection) {
                            case Direction.Up:
                                invertedDirection = Direction.Down;
                                break;
                            case Direction.Down:
                                invertedDirection = Direction.Up;
                                break;
                            case Direction.Left:
                                invertedDirection = Direction.Right;
                                break;
                            case Direction.Right:
                                invertedDirection = Direction.Left;
                                break;
                        }
                        if (thingAtPoint.GetComponent<BoulderMove>().facingDirection == invertedDirection) {
                            shouldMove = false;
                            shouldDie = false;
                        }
                    }
                }
            }
        }
        if (shouldMove) {
            ExecuteMove(gridPosition, gridMovePosition, gridLookPosition);
        }

        Vector2Int ourPositon = nextPosition;
        Vector2Int playerPosition = GridManager.GetPlayerPosition();
        Vector2Int playerDistance = playerPosition - ourPositon;

        // If Player Is To The Right, Use Right Side Of Enemy As Reference Point
        if (playerDistance.x > 0) {
            playerDistance.x--;
        }
        // If Player Is To Up, Use Top Side Of Enemy As Reference Point
        if (playerDistance.y > 0) {
            playerDistance.y--;
        }
        // If Enemy Is On Player,
        if (playerDistance == Vector2Int.zero) {
            shouldKill = true;
        } else {
            shouldKill = false;
        }

        if (!shouldDie) {
            Vector2Int ourGridPosition = nextPosition;
            List<GameObject> thingsAtUs = GridManager.LookInGrid(ourGridPosition);
            ourGridPosition.x++;
            List<GameObject> thingsAtUs2 = GridManager.LookInGrid(ourGridPosition);
            ourGridPosition.y++;
            List<GameObject> thingsAtUs3 = GridManager.LookInGrid(ourGridPosition);
            ourGridPosition.x--;
            List<GameObject> thingsAtUs4 = GridManager.LookInGrid(ourGridPosition);

            List<List<GameObject>> thingsAtAllPoint = new List<List<GameObject>>() {
                thingsAtUs,
                thingsAtUs2,
                thingsAtUs3,
                thingsAtUs4,
            };

            bool deathByHole = true;
            foreach (List<GameObject> currentSelfPoint in thingsAtAllPoint) {
                if (currentSelfPoint == null || !currentSelfPoint[0].CompareTag("Hole")) {
                    deathByHole = false;
                    break;
                }
            }
            if (deathByHole) {
                shouldDie = true;
            }

            foreach (List<GameObject> currentSelfPoint in thingsAtAllPoint) {
                if (currentSelfPoint != null && currentSelfPoint[0].CompareTag("Cracked") && currentSelfPoint[0].GetComponent<FallingDebris>().isDangerous) {
                    shouldDie = true;
                }
            }
        }

        if (shouldDie) {
            StartDieProcess();
            shouldKill = false;
        }
        if (shouldKill) {
            restartManager.StartRespawnProcess();
        }
        CalculateLookDirection();
    }

    public void FinalTurn() {
        transform.position = new Vector3(nextPosition.x, height, nextPosition.y);
    }

    void ExecuteMove(Vector2Int oldPosition, Vector2Int newAnchorPosition, Vector2Int newEdgePosition) {
        if (oldPosition.y < newAnchorPosition.y) {
            GridManager.MoveObjectInGrid(gameObject, oldPosition, newEdgePosition);
            oldPosition.x++;
            newEdgePosition.x++;
            GridManager.MoveObjectInGrid(gameObject, oldPosition, newEdgePosition);
        } else if (oldPosition.y > newAnchorPosition.y) {
            oldPosition.y++;
            GridManager.MoveObjectInGrid(gameObject, oldPosition, newEdgePosition);
            oldPosition.x++;
            newEdgePosition.x++;
            GridManager.MoveObjectInGrid(gameObject, oldPosition, newEdgePosition);
        } else if (oldPosition.x < newAnchorPosition.x) {
            GridManager.MoveObjectInGrid(gameObject, oldPosition, newEdgePosition);
            oldPosition.y++;
            newEdgePosition.y++;
            GridManager.MoveObjectInGrid(gameObject, oldPosition, newEdgePosition);
        } else if (oldPosition.x > newAnchorPosition.x) {
            oldPosition.x++;
            GridManager.MoveObjectInGrid(gameObject, oldPosition, newEdgePosition);
            oldPosition.y++;
            newEdgePosition.y++;
            GridManager.MoveObjectInGrid(gameObject, oldPosition, newEdgePosition);
        }

        anim.SetTrigger("Move");
        nextPosition = newAnchorPosition;
    }

    public void StartDieProcess() {
        StartCoroutine(WaitToDie());
    }

    IEnumerator WaitToDie() {
        yield return null;
        Vector2Int positon = nextPosition;
        GridManager.DeleteFromGrid(gameObject, positon);
        GridManager.DeleteFromGrid(gameObject, new Vector2Int(positon.x + 1, positon.y));
        GridManager.DeleteFromGrid(gameObject, new Vector2Int(positon.x, positon.y + 1));
        GridManager.DeleteFromGrid(gameObject, new Vector2Int(positon.x + 1, positon.y + 1));
        turnManager.RemoveLargeEnemy(this);
        yield return new WaitForSeconds(turnManager.timePerTurn - 0.15f);
        StartShrink();
    }

    public void StartShrink() {
        anim.enabled = false;
        transform.position = new Vector3(nextPosition.x, height, nextPosition.y);
        body.transform.localPosition = Vector3.zero;
        body.transform.localScale = Vector3.one;
        StartCoroutine(ShrinkToNothing());
    }

    IEnumerator ShrinkToNothing() {
        while (true) {
            transform.Rotate(Vector3.up * -1800 * Time.deltaTime);
            transform.localScale -= (transform.localScale + (transform.localScale * 8f)) * Time.deltaTime;
            yield return null;
        }
    }
}
