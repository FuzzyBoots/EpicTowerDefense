using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    [SerializeField] Vector3 _center = new Vector3(-35, 20, -6);

    [SerializeField] Camera _camera;

    [SerializeField] float _2dMoveSpeed = 10f;
    [SerializeField] float _zMoveSpeed = 50f;

    private void Start()
    {
        if (!_camera)
        {
            _camera = Camera.main;
        }
    }

    Vector2 XExtents {  
        get { 
            float zPos = _camera.transform.localPosition.y;
            return new Vector2(zPos * 1.2f - 53.5f, zPos * -0.7f + 1f);
        } 
    }
    Vector2 YExtents
    {
        get
        {
            float zPos = _camera.transform.localPosition.y;
            return new Vector2(zPos * -0.8f - 9f, zPos * 1.2f + 21f);
        }
    }

    Vector2 ZExtents
    {
        get
        {
            return new Vector2(10, 15);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _camera.transform.position = _center;
        } else
        {
            float xAxis = Input.GetAxis("Horizontal");
            float yAxis = Input.GetAxis("Vertical");
            float zAxis = Input.GetAxis("Mouse ScrollWheel");

            if (Math.Abs(xAxis) > 0.1f || Math.Abs(yAxis) > 0.1f || Math.Abs(zAxis) > 0.1f) {
                Vector3 cameraPos = _camera.transform.position;
                cameraPos.x += xAxis * _2dMoveSpeed * Time.deltaTime;
                cameraPos.z += yAxis * _2dMoveSpeed * Time.deltaTime;
                cameraPos.y += zAxis * _zMoveSpeed * Time.deltaTime; 
     
                cameraPos.x = Mathf.Clamp(cameraPos.x, XExtents.x, XExtents.y);
                cameraPos.z = Mathf.Clamp(cameraPos.z, YExtents.x, YExtents.y);
                cameraPos.y = Mathf.Clamp(cameraPos.y, ZExtents.x, ZExtents.y);

                _camera.transform.position = cameraPos;
            }
        }
    }
}
