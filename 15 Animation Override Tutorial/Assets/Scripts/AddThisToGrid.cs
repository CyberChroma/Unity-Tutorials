using UnityEngine;

public class AddThisToGrid : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GridManager.AddToGrid(gameObject);
    }
}
