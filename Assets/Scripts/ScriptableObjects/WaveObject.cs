using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/SpawnWave", order = 1)]
public class WaveObject : ScriptableObject
{
    public string _waveName = "Wave";
    public SpawnObject[] _spawns;
}
