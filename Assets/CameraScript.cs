using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScript : MonoBehaviour
{
    CinemachineVirtualCamera _vcam;
    Camera _cam;
    float _minFov;
    float _maxFov;
    float _minAngle = 55;
    float _maxAngle = 82;

    void Awake()
    {
        _vcam = GetComponent<CinemachineVirtualCamera>();
        _cam = Camera.main;
    }

    void Start()
    {
        _minFov = _vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_MinimumFOV;
        _maxFov = _vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_MaximumFOV;
    }

    void Update()
    {
        float mario = Mathf.Lerp(_minAngle, _maxAngle, Mathf.InverseLerp(_minFov, _maxFov, _cam.fieldOfView));

        _vcam.transform.rotation = Quaternion.Euler(new Vector3
        (mario, _vcam.transform.rotation.eulerAngles.y, _vcam.transform.rotation.eulerAngles.z));
    }
}
