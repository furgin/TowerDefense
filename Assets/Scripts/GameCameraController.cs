using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameCameraController : MonoBehaviour
{
    private Vector2 startAxis;

    // Update is called once per frame
    void Update()
    {
        Camera mainCamera = Camera.main;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        if (Input.GetKey(KeyCode.Space) && mainCamera)
        {
            mainCamera.transform.RotateAround(Vector3.zero, Vector3.up,
                (Input.GetAxis("Mouse X") - startAxis.x));
        }
    }
}