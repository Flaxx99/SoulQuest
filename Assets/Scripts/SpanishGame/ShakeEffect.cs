using UnityEngine;

public class ShakeEffect : MonoBehaviour
{
    public float shakeIntensity = 0.1f;
    public float shakeDuration = 0.4f;
    private Vector3 originalPosition;
    private bool isShaking = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        if (isShaking)
        {
            // Realizamos el temblor en el libro
            float shakeAmountX = Random.Range(-shakeIntensity, shakeIntensity);
            float shakeAmountY = Random.Range(-shakeIntensity, shakeIntensity);
            transform.position = originalPosition + new Vector3(shakeAmountX, shakeAmountY, 0);
        }
    }

    // Función pública para iniciar el shake
    public void Shake()
    {
        if (!isShaking)
        {
            isShaking = true;
            Invoke("StopShake", shakeDuration); // Detener después de la duración del shake
        }
    }

    // Detener el shake
    void StopShake()
    {
        isShaking = false;
        transform.position = originalPosition;
    }
}
