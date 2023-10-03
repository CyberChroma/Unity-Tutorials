using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Direction arrowDirection = Direction.Right;

    // Start is called before the first frame update
    void Start()
    {
        GridManager.AddToGrid(gameObject);
    }
}
