using UnityEngine;

public class CarCamera : MonoBehaviour
{
    public float distance = 6.0f;
    public float height = 4.0f;
    public float damping = 5.0f;
    public bool smoothRotation = true;
    public bool followBehind = true;
    public float rotationDamping = 10.0f;
    public Rigidbody carRigidbody;

    void FixedUpdate()
    {
        Vector3 wantedPosition;
        if (followBehind)
            wantedPosition = transform.TransformPoint(0, height, -distance);
        else
            wantedPosition = transform.TransformPoint(0, height, distance);

        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, wantedPosition, Time.deltaTime * damping);

        if (smoothRotation)
        {
            Quaternion wantedRotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position, transform.up);
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
        }
        else Camera.main.transform.LookAt(transform, transform.up);
    }
}