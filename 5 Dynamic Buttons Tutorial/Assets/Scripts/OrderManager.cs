using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    public int maxOrders;
    public GameObject[] orderTypes;
    public Button[] orderButtons;

    private int currOrders;
    private GameObject[] orders;
    private Transform ordersPanel;

    // Start is called before the first frame update
    void Start()
    {
        currOrders = 0;
        orders = new GameObject[maxOrders];
        ordersPanel = GameObject.Find("Orders Panel").transform;
    }

    public void OrderShape (int shapeNum)
    {
        orders[currOrders] = Instantiate(orderTypes[shapeNum], ordersPanel);
        orders[currOrders].transform.localPosition = new Vector2(0, 130 - 60 * currOrders);
        orders[currOrders].GetComponent<ShapeOrder>().orderNumber = currOrders;
        currOrders++;
        if (currOrders == maxOrders)
        {
            foreach(Button orderButton in orderButtons)
            {
                orderButton.interactable = false;
            }
        }
    }

    public void RemoveItem(GameObject order)
    {
        for(int i = order.GetComponent<ShapeOrder>().orderNumber; i < currOrders - 1; i++)
        {
            orders[i] = orders[i + 1];
            orders[i].GetComponent<ShapeOrder>().orderNumber--;
            orders[i].transform.localPosition = new Vector2(0, 130 - 60 * i);
        }
        orders[currOrders - 1] = null;
        currOrders--;
        if (currOrders == maxOrders - 1)
        { 
            foreach (Button orderButton in orderButtons)
            {
                orderButton.interactable = true;
            }
        }
        Destroy(order);
    }
}