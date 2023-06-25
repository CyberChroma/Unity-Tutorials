using UnityEngine;

public class ShapeOrder : MonoBehaviour {
    [HideInInspector] public int orderNumber;

    public void RemoveItem() {
        FindAnyObjectByType<OrderManager>().RemoveItem(gameObject);
    }
}
