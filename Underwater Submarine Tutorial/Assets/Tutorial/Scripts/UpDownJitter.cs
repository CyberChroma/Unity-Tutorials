using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownJitter : MonoBehaviour
{
    public float moveSpeed; // The speed that the object moves up and down
    public float stopTime; // The time that the object stays stopped at the top and bottom of it's movement cycle
    public float jitterDis; // How far the object moves from it's original position
    public bool randomizeSpeed; // How much to randomize the speeds for multiple objects jittering

    private Vector3 startPosition; // Stores the starting position of the object

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.localPosition; // Getting the starting position of the object
        if (randomizeSpeed) // If the speed should be random
        {
            transform.localPosition = new Vector3(startPosition.x, startPosition.y + Random.Range(-jitterDis, jitterDis), startPosition.z); // Start the object at a random point in its jitter cycle
        }
        StartCoroutine(MoveUp()); // Start moving the object up
    }

    IEnumerator MoveUp()
    {
        while (Mathf.Abs(transform.localPosition.y - (startPosition.y + jitterDis)) > 0.01f) // While the object has not reached its highest point in its cycle
        {
            if (randomizeSpeed) // If the speed should be random
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(startPosition.x, startPosition.y + jitterDis, startPosition.z), Random.Range(0, moveSpeed) * Time.deltaTime); // Move the object up a random amount
            }
            else // If the speed should be consistent
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(startPosition.x, startPosition.y + jitterDis, startPosition.z), moveSpeed * Time.deltaTime); // Move the object up a consistent amount
            }
            yield return null;
        }
        yield return new WaitForSeconds(stopTime); // Have the object pause at the top before moving down
        StartCoroutine(MoveDown()); // Start moving the object down
    }

    IEnumerator MoveDown()
    {
        while (Mathf.Abs(transform.localPosition.y - (startPosition.y - jitterDis)) > 0.01f) // If the speed should be random
        {
            if (randomizeSpeed)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(startPosition.x, startPosition.y - jitterDis, startPosition.z), Random.Range(0, moveSpeed) * Time.deltaTime); // Move the object down a random amount
            }
            else // If the speed should be consistent
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(startPosition.x, startPosition.y - jitterDis, startPosition.z), moveSpeed * Time.deltaTime); // Move the object down a consistent amount
            }
            yield return null;
        }
        yield return new WaitForSeconds(stopTime); // Have the object pause at the top before moving up
        StartCoroutine(MoveUp()); // Start moving the object up
    }
}
