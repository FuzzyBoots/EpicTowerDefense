using CodeMonkey.Toolkit.TMousePosition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] Turret _placementPrefab;

    [SerializeField] Shader _invalidEffect;
    [SerializeField] Shader _validEffect;

    Turret _placementObject;
    
    bool _isValid;
    [SerializeField] private LayerMask _mouseColliderLayerMask;

    void Start()
    {
        if (_placementPrefab != null)
        {
            _placementObject = Instantiate(_placementPrefab);
        }
    }

    public void SetPlacementObject(Turret incoming)
    {
        Debug.Log("Attempting to change placement object to " + incoming);
        if (_placementObject)
        {
            Destroy(_placementObject);
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
        PlacementObject placementPoint = null;
        if (_placementObject != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _mouseColliderLayerMask))
            {
                if (raycastHit.collider.gameObject.TryGetComponent<PlacementObject>(out placementPoint))
                {
                    _placementObject.transform.position = raycastHit.collider.gameObject.transform.position;
                    _isValid = placementPoint.TurretObject == null;
                }
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
                    Turret placed = Instantiate(_placementPrefab, _placementObject.transform.position, Quaternion.identity);
                    placementPoint.TurretObject = placed;
                    placed.SetActive(true);
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                SetPlacementObject(null);
                // Contact UI Manager to un-highlight?
            }
        }
    }
}
