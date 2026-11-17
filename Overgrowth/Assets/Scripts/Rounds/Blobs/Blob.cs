using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Blob", menuName = "Scriptable Objects/Blob")]
public class Blob : ScriptableObject
{
    public List<GameObject> enemies;
    public List<int> amount;
    public int typeCount;
    
    public List<string> spawnType;
    public List<int> xCenter;
    public List<int> zCenter;
}
