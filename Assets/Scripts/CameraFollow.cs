using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float followThreshold = 2f;

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 targetPosition = transform.position;
        float distance = player.position.x - transform.position.x;

        if (Mathf.Abs(distance) > followThreshold)
        {
            targetPosition.x = Mathf.Lerp(transform.position.x, player.position.x, smoothSpeed);
        }

        transform.position = new Vector3(targetPosition.x, transform.position.y, transform.position.z) + offset;
    }
}
