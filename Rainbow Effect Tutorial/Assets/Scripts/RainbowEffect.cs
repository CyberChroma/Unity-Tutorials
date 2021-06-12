using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowEffect : MonoBehaviour
{
    public float rainbowSpeed;
    public bool invert;
    public bool randomize;

    private float hue;
    private float sat;
    private float bri;
    private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (randomize)
        {
            hue = Random.Range(0f, 1f);
        }
        sat = 1;
        bri = 1;
        meshRenderer.material.color = Color.HSVToRGB(hue, sat, bri);
    }

    // Update is called once per frame
    void Update()
    {
        Color.RGBToHSV(meshRenderer.material.color, out hue, out sat, out bri);
        if (invert)
        {
            hue -= rainbowSpeed / 10000;
            if (hue <= 0)
            {
                hue = 0.99f;
            }
        }
        else
        {
            hue += rainbowSpeed / 10000;
            if (hue >= 1)
            {
                hue = 0;
            }
        }
        meshRenderer.material.color = Color.HSVToRGB(hue, sat, bri);
    }
}
