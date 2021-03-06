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
        var right = new Vector3(dir.z, dir.y, -dir.x);
        var left = -right;
        
        if (Input.GetKey(KeyCode.W))
        {
            velocity += dir;
        }
        if (Input.GetKey(KeyCode.A))
        {
            velocity += left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity += right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            velocity += -dir;
        }
        
        transform.localPosition += velocity * Speed * Time.deltaTime;
        velocity *= 0.8f;
        if (velocity.magnitude < 0.1f)
        {
            velocity = Vector3.zero;
        }

    }
}
