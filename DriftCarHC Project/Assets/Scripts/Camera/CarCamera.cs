using UnityEngine;

public class CarCamera : MonoBehaviour
{
    public float distance = 6.0f;
    public float height = 4.0f;
    public float damping = 5.0f;
    public bool smoothRotation = true;
    public bool followBehind = true;
    public float rotationDamping = 10.0f;

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
            Vector3 direction = transform.position - Camera.main.transform.position;
            if (direction != Vector3.zero)
            {
                Quaternion wantedRotation = Quaternion.LookRotation(direction, transform.up);
                
                Vector3 eulerRotation = wantedRotation.eulerAngles;
                eulerRotation.z = 0;  
                wantedRotation = Quaternion.Euler(eulerRotation);  
            
                Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
            }
        }
        else
        {
            Camera.main.transform.LookAt(transform, transform.up);
        }
    }

}