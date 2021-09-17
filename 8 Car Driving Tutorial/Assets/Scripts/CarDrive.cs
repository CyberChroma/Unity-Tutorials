using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDrive : MonoBehaviour
{
    public float speed;
    public float turnSpeed;
    public float gravityMultiplier;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Accelerate();
        Turn();
        Fall();
    }

    void Accelerate ()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddRelativeForce(new Vector3(Vector3.forward.x, 0, Vector3.forward.z) * speed * 10);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.AddRelativeForce(-new Vector3(Vector3.forward.x, 0, Vector3.forward.z) * speed * 10);
        }

        Vector3 locVel = transform.InverseTransformDirection(rb.velocity);
        locVel = new Vector3(0, locVel.y, locVel.z);
        rb.velocity = transform.TransformDirection(locVel);
    }

    void Turn ()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(-transform.up * turnSpeed * 10);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.AddTorque(transform.up * turnSpeed * 10);
        }
    }

    void Fall()
    {
        rb.AddForce(Vector3.down * gravityMultiplier * 10);
    }
}
