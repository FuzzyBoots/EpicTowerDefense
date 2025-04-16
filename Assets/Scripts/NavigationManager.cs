using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    [SerializeField] Vector2 xExtents = new Vector2(-47, -35);
    [SerializeField] Vector2 zExtents = new Vector2(-12, 3);

    [SerializeField] Vector3 _center = new Vector3(-35, 20, -6);

    [SerializeField] Camera _camera;

    [SerializeField] float _moveSpeed = 1f;

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

            // Debug.Log($"Movement: {xAxis}, {yAxis}");
            if (Math.Abs(xAxis) > 0.1f || Math.Abs(yAxis) > 0.1f) {
                Vector3 cameraPos = _camera.transform.position;
                cameraPos.x += xAxis * _moveSpeed * Time.deltaTime;
                cameraPos.z += yAxis * _moveSpeed * Time.deltaTime;
                cameraPos.x = Mathf.Clamp(cameraPos.x, xExtents.x, xExtents.y);
                cameraPos.z = Mathf.Clamp(cameraPos.z, zExtents.x, zExtents.y);

                Debug.Log(cameraPos);
                _camera.transform.position = cameraPos;
            }
        }
    }
}
