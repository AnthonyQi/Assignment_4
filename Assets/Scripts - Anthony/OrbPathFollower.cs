using UnityEngine;

public class OrbFollowPath : MonoBehaviour
{
    [Header("Path Settings")]
    public Transform[] waypoints; // Assign empty objects along the path
    public float moveSpeed = 3f;
    public float stopDistance = 0.1f; // How close to a waypoint before moving to next

    [Header("Fade Settings")]
    public float fadeDuration = 2f; // Seconds to fade out at end
    public Light orbLight;          // Optional

    private int currentWaypoint = 0;
    private Material orbMaterial;
    private Color originalColor;
    private bool fading = false;
    private float fadeTimer = 0f;

    void Start()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogWarning("No waypoints assigned!");
            enabled = false;
            return;
        }

        orbMaterial = GetComponent<Renderer>().material;
        originalColor = orbMaterial.color;
    }

    void Update()
    {
        if (!fading)
        {
            MoveAlongPath();
        }
        else
        {
            FadeOut();
        }
    }

    void MoveAlongPath()
    {
        Transform target = waypoints[currentWaypoint];
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) <= stopDistance)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
            {
                // Start fading once we reach the last waypoint
                fading = true;
                fadeTimer = 0f;
            }
        }
    }

    void FadeOut()
    {
        fadeTimer += Time.deltaTime;
        float t = Mathf.Clamp01(fadeTimer / fadeDuration);

        // Fade material
        Color c = originalColor;
        c.a = Mathf.Lerp(originalColor.a, 0f, t);
        orbMaterial.color = c;

        // Fade light intensity
        if (orbLight != null)
            orbLight.intensity = Mathf.Lerp(orbLight.intensity, 0f, t);

        if (t >= 1f)
            Destroy(gameObject);
    }
}
