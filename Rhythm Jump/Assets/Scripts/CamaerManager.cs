using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaerManager : MonoBehaviour
{
    public CinemachineTargetGroup target;
    public CinemachineVirtualCamera vCam;
    float curSize; // ���� ��ũ��
    float zoomSize; // ��ǥ ����,�ܾƿ� ũ��
    bool zoom = false; // �� ���� ����

    void Awake()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        curSize = vCam.m_Lens.OrthographicSize;
    }

    void Update()
    {
        if (zoom)
        {
            if (zoomSize > curSize) // �ܾƿ�
            {
                vCam.m_Lens.OrthographicSize += 0.05f;
                if (vCam.m_Lens.OrthographicSize >= zoomSize)
                {
                    zoom = false;
                    vCam.m_Lens.OrthographicSize = Mathf.FloorToInt(vCam.m_Lens.OrthographicSize);
                    curSize = zoomSize;
                }
            }
            else if (zoomSize < curSize) // ����
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
