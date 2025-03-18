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
    private Coroutine moveCoroutine; // Reference to the current movement coroutine

    private void Start()
    {
        // Initialize camera to start position
        transform.position = homePosition.position;
        transform.rotation = homePosition.rotation;
    }

    // Move to Garage View
    public void MoveToGarageView()
    {
        StartNewMovement(garagePosition);
    }

    // Move to Levels View
    public void MoveToLevelsView()
    {
        StartNewMovement(levelsPosition);
    }

    // Move to Home View
    public void MoveToHomeView()
    {
        StartNewMovement(homePosition);
    }

    // Move to Home Reverse View
    public void MoveToHomeReverseView()
    {
        StartNewMovement(homeReversePosition);
    }

    // Toggle between Home and Home Reverse
    public void ToggleHomeView()
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

    // Start a new movement, stopping any ongoing movement
    private void StartNewMovement(Transform destination)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine); // Stop the previous movement if it's running
        }
        moveCoroutine = StartCoroutine(MoveCamera(destination)); // Start the new movement
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
