using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public float shotDelay;
    public GameObject playerShot;
    public Transform shotsParent;

    private bool canShoot;
    private bool inputUp;
    private bool inputDown;
    private bool inputLeft;
    private bool inputRight;
    private Vector2 shootDir;

    // Start is called before the first frame update
    void Start()
    {
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canShoot)
        {
            inputUp = Input.GetKey(KeyCode.UpArrow);
            inputDown = Input.GetKey(KeyCode.DownArrow);
            inputLeft = Input.GetKey(KeyCode.LeftArrow);
            inputRight = Input.GetKey(KeyCode.RightArrow);

            shootDir = Vector2.zero;
            if (inputUp)
            {
                shootDir.y = 1;
            }
            else if (inputDown)
            {
                shootDir.y = -1;
            } 

            if (inputRight)
            {
                shootDir.x = 1;
            }
            else if (inputLeft)
            {
                shootDir.x = -1;
            }

            if (shootDir != Vector2.zero)
            {
                Instantiate(playerShot, transform.position, Quaternion.LookRotation(Vector3.forward, shootDir), shotsParent);
                StartCoroutine(WaitToShoot());
            }
        }
    }

    IEnumerator WaitToShoot ()
    {
        canShoot = false;
        yield return new WaitForSeconds(shotDelay);
        canShoot = true;
    }
}
