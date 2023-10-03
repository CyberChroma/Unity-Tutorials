using UnityEngine;

public class HideOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
}
