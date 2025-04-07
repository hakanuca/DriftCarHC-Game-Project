using UnityEngine;

public class MotorSoundController : MonoBehaviour
{
    public Rigidbody carRigidbody;        // Aracın Rigidbody'si
    public AudioSource motorAudioSource;  // Motor sesi
    public float minPitch = 0.8f;         // En düşük pitch
    public float maxPitch = 2.0f;         // En yüksek pitch
    public float maxSpeed = 100f;         // Maksimum hız (bu hızda maxPitch olacak)

    void Update()
    {
        float speed = carRigidbody.velocity.magnitude; // Hızı al
        float pitch = Mathf.Lerp(minPitch, maxPitch, speed / maxSpeed); // Pitch'i hesapla
        motorAudioSource.pitch = pitch;
        
        if (!motorAudioSource.isPlaying)
        {
            motorAudioSource.Play();
        }
    }
}