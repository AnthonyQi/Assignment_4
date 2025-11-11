using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class flicker : MonoBehaviour
{
    public Light light;
    public int tim;
    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        tim += 1;
        if (tim%5 == 0)
        {
            light.intensity = 0;
        }
        else
        {
            light.intensity = 1;
        }
        
        if (tim < 25)
        {
            rb.velocity = new Vector3(0f, 0f, -1f);
        }
        else
        {
            rb.velocity = new Vector3(0f, 0f, 0f);
        }

        if (tim > 70)
        {
            SceneManager.LoadScene("Scene_3_Laurynn");
        }
    }
}
