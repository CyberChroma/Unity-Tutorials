using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    public int numCollectables;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Collectable"))
        {
            numCollectables++;
            collision.gameObject.SetActive(false);
        }
    }
}
