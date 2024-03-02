using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    // Start is called before the first frame update

    public Camera cam;
    private float xRotation;

    public float xSensitivity;
    public float ySensitivity;


    public void Look(Vector2 lookInput)
    {
        float mouseX = lookInput.x;
        float mouseY = lookInput.y;

        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }

}
