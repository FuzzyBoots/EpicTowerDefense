using CodeMonkey.Toolkit.TMousePosition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

        _placementPrefab = incoming;

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
        PlacementObject placementPoint = null;
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
                _placementObject.transform.position = MousePositionPlane.GetPosition();
                _isValid = false;
            }

            if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
            {
                if (_isValid && placementPoint != null)
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
