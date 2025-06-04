using CodeMonkey.Toolkit.TMousePosition;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] GameObject _placementPrefab;

    [SerializeField] Shader _invalidEffect;
    [SerializeField] Shader _validEffect;

    GameObject _placementObject;
    
    bool _isValid;
    public bool IsValid { get { return _isValid; } 
        set { 
            if (_isValid != value)
            {
                // Change shader
                foreach (Renderer renderer in GetComponents<Renderer>())
                {
                    foreach (Material material in renderer.materials)
                    {
                        material.shader = value ? _validEffect : _invalidEffect;
                    }
                }

                foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
                {
                    foreach (Material material in renderer.materials)
                    {
                        material.shader = value ? _validEffect : _invalidEffect;
                    }
                }
            }
            _isValid = value; 
        } 
    }

    [SerializeField] private LayerMask _mouseColliderLayerMask;

    void Start()
    {
        _placementObject = Instantiate(_placementPrefab);
    }

    public void SetPlacementObject(GameObject placementObject)
    {
        _placementObject = placementObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (_placementObject != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _mouseColliderLayerMask))
            {
                IsValid = DeterminePlacementValidity();
                _placementObject.transform.position = raycastHit.collider.gameObject.transform.position;                
            }
            else
            {
                IsValid = false;
                _placementObject.transform.position = MousePositionPlane.GetPosition();
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (_isValid)
                {
                    Instantiate(_placementPrefab, _placementObject.transform.position, Quaternion.identity);
                }
            }
        }
    }

    private bool DeterminePlacementValidity()
    {
        throw new NotImplementedException();
    }
}
