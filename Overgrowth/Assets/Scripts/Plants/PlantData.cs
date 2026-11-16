using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantData", menuName = "Scriptable Objects/PlantData")]
public class PlantData : ScriptableObject
{
    public int index;
    public string plant;
    public string effect;
    public int effectAmount;
    public int cost;
    public Sprite plantIcon;
}
