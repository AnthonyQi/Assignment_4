using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform camPos;

    void LateUpdate()
    {
        if (camPos != null)
            transform.position = camPos.position;
    }
}
