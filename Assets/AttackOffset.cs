using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackOffset : MonoBehaviour
{
    Vector3 _offset;
    public Vector3 Offset { get { return _offset; } private set { _offset = value; } }
}
