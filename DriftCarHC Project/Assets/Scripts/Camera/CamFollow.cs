using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 lookAtOffset;

    private void LateUpdate()
    {
        if (playerTransform != null)
        {
            transform.position = playerTransform.position + offset;

            transform.LookAt(playerTransform.position + lookAtOffset);
        }
    }
}