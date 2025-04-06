using CodeMonkey.Toolkit.TMousePosition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObject : MonoBehaviour
{
    [SerializeField] Transform _placement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_placement != null)
        {
            _placement.position = MousePositionPlane.GetPosition();
        }
    }
}
