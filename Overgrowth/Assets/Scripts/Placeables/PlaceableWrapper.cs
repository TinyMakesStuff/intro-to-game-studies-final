using UnityEngine;

public class PlaceableWrapper : MonoBehaviour
{
    public Placeable placeable;
    public PlantData plantData;
    public int health;

    public void Start()
    {
        health = placeable.health;
    }
}
