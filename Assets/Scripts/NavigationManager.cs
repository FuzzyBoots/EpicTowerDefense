using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    [SerializeField] Vector2 _xExtents = new Vector2(-47, -35);
    [SerializeField] Vector2 _yExtents = new Vector2(15, 25);
    [SerializeField] Vector2 _zExtents = new Vector2(-12, 3);

    [SerializeField] Vector3 _center = new Vector3(-35, 20, -6);

    [SerializeField] Camera _camera;

    [SerializeField] float _2dMoveSpeed = 1f;
    [SerializeField] float _zMoveSpeed = 50f;

    private void Start()
    {
        if (!_camera)
        {
            _camera = Camera.main;
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
     
                cameraPos.x = Mathf.Clamp(cameraPos.x, _xExtents.x, _xExtents.y);
                cameraPos.z = Mathf.Clamp(cameraPos.z, _zExtents.x, _zExtents.y);
                cameraPos.y = Mathf.Clamp(cameraPos.y, _yExtents.x, _yExtents.y);

                _camera.transform.position = cameraPos;
            }
        }
    }
}
