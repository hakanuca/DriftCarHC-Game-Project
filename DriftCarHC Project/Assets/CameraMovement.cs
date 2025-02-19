using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public Transform startPosition;
    public Transform targetPosition;
    public float moveDuration = 1.5f;

    public void MoveToTarget()
    {
        transform.DOMove(targetPosition.position, moveDuration);
    }

    public void MoveToStart()
    {
        transform.DOMove(startPosition.position, moveDuration);
    }
}