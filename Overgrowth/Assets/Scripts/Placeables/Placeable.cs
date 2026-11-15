using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Placeable", menuName = "Scriptable Objects/Placeable")]
public class Placeable : ScriptableObject
{
    // Main variables
    public string type;
    public List<int> cost;
    public int health;
}
