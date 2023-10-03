using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public int height = 3;

    private bool nextTurnIsArrowMove = false;
    private Animator anim;
    private GameObject body;
    private Direction facingDirection = Direction.Down;
    private Direction movingDirection = Direction.Down;
    private RestartManager restartManager;
    private TurnManager turnManager;
    private Vector2Int nextPosition;

    private void Awake() {
        GridManager.AddToGrid(gameObject);
    }

    // Start is called before the first frame update
    void Start() {
        anim = GetComponentInChildren<Animator>();
        body = anim.gameObject;
        restartManager = GetComponentInParent<RestartManager>();
        turnManager = restartManager.GetComponent<TurnManager>();
        turnManager.AddEnemy(this);
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
        if (nextTurnIsArrowMove) {
            return;
        }
        Vector2Int ourPositon = nextPosition;
        Vector2Int playerPosition = GridManager.GetPlayerPosition();
        Vector2Int playerDistance = playerPosition - ourPositon;

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
            gridMovePosition.y++;
        } else {
            gridMovePosition.y--;
        }
        return TryMoveCommon(gridMovePosition);
    }

    private bool TryMoveOnX(Vector2Int playerDistance) {
        Vector2Int gridMovePosition = nextPosition;
        if (playerDistance.x > 0) {
            gridMovePosition.x++;
        } else {
            gridMovePosition.x--;
        }
        return TryMoveCommon(gridMovePosition);
    }

    private bool TryMoveCommon(Vector2Int gridMovePosition) {
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
        float swap;
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
        switch (facingDirection) {
            case Direction.Up:
                movingDirection = Direction.Up;
                gridMovePosition.y++;
                break;
            case Direction.Down:
                movingDirection = Direction.Down;
                gridMovePosition.y--;
                break;
            case Direction.Left:
                movingDirection = Direction.Left;
                gridMovePosition.x--;
                break;
            case Direction.Right:
                movingDirection = Direction.Right;
                gridMovePosition.x++;
                break;
        }
        if (nextTurnIsArrowMove) {
            nextTurnIsArrowMove = false;
        }
        List<GameObject> thingsAtPoint = GridManager.LookInGrid(gridMovePosition);
        bool shouldMove = false;
        bool shouldDie = false;
        bool shouldKill = false;
        if (thingsAtPoint == null) {
            shouldMove = true;
        } else {
            if (thingsAtPoint[0].CompareTag("Player")) {
                if (thingsAtPoint.Count > 1 && (thingsAtPoint[1].CompareTag("Enemy") || thingsAtPoint[1].CompareTag("LargeEnemy"))) {
                    anim.SetTrigger("BumpWall");
                } else {
                    shouldMove = true;
                    shouldKill = true;
                }
            }
            if (thingsAtPoint[0].CompareTag("Hole") || thingsAtPoint[0].CompareTag("Fire")) {
                shouldMove = true;
                shouldDie = true;
            }
            if (thingsAtPoint[0].CompareTag("Cracked")) {
                shouldMove = true;
                if (thingsAtPoint[0].GetComponent<FallingDebris>().isDangerous) {
                    shouldDie = true;
                }
            }
            if (thingsAtPoint[0].CompareTag("FadedArrow")) {
                shouldMove = true;
            }
            if (thingsAtPoint[0].CompareTag("Arrow")) {
                shouldMove = true;
                nextTurnIsArrowMove = true;
                facingDirection = thingsAtPoint[0].GetComponent<Arrow>().arrowDirection;
                switch (facingDirection) {
                    case Direction.Up:
                        anim.SetTrigger("LookUp");
                        break;
                    case Direction.Down:
                        anim.SetTrigger("LookDown");
                        break;
                    case Direction.Left:
                        anim.SetTrigger("LookLeft");
                        break;
                    case Direction.Right:
                        anim.SetTrigger("LookRight");
                        break;
                }
            }
            if (thingsAtPoint[0].CompareTag("Spring")) {
                shouldMove = true;
            }

            foreach (GameObject thingAtPoint in thingsAtPoint) {
                if (thingAtPoint.CompareTag("Player")) {
                    shouldMove = true;
                    shouldKill = true;
                }
                if (thingAtPoint.CompareTag("Enemy")) {
                    shouldMove = false;
                    anim.SetTrigger("BumpWall");
                }
                if (thingAtPoint.CompareTag("LargeEnemy")) {
                    shouldMove = false;
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
                    } else {
                        shouldMove = true;
                    }
                }
            }

            if (thingsAtPoint[0].CompareTag("Wall") || thingsAtPoint[0].CompareTag("Enemy") || thingsAtPoint[0].CompareTag("LargeEnemy") || thingsAtPoint[0].CompareTag("Goal")) {
                anim.SetTrigger("BumpWall");
                Vector2Int ourPositon = nextPosition;
                Vector2Int playerPosition = GridManager.GetPlayerPosition();
                Vector2Int playerDistance = playerPosition - ourPositon;
                if (playerDistance == Vector2Int.zero) {
                    shouldKill = true;
                } else {
                    shouldKill = false;
                }
                List<GameObject> thingsAtUs = GridManager.LookInGrid(gridPosition);
                if (thingsAtUs[0].CompareTag("Cracked") && thingsAtUs[0].GetComponent<FallingDebris>().isDangerous) {
                    shouldDie = true;
                } else {
                    shouldDie = false;
                }
                shouldMove = false;
            }
        }
        if (shouldMove) {
            ExecuteMove(gridPosition, gridMovePosition);
        }
        if (shouldDie) {
            StartDieProcess();
        }
        if (shouldKill) {
            restartManager.StartRespawnProcess();
        }
        CalculateLookDirection();
    }

    public void FinalTurn() {
        transform.position = new Vector3(nextPosition.x, height, nextPosition.y);
    }

    void ExecuteMove(Vector2Int oldPosition, Vector2Int newPosition) {
        GridManager.MoveObjectInGrid(gameObject, oldPosition, newPosition);
        anim.SetTrigger("Move");
        nextPosition = newPosition;
    }

    public void StartDieProcess() {
        StartCoroutine(WaitToDie());
    }

    IEnumerator WaitToDie() {
        yield return null;
        GridManager.DeleteFromGrid(gameObject, nextPosition);
        turnManager.RemoveEnemy(this);
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
