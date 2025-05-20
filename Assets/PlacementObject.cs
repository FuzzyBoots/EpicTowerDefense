using QFSW.QC.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementObject : MonoBehaviour
{
    [SerializeField] Turret _turretObject;
    public Turret TurretObject { get { return _turretObject; } set { _turretObject = value; } }
}
