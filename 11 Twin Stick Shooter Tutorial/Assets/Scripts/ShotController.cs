using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    public float moveSpeed;
    public float destroyTime;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = transform.up * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Environment"))
        {
            Destroy(gameObject);
            // Spawn cool explosion!
        }
        else if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
            // Spawn cool explosion!
        }
    }

}
