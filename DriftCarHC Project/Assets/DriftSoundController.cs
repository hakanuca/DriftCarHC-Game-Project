using UnityEngine;

public class DriftSoundController : MonoBehaviour
{
    public CarController carController;
    public AudioSource driftAudioSource;

    [Header("Pitch Ayarı")]
    public float minPitch = 0.9f;
    public float maxPitch = 1.3f;
    public float maxDriftAngle = 45f;

    [Header("Kontrol")]
    public float driftPlayDelay = 0.4f; // Saniye cinsinden, sesin tekrar oynatılması süresi
    private float driftTimer;

    void Update()
    {
        if (carController == null || driftAudioSource == null)
            return;

        bool drifting = carController.IsDrifting();

        if (drifting)
        {
            driftTimer += Time.deltaTime;

            if (driftTimer >= driftPlayDelay)
            {
                // Farklı pitch ile aynı sesi tekrar çal
                float driftAngle = Mathf.Abs(carController.GetDriftAngle());
                float pitch = Mathf.Lerp(minPitch, maxPitch, driftAngle / maxDriftAngle);
                driftAudioSource.pitch = pitch;

                driftAudioSource.Play();
                driftTimer = 0;
            }
        }
        else
        {
            driftAudioSource.Stop();
            driftTimer = driftPlayDelay;
        }
    }
}