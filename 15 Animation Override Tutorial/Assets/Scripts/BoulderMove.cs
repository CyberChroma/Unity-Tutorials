using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderMove : MonoBehaviour
{
    public Direction facingDirection = Direction.Down;
    public int height = 2;

    private Animator anim;
    private GameObject body;
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
        turnManager.AddBoulder(this);
        nextPosition = GridManager.GetGridPosition(gameObject);
        movingDirection = facingDirection;
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
        List<GameObject> thingsAtPoint = GridManager.LookInGrid(gridMovePosition);
        if (thingsAtPoint == null) {
            ExecuteMove(gridPosition, gridMovePosition);
        } else {
            if (thingsAtPoint[0].CompareTag("Wall") || thingsAtPoint[0].CompareTag("Goal") || thingsAtPoint[0].CompareTag("Boulder")) {
                anim.SetTrigger("BumpWall");
                switch (facingDirection) {
                    case Direction.Up:
                        facingDirection = Direction.Down;
                        break;
                    case Direction.Down:
                        facingDirection = Direction.Up;
                        break;
                    case Direction.Left:
                        facingDirection = Direction.Right;
                        break;
                    case Direction.Right:
                        facingDirection = Direction.Left;
                        break;
                }
            }
            if (thingsAtPoint[0].CompareTag("Hole")) {
                ExecuteMove(gridPosition, gridMovePosition);
                StartDieProcess();
            }
            if (thingsAtPoint[0].CompareTag("Fire")) {
                ExecuteMove(gridPosition, gridMovePosition);
            }
            if (thingsAtPoint[0].CompareTag("Cracked")) {
                ExecuteMove(gridPosition, gridMovePosition);
            }
            if (thingsAtPoint[0].CompareTag("Spring")) {
                ExecuteMove(gridPosition, gridMovePosition);
            }
            if (thingsAtPoint[0].CompareTag("Enemy")) {
                ExecuteMove(gridPosition, gridMovePosition);
            }
            if (thingsAtPoint[0].CompareTag("LargeEnemy")) {
                ExecuteMove(gridPosition, gridMovePosition);
            }
            if (thingsAtPoint[0].CompareTag("Player")) {
                ExecuteMove(gridPosition, gridMovePosition);
            }
            if (thingsAtPoint[0].CompareTag("FadedArrow")) {
                ExecuteMove(gridPosition, gridMovePosition);
                facingDirection = thingsAtPoint[0].GetComponent<Arrow>().arrowDirection;
            }
            if (thingsAtPoint[0].CompareTag("Arrow")) {
                ExecuteMove(gridPosition, gridMovePosition);
                facingDirection = thingsAtPoint[0].GetComponent<Arrow>().arrowDirection;
            }
            List<GameObject> thingsInSameSpace = GridManager.LookInGrid(nextPosition);
            foreach(GameObject thingInSameSpace in thingsInSameSpace) {
                if (thingInSameSpace.CompareTag("Player")) {
                    restartManager.StartRespawnProcess();
                } else if (thingInSameSpace.CompareTag("Enemy")) {
                    thingInSameSpace.GetComponent<EnemyMove>().StartDieProcess();
                } else if (thingInSameSpace.CompareTag("LargeEnemy")) {
                    thingInSameSpace.GetComponent<LargeEnemyMove>().StartDieProcess();
                }
            }
        }
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
        turnManager.RemoveBoulder(this);
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
