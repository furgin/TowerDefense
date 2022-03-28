using Cinemachine;
using UnityEngine;

public class GameCameraController : MonoBehaviour
{
    private CinemachineFreeLook freeLook;
    private float speed;

    public void Awake()
    {
        this.freeLook = GetComponent<CinemachineFreeLook>();
        this.speed = freeLook.m_XAxis.m_MaxSpeed;
    }
    public void Update()
    {
        if (Input.GetMouseButton(1))
        {
            freeLook.m_XAxis.m_MaxSpeed = speed;
        }
        else
        {
            freeLook.m_XAxis.m_MaxSpeed = 0.0f;
        }
    }
}