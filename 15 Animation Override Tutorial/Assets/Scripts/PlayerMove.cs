using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public bool canMove = true;
    public int height = 5;

    private bool nextTurnIsSpringMove = false;
    private bool nextTurnIsArrowMove = false;
    private bool elevated = false;
    private Vector2Int nextPosition;
    private Direction facingDirection = Direction.Up;
    private Direction movingDirection = Direction.Up;
    private Animator anim;
    private GameObject animatedBody;
    private RestartManager restartManager;
    private LevelManager levelManager;

    void Awake() {
        GridManager.AddToGrid(gameObject, true);
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        animatedBody = anim.gameObject;
        restartManager = FindFirstObjectByType<RestartManager>();
        levelManager = restartManager.GetComponent<LevelManager>();
        nextPosition = GridManager.GetGridPosition(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void LateUpdate() {
        Vector3 position = animatedBody.transform.localPosition;
        Vector3 scale = animatedBody.transform.localScale;
        float swap;
        switch (movingDirection) {
            case Direction.Up:
                break;
            case Direction.Down:
                position *= -1;
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
        animatedBody.transform.localPosition = position;
        animatedBody.transform.localScale = scale;

        AnimatorStateInfo animStateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (animStateInfo.IsName("No Motion")) {
            transform.position = new Vector3(nextPosition.x, height, nextPosition.y);
        }
    }

    public void Move() {
        transform.position = new Vector3(nextPosition.x, height, nextPosition.y);
        Vector2Int gridPosition = GridManager.GetGridPosition(gameObject);
        Vector2Int gridMovePosition = gridPosition;
        int moveAmount = 1;
        if (nextTurnIsSpringMove) {
            moveAmount = 2;
        }
        switch (facingDirection) {
            case Direction.Up:
                movingDirection = Direction.Up;
                gridMovePosition.y += moveAmount;
                break;
            case Direction.Down:
                movingDirection = Direction.Down;
                gridMovePosition.y -= moveAmount;
                break;
            case Direction.Left:
                movingDirection = Direction.Left;
                gridMovePosition.x -= moveAmount;
                break;
            case Direction.Right:
                movingDirection = Direction.Right;
                gridMovePosition.x += moveAmount;
                break;
        }
        if (nextTurnIsArrowMove) {
            nextTurnIsArrowMove = false;
        }
        List<GameObject> thingsAtPoint = GridManager.LookInGrid(gridMovePosition);
        bool shouldMove = false;
        bool shouldSpring = false;
        bool shouldDie = false;
        if (thingsAtPoint == null) {
            shouldMove = true;
            if (elevated) {
                elevated = false;
            }
        } else {
            if (thingsAtPoint[0].CompareTag("Fire")) {
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
                if (elevated) {
                    elevated = false;
                }
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
            if (elevated) {
                if (thingsAtPoint[0].CompareTag("Wall")) {
                    shouldMove = true;
                }
                if (thingsAtPoint[0].CompareTag("Hole")) {
                    shouldMove = false;
                    anim.SetTrigger("StoppedByHole");
                }
                if (thingsAtPoint[0].CompareTag("Enemy")) {
                    shouldMove = true;
                    elevated = false;
                }
                if (thingsAtPoint[0].CompareTag("LargeEnemy")) {
                    shouldMove = true;
                    elevated = false;
                }
                if (thingsAtPoint[0].CompareTag("Boulder")) {
                    shouldMove = true;
                    elevated = false;
                }
                if (thingsAtPoint[0].CompareTag("Spring")) {
                    shouldMove = true;
                    shouldSpring = true;
                    elevated = false;
                }
            }
            else if (nextTurnIsSpringMove) {
                shouldMove = true;
                if (thingsAtPoint[0].CompareTag("Wall")) {
                    elevated = true;
                }
                if (thingsAtPoint[0].CompareTag("Hole")) {
                    restartManager.StartRespawnProcess();
                }
                if (thingsAtPoint[0].CompareTag("Enemy")) {
                    // Do Nothing
                }
                if (thingsAtPoint[0].CompareTag("LargeEnemy")) {
                    // Do Nothing
                }
                if (thingsAtPoint[0].CompareTag("Boulder")) {
                    // Do Nothing
                }
                if (thingsAtPoint[0].CompareTag("Spring")) {
                    shouldSpring = true;
                }
            } else {
                if (thingsAtPoint[0].CompareTag("Wall")) {
                    shouldMove = false;
                    anim.SetTrigger("BumpWall");
                }
                if (thingsAtPoint[0].CompareTag("Hole")) {
                    shouldMove = false;
                    anim.SetTrigger("StoppedByHole");
                }
                if (thingsAtPoint[0].CompareTag("Spring")) {
                    shouldMove = true;
                    shouldSpring = true;
                }

                foreach (GameObject thingAtPoint in thingsAtPoint) {
                    if (thingAtPoint.CompareTag("Enemy")) {
                        shouldMove = false;
                        shouldSpring = false;
                    }
                    if (thingAtPoint.CompareTag("LargeEnemy")) {
                        shouldMove = true;
                        shouldSpring = false;
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
                            shouldSpring = false;
                            shouldDie = false;
                        } else {
                            shouldMove = true;
                        }
                    }
                }
            }
            if (thingsAtPoint[0].CompareTag("Goal")) {
                shouldMove = true;
                elevated = false;
                canMove = false;
                levelManager.LoadNextLevel();
            }
        }
        if (shouldMove) {
            ExecuteMove(gridPosition, gridMovePosition);
        }
        if (shouldSpring) {
            nextTurnIsSpringMove = true;
        }
        if (shouldDie) {
            restartManager.StartRespawnProcess();
        }
    }

    public void FinalTurn() {
        transform.position = new Vector3(nextPosition.x, height, nextPosition.y);
    }

    void GetInput() {
        if (!canMove || nextTurnIsArrowMove) {
            return;
        }
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && facingDirection != Direction.Up) {
            facingDirection = Direction.Up;
            anim.ResetTrigger("LookDown");
            anim.ResetTrigger("LookLeft");
            anim.ResetTrigger("LookRight");
            anim.SetTrigger("LookUp");
        } else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && facingDirection != Direction.Down) {
            facingDirection = Direction.Down;
            anim.ResetTrigger("LookUp");
            anim.ResetTrigger("LookLeft");
            anim.ResetTrigger("LookRight");
            anim.SetTrigger("LookDown");
        } else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && facingDirection != Direction.Left) {
            facingDirection = Direction.Left;
            anim.ResetTrigger("LookUp");
            anim.ResetTrigger("LookDown");
            anim.ResetTrigger("LookRight");
            anim.SetTrigger("LookLeft");
        } else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && facingDirection != Direction.Right) {
            facingDirection = Direction.Right;
            anim.ResetTrigger("LookUp");
            anim.ResetTrigger("LookLeft");
            anim.ResetTrigger("LookDown");
            anim.SetTrigger("LookRight");
        }
    }

    void ExecuteMove(Vector2Int oldPosition, Vector2Int newPosition) {
        GridManager.MoveObjectInGrid(gameObject, oldPosition, newPosition, true);
        if (nextTurnIsSpringMove) {
            anim.SetTrigger("Spring");
            nextTurnIsSpringMove = false;
        } else {
            anim.SetTrigger("Move");
        }
        nextPosition = newPosition;
    }

    public void StartShrink() {
        anim.enabled = false;
        transform.position = new Vector3(nextPosition.x, height, nextPosition.y);
        animatedBody.transform.localPosition = Vector3.zero;
        animatedBody.transform.localScale = Vector3.one;
        StartCoroutine(ShrinkToNothing());
    }

    IEnumerator ShrinkToNothing() {
        while(true) {
            transform.Rotate(Vector3.up * 1800 * Time.deltaTime);
            transform.localScale -= (transform.localScale + (transform.localScale * 8f)) * Time.deltaTime;
            yield return null;
        }
    }
}
