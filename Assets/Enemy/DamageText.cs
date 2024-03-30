using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public float duration = 50f;
    public float moveSpeed = 1f;
    public TextMeshPro damageText;
   

    public void SpawnText(int damageAmount)
    {
        damageText.text = damageAmount.ToString();
    }

    private void Update()
    {
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            Destroy(gameObject);
        }
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
