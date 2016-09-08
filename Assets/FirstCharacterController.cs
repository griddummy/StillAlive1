using UnityEngine;
using System.Collections;

public class FirstCharacterController : MonoBehaviour {

    public bool m_Zoom = false;
    public float ZoomScale = 2f;
    public Camera ZoomCamera;
    public GameObject Weapon;
    public Transform WeaponPosZoom;
    public Transform WeaponPosIdle;
    private float OriginFieldOfView;

    void Start()
    {
        OriginFieldOfView = ZoomCamera.fieldOfView;
    }
    void Update()
    {
        if(m_Zoom)
        {
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                m_Zoom = false;
                ZoomCamera.fieldOfView = OriginFieldOfView;
            }
        }
        else if (!m_Zoom)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                m_Zoom = true;
                ZoomCamera.fieldOfView = OriginFieldOfView / ZoomScale;
            }
        }
    }
}