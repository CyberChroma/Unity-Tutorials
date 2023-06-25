using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeOrder : MonoBehaviour
{
    [HideInInspector] public int orderNumber;

    public void RemoveItem()
    {
        FindAnyObjectByType<OrderManager>().RemoveItem(gameObject);
    }
}
