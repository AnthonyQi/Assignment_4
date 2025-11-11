using UnityEngine;

public class RainSystem : MonoBehaviour
{
    [Header("Rain Settings")]
    public ParticleSystem rainParticles;
    public bool isRaining = true;
    [Range(0f, 1f)] public float rainIntensity = 0.7f;
    
    [Header("Audio")]
    public AudioSource rainSound;
    
    [Header("Follow Player")]
    public Transform player;
    public Vector3 offset = new Vector3(0, 10, 0);
    
    private ParticleSystem.EmissionModule emission;
    private float maxEmissionRate = 1000f;
    
    void Start()
    {
        if (rainParticles == null)
        {
            Debug.LogError("Rain Particle System not assigned!");
            return;
        }
        
        emission = rainParticles.emission;
        UpdateRainIntensity();
    }
    
    void Update()
    {
        // Follow player so rain is always around them
        if (player != null)
        {
            transform.position = player.position + offset;
        }
        
        // Toggle rain with R key (optional)
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleRain();
        }
    }
    
    public void SetRainIntensity(float intensity)
    {
        rainIntensity = Mathf.Clamp01(intensity);
        UpdateRainIntensity();
    }
    
    void UpdateRainIntensity()
    {
        if (rainParticles == null) return;
        
        emission.rateOverTime = maxEmissionRate * rainIntensity;
        
        if (rainSound != null)
        {
            rainSound.volume = rainIntensity * 0.5f;
        }
    }
    
    public void ToggleRain()
    {
        isRaining = !isRaining;
        
        if (isRaining)
        {
            rainParticles.Play();
            if (rainSound != null) rainSound.Play();
        }
        else
        {
            rainParticles.Stop();
            if (rainSound != null) rainSound.Stop();
        }
    }
    
    public void StartRain()
    {
        isRaining = true;
        rainParticles.Play();
        if (rainSound != null) rainSound.Play();
    }
    
    public void StopRain()
    {
        isRaining = false;
        rainParticles.Stop();
        if (rainSound != null) rainSound.Stop();
    }
}