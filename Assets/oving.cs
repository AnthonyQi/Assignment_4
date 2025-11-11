using UnityEngine;
using Unity.Mathematics;

public class oving : MonoBehaviour
{

    Rigidbody rb;
    private double uno = 0.0;
    private double dos = 0.0;
    private double tres = 0.0;
    public int tim = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        tim += 1;
        if (tim < 25)
        {
            tres -= 0.01;
            transform.position += new Vector3((float)uno,(float)dos, (float)tres);
        }
    }
}
