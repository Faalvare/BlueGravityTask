using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineConfiner2D confiner;
    // Start is called before the first frame update
    void Start()
    {
        PolygonCollider2D CameraBoundaries = GameObject.Find("CameraBoundaries").GetComponent<PolygonCollider2D>();
        GameObject player = GameObject.Find("Player");
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        confiner = GetComponent<CinemachineConfiner2D>();
        virtualCamera.Follow = player.transform;
        confiner.m_BoundingShape2D = CameraBoundaries;
    }
}
