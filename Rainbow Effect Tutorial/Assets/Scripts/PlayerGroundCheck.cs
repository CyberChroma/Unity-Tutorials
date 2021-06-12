using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    [HideInInspector] public bool isGrounded;

    void OnTriggerEnter(Collider collision)
    {
        if (!isGrounded && collision.gameObject.layer == 8)
        {
            isGrounded = true;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (isGrounded && collision.gameObject.layer == 8)
        {
            isGrounded = false;
        }
    }
}
