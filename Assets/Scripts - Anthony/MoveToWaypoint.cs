using UnityEngine;

public class MoveToWaypoint : MonoBehaviour
{
    public Transform target;  // assign your last waypoint here
    public float speed = 5f;
    public float stopDistance = 0.1f;  // how close before it "reaches" the target

    void Update()
    {
        if (target == null) return;

        // Move toward the target
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        // Rotate to face the target
        Vector3 direction = target.position - transform.position;
        if (direction.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(direction);

        // Check if reached target
        if (Vector3.Distance(transform.position, target.position) <= stopDistance)
        {
            Destroy(gameObject); // kill itself
        }
    }
}
