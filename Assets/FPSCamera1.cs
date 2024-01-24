using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera1 : MonoBehaviour
{
    // Start is called before the first frame update

    public float mouseSensitivity;
    public Transform player;

    private float xRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Look(Vector2 lookInput)
    {
        Debug.Log("looking");
      
    }
  
}
