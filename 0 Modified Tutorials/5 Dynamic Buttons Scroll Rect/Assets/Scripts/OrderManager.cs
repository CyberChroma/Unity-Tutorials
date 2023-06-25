using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour {
    public int maxOrders;
    public GameObject[] orderTypes;
    public Button[] orderButtons;
    public Transform ordersParent;

    private int currOrders;
    private GameObject[] orders;

    // Start is called before the first frame update
    void Start() {
        currOrders = 0;
        orders = new GameObject[maxOrders];
    }

    public void OrderShape(int shapeNum) {
        orders[currOrders] = Instantiate(orderTypes[shapeNum], ordersParent);
        orders[currOrders].transform.localPosition = Vector2.zero;
        orders[currOrders].GetComponent<ShapeOrder>().orderNumber = currOrders;
        currOrders++;
        if (currOrders == maxOrders) {
            foreach (Button orderButton in orderButtons) {
                orderButton.interactable = false;
            }
        }
    }

    public void RemoveItem(GameObject order) {
        for (int i = order.GetComponent<ShapeOrder>().orderNumber; i < currOrders - 1; i++) {
            orders[i] = orders[i + 1];
            orders[i].GetComponent<ShapeOrder>().orderNumber--;
            orders[i].transform.localPosition = Vector2.zero;
        }
        orders[currOrders - 1] = null;
        currOrders--;
        if (currOrders == maxOrders - 1) {
            foreach (Button orderButton in orderButtons) {
                orderButton.interactable = true;
            }
        }
        Destroy(order);
    }
}