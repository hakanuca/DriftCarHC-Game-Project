using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Camera Positions")]
    public Transform homePosition;
    public Transform levelsPosition;
    public Transform garagePosition;
    public Transform homeReversePosition;

    [Header("Movement Settings")]
    public float moveDuration = 1.5f;
    private bool isMoving = false;
    private Vector3 startPos;
    private Quaternion startRot;
    private Vector3 endPos;
    private Quaternion endRot;
    private float timeElapsed = 0f;

    private bool isHomeReverse = true; // Flag to track toggle state

    private void Start()
    {
        // Initialize camera to start position
        transform.position = homePosition.position;
        transform.rotation = homePosition.rotation;
    }

    // Move to Garage View
    public void MoveToGarageView()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveCamera(garagePosition));
        }
    }

    // Move to Levels View
    public void MoveToLevelsView()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveCamera(levelsPosition));
        }
    }

    // Move to Home View
    public void MoveToHomeView()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveCamera(homePosition));
        }
    }

    // Move to Home Reverse View
    public void MoveToHomeReverseView()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveCamera(homeReversePosition));
        }
    }

    // Toggle between Home and Home Reverse
    public void ToggleHomeView()
    {
        if (!isMoving)
        {
            if (isHomeReverse)
            {
                MoveToHomeReverseView();
            }
            else
            {
                MoveToHomeView();
            }
            isHomeReverse = !isHomeReverse; // Toggle state
        }
    }

    private System.Collections.IEnumerator MoveCamera(Transform destination)
    {
        isMoving = true;
        timeElapsed = 0f;
        startPos = transform.position;
        startRot = transform.rotation;
        endPos = destination.position;
        endRot = destination.rotation;

        while (timeElapsed < moveDuration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / moveDuration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.rotation = Quaternion.Lerp(startRot, endRot, t);
            yield return null;
        }

        // Ensure final position and rotation
        transform.position = endPos;
        transform.rotation = endRot;
        isMoving = false;
    }
}
