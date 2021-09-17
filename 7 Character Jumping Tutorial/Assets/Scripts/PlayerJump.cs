using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpForce;
    public float gravityMultiplier;
    public float fallMultiplier;
    public float lowJumpMultiplier;

    private Rigidbody rb;
    private PlayerGroundCheck playerGroundCheck;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerGroundCheck = GetComponentInChildren<PlayerGroundCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
    }

    void FixedUpdate()
    {
        Fall();
    }

    void Jump ()
    {
        if (playerGroundCheck.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce * 10, ForceMode.Impulse);
        }
    }

    void Fall ()
    {
        if (!playerGroundCheck.isGrounded)
        {
            if (rb.velocity.y >= 0 && !Input.GetKey(KeyCode.Space))
            {
                rb.AddForce(Vector3.down * lowJumpMultiplier * 10);
            } else if (rb.velocity.y < 0)
            {
                rb.AddForce(Vector3.down * fallMultiplier * 10);
            }

            rb.AddForce(Vector3.down * gravityMultiplier * 10);
        }
    }
}
