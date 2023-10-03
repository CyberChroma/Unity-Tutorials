using UnityEngine;

public class ArrowSpin : MonoBehaviour
{
    public bool clockwise = true;
    public float turnSmoothing = 10;

    private Vector3 targetRotation;
    private Arrow arrow;
    private TurnManager turnManager;

    // Start is called before the first frame update
    void Start()
    {
        arrow = GetComponent<Arrow>();
        turnManager = GetComponentInParent<TurnManager>();
        turnManager.AddArrow(this);
        switch (arrow.arrowDirection) {
            case Direction.Up:
                targetRotation = Vector3.up * 0;
                break;
            case Direction.Right:
                targetRotation = Vector3.up * 90;
                break;
            case Direction.Down:
                targetRotation = Vector3.up * 180;
                break;
            case Direction.Left:
                targetRotation = Vector3.up * -90;
                break;
        }
        transform.rotation = Quaternion.Euler(targetRotation);
    }

    void Update() {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRotation), turnSmoothing * Time.deltaTime);
    }

    public void Turn() {
        if (clockwise) {
            targetRotation.y += 90;
            switch (arrow.arrowDirection) {
                case Direction.Up:
                    arrow.arrowDirection = Direction.Right;
                    break;
                case Direction.Right:
                    arrow.arrowDirection = Direction.Down;
                    break;
                case Direction.Down:
                    arrow.arrowDirection = Direction.Left;
                    break;
                case Direction.Left:
                    arrow.arrowDirection = Direction.Up;
                    break;
            }
        } else {
            targetRotation.y -= 90;
            switch (arrow.arrowDirection) {
                case Direction.Up:
                    arrow.arrowDirection = Direction.Left;
                    break;
                case Direction.Left:
                    arrow.arrowDirection = Direction.Down;
                    break;
                case Direction.Down:
                    arrow.arrowDirection = Direction.Right;
                    break;
                case Direction.Right:
                    arrow.arrowDirection = Direction.Up;
                    break;
            }
        }
    }
}
