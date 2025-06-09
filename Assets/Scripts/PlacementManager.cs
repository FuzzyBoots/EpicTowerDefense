using CodeMonkey.Toolkit.TMousePosition;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] Emplacement _placementPrefab;

    [SerializeField] Shader _invalidEffect;
    [SerializeField] Shader _validEffect;

    Emplacement _placementObject;
    
    bool _isValid;
    public bool IsValid { get { return _isValid; } 
        set { 
            if (_isValid != value)
            {
                // Change shader
                foreach (Renderer renderer in _placementObject.GetComponents<Renderer>())
                {
                    foreach (Material material in renderer.materials)
                    {
                        material.shader = value ? _validEffect : _invalidEffect;
                    }
                }

                foreach (Renderer renderer in _placementObject.GetComponentsInChildren<Renderer>())
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
        if (_placementPrefab != null)
        {
            SetPlacementObject(_placementPrefab);
        }
    }

    public void SetPlacementObject(Emplacement incoming)
    {
        Debug.Log("Attempting to change placement object to " + incoming);
        if (_placementObject)
        {
            Destroy(_placementObject.gameObject);
        }

        _placementPrefab = incoming;

        if (incoming == null)
        {
            Debug.Log("Null set");
            return;
        }

        Debug.Log("Setting placement object to " + incoming.name, incoming);
        _placementObject = Instantiate(_placementPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        if (_placementObject != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _mouseColliderLayerMask))
            {
                GameObject target = raycastHit.collider.gameObject;
                PlacementObject targetObject = target.GetComponent<PlacementObject>();
                IsValid = DeterminePlacementValidity(targetObject);
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
                    Emplacement placed = Instantiate(_placementPrefab, _placementObject.transform.position, Quaternion.identity);
                    placed.SetActive(true);
                    GameManager.Instance.ModifyCash(-_placementPrefab.Cost);
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                SetPlacementObject(null);
                // Contact UI Manager to un-highlight?
            }
        }
    }

    private bool DeterminePlacementValidity(PlacementObject targetObject)
    {
        return (targetObject != null && targetObject.TurretObject == null && _placementObject.Cost <= GameManager.Instance.Cash);
    }
}
