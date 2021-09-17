using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    public float drag;
    public Transform mCam;

    private float movVert;
    private float movHorz;
    private Vector3 moveDir;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector3(rb.velocity.x * drag, rb.velocity.y, rb.velocity.z * drag);
        if (Input.GetKey(KeyCode.W))
        {
            movVert = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movVert = -1;
        }
        else
        {
            movVert = 0;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movHorz = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movHorz = -1;
        }
        else
        {
            movHorz = 0;
        }
        moveDir = mCam.right * movHorz + mCam.forward * movVert;
        moveDir = new Vector3(moveDir.x, 0, moveDir.z).normalized;
        rb.AddForce(moveDir * speed, ForceMode.Impulse);
    }
}
