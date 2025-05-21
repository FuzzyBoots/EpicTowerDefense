using QFSW.QC.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementObject : MonoBehaviour
{
    [SerializeField] Emplacement _turretObject;
    public Emplacement TurretObject { get { return _turretObject; } set { _turretObject = value; } }
}
