using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Light))]
public class Flashlight : MonoBehaviour
{
    [Header("References")]
    public Transform playerCamera;   // Assign your Camera here

    [Header("Light Settings")]
    public float intensity = 2f;
    public float range = 20f;
    public float spotAngle = 30f;
    public bool enableFlicker = false;
    public float flickerAmount = 0.2f; // max +/- change in intensity per frame

    private Light flash;
    private bool isOn = true;

    void Awake()
    {
        flash = GetComponent<Light>();
        flash.type = LightType.Spot;
        flash.intensity = intensity;
        flash.range = range;
        flash.spotAngle = spotAngle;
        flash.enabled = isOn;
    }

    void Update()
    {
        // Make flashlight follow the camera
        if (playerCamera != null)
        {
            transform.position = playerCamera.position;
            transform.rotation = playerCamera.rotation;
        }

        // Toggle flashlight with "F" key
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            isOn = !isOn;
            flash.enabled = isOn;
        }

        // Optional flicker
        if (enableFlicker && isOn)
        {
            flash.intensity = intensity + Random.Range(-flickerAmount, flickerAmount);
        }
    }
}
