using Cinemachine;
using UnityEngine;

public class GameCameraController : MonoBehaviour
{
    public void Update()
    {
        var freeLook = GetComponent<CinemachineFreeLook>();
        if (Input.GetKey(KeyCode.Space))
        {
            freeLook.m_XAxis.m_MaxSpeed = 500.0f;
        }
        else
        {
            freeLook.m_XAxis.m_MaxSpeed = 0.0f;
        }
    }
}