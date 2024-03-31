using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public float duration = 2f; // Adjusted for a typical duration
    public AnimationCurve movementCurve;
    private float startTime;
    private Vector3 startPosition;
    private float verticalOffset = 1f; // Vertical offset from the hit point
    private float horizontalRandomness = 0.75f; // Randomness in horizontal direction

    public TextMeshPro damageText;

    private void Awake()
    {
        startTime = Time.time;
        startPosition = transform.position + Vector3.up * verticalOffset;
        startPosition += Vector3.right * Random.Range(-horizontalRandomness, horizontalRandomness);
        transform.position = startPosition;
    }

    public void SpawnText(int damageAmount)
    {
        damageText.text = damageAmount.ToString();
       
    }

    private void Update()
    {
        float elapsed = Time.time - startTime;
        float normalizedTime = elapsed / duration;
        if (normalizedTime > 1f)
        {
            Destroy(gameObject);
            return;
        }

     
        transform.position = startPosition + Vector3.up * movementCurve.Evaluate(normalizedTime);
      
        FaceCamera();
    }

    private void FaceCamera()
    {
        if (Camera.main != null)
        {
            Transform cameraTransform = Camera.main.transform;
            transform.LookAt(transform.position + cameraTransform.rotation * Vector3.forward, cameraTransform.rotation * Vector3.up);
        }
    }
}
