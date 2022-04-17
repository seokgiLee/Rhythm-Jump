using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaerManager : MonoBehaviour
{
    public CinemachineTargetGroup target;
    public CinemachineVirtualCamera vCam;
    float curSize; // ÇöÀç ÁÜÅ©±â
    float zoomSize; // ¸ñÇ¥ ÁÜÀÎ,ÁÜ¾Æ¿ô Å©±â
    bool zoom = false; // ÁÜ ½ÇÇà ¿©ºÎ

    void Awake()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        curSize = vCam.m_Lens.OrthographicSize;
    }

    void Update()
    {
        if (zoom)
        {
            if (zoomSize > curSize) // ÁÜ¾Æ¿ô
            {
                vCam.m_Lens.OrthographicSize += 0.05f;
                if (vCam.m_Lens.OrthographicSize >= zoomSize)
                {
                    zoom = false;
                    vCam.m_Lens.OrthographicSize = Mathf.FloorToInt(vCam.m_Lens.OrthographicSize);
                    curSize = zoomSize;
                }
            }
            else if (zoomSize < curSize) // ÁÜÀÎ
            {
                vCam.m_Lens.OrthographicSize -= 0.05f;
                if (vCam.m_Lens.OrthographicSize <= zoomSize)
                {
                    zoom = false;
                    vCam.m_Lens.OrthographicSize = Mathf.FloorToInt(vCam.m_Lens.OrthographicSize);
                    curSize = zoomSize;
                }
            }
        }
    }

    public void Zoom(float size)
    {
        zoomSize = size;
        zoom = true;
    }
}
