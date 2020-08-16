using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed;
    public float smoothing;

    private bool inputU;
    private bool inputD;
    private bool inputL;
    private bool inputR;

    private Vector2 dir;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        inputU = Input.GetKey(KeyCode.W);
        inputD = Input.GetKey(KeyCode.S);
        inputL = Input.GetKey(KeyCode.A);
        inputR = Input.GetKey(KeyCode.D);

        if (inputU && inputD || !inputU && !inputD)
        {
            dir.y = dir.y / 2f;
        }
        else
        {
            if (inputU)
            {
                dir.y = 1;
            }
            if (inputD)
            {
                dir.y = -1;
            }
        }

        if (inputL && inputR || !inputL && !inputR)
        {
            dir.x = dir.x / 2f;
        }
        else
        {
            if (inputL)
            {
                dir.x = -1;
            }
            if (inputR)
            {
                dir.x = 1;
            }
        }
        if (dir.magnitude > 1)
        {
            dir = dir.normalized;
        }
        rb.velocity = Vector2.Lerp(rb.velocity, dir * moveSpeed, smoothing);
    }
}
