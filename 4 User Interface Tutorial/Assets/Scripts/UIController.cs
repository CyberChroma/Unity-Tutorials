using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject cube;
    public Light mainLight;
    public Camera mainCamera;

    public Material blue;
    public Material green;
    public Material red;

    public Toggle activeToggle;
    public Slider slider;
    public Scrollbar scrollbar;
    public Dropdown dropdown;
    public InputField inputField; 
    public Text titleText;

    public void Button()
    {
        SceneManager.LoadScene(0);
    }

    public void Toggle()
    {
        cube.SetActive(activeToggle.isOn);
    }

    public void Slider()
    {
        mainLight.intensity = slider.value;
    }

    public void Scrollbar ()
    {
        mainCamera.fieldOfView = scrollbar.value * 100;
    }

    public void Dropdown()
    {
        if (dropdown.value == 0)
        {
            cube.GetComponent<MeshRenderer>().material = blue;
        }
        else if (dropdown.value == 1)
        {
            cube.GetComponent<MeshRenderer>().material = green;
        }
        else
        {
            cube.GetComponent<MeshRenderer>().material = red;
        }
    }

    public void InputField ()
    {
        titleText.text = inputField.text;
    }
}