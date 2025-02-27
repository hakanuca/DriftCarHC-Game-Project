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
    }


    public void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}