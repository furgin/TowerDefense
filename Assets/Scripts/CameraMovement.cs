using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    public float Speed = 1.0f;
    [SerializeField]
    public float SpeedDecay = 0.5f;

    private Vector3 velocity;
    
    void Update()
    {
        var fwd = Camera.main.transform.forward;
        var dir = new Vector3(fwd.x, 0, fwd.z).normalized;

        if (Input.GetKey(KeyCode.W))
        {
            velocity += dir;
        }
        
        transform.localPosition += velocity * Speed * Time.deltaTime;
        velocity *= 0.8f;
        if (velocity.magnitude < 0.1f)
        {
            velocity = Vector3.zero;
        }

    }
}
