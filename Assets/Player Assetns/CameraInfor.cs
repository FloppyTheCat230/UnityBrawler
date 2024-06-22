using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraInfor : MonoBehaviour
{
    public float cameraDistance = 0f;
    public Vector3 centerScreenPoint;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        centerScreenPoint = Camera.main.ViewportToWorldPoint(new Vector3(0f, 50f, cameraDistance));
        // EXAMPLE IDEA transform.RotateAround(playerTransform.position, Vector3.up, Time.deltaTime * rotationSpeed);
    }

}
