using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float rotSpeed;
    public bool lockCursor;

    private float rotX;
    private float rotY;

    // Start is called before the first frame update
    void Start()
    {
        rotX = transform.rotation.eulerAngles.x;
        rotY = transform.rotation.eulerAngles.y;
        if (lockCursor)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Update is called once per frame
    void Update()
    {
        rotX += -Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;
        rotX = Mathf.Clamp(rotX, -85, 85);
        transform.rotation = Quaternion.Euler(new Vector3(rotX, rotY, 0));
    }
}
