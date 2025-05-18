using CodeMonkey.Toolkit.TMousePosition;
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
    [SerializeField] private LayerMask _mouseColliderLayerMask;

    void Start()
    {
        _placementObject = Instantiate(_placementPrefab);
    }

    public void SetPlacementObject(GameObject placementObject)
    {
        _placementObject = placementObject;
    }

    // Start is called before the first frame update
    //void Start()
    //{
    //    Debug.Log("Entering Start");

    //    _invalidPlacement = Instantiate(_placementObject);

    //    foreach(Renderer renderer in renderers)
    //    {
    //        // Create a new instance of the material(s)
    //        Material[] newMaterials = new Material[renderer.sharedMaterials.Length];
    //        for (int i = 0; i < renderer.sharedMaterials.Length; i++)
    //        {
    //            newMaterials[i] = new Material(renderer.sharedMaterials[i]); // Create a new Material instance
    //        }

    //        // Assign the new materials to the clone's renderer
    //        renderer.materials = newMaterials;
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        //// We actually only want to do this on change
        //foreach()
        //{
        //    mat.shader = _isValid ? _validEffect : _invalidEffect;
        //}

        if (_placementObject != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _mouseColliderLayerMask))
            {
                _placementObject.transform.position = raycastHit.collider.gameObject.transform.position;
                _isValid = true;
            }
            else
            {
                _placementObject.transform.position = MousePositionPlane.GetPosition();
                _isValid = false;
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
}
