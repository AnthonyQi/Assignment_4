using UnityEngine;

public class OrbFloat : MonoBehaviour
{
    public float amplitude = 0.25f;
    public float frequency = 1f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * frequency) * amplitude;
    }
}
