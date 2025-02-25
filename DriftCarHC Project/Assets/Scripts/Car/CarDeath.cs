using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class CarDeath : MonoBehaviour
{
    public UnityEvent onDeath;
    public float collisionSpeedTolerance = 5f;
    Rigidbody _rb;
    int currentSection = 0;

    private void Start() {
        _rb = GetComponent<Rigidbody>();
    }

    public void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Crashable") {
            if (collision.impulse.magnitude / _rb.mass > collisionSpeedTolerance) {
                onDeath.Invoke();
            }
        }
        // else if (collision.gameObject.transform.parent.gameObject.tag == "Road")
        // {
        //     currentSection = collision.gameObject.transform.parent.gameObject.GetComponent<TrackSection>().number;
        // }
    }


    public void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}