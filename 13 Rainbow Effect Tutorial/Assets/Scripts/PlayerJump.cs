using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpPower;
    public float lowJumpMultiplier;
    public float fallMultiplier;
    public float gravityMultiplier;

    private bool canJump;
    private Rigidbody rb;
    private PlayerGroundCheck playerGroundCheck;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerGroundCheck = GetComponentInChildren<PlayerGroundCheck>();
    }

    void FixedUpdate()
    {
        Fall();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void Jump()
    {
        if (playerGroundCheck.isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpPower * 10, ForceMode.Impulse);
            playerGroundCheck.isGrounded = false;
        }
    }

    void Fall()
    {
        if (!playerGroundCheck.isGrounded)
        {
            if (rb.velocity.y >= 0 && !Input.GetKey(KeyCode.Space))
            {
                rb.AddForce(transform.up * -lowJumpMultiplier * 10);
            }
            else if (rb.velocity.y < 0)
            {
                rb.AddForce(transform.up * -fallMultiplier * 10);
            }
            rb.AddForce(transform.up * -gravityMultiplier * 10);
        }
    }
}
