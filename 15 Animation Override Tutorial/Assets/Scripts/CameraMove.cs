using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float moveSmoothing = 10;

    private Vector2 destinationPosition;
    private Vector2 currentPosition;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindFirstObjectByType<PlayerMove>().GetComponentInChildren<Animator>().transform;
        destinationPosition = new Vector2(player.position.x, player.position.z);
        currentPosition = new Vector2(player.position.x, player.position.z);
        transform.position = new Vector3(destinationPosition.x, 10, destinationPosition.y);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateDestinationPosition();
        MoveCamera();
    }

    void CalculateDestinationPosition() {
        destinationPosition = new Vector2(player.position.x, player.position.z);
    }

    void MoveCamera() {
        currentPosition = Vector2.Lerp(currentPosition, destinationPosition, moveSmoothing * Time.deltaTime);
        transform.position = new Vector3(currentPosition.x, 10, currentPosition.y);
    }
}
