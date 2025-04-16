using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseObject : ScriptableObject
{
    [SerializeField] public int _cost;
    [SerializeField] DefenseObject _upgrade;
}
