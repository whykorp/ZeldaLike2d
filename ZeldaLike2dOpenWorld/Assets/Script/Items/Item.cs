
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]

public class Item : ScriptableObject
{
    public int id;
    public new string name;
    public string description;
    public Sprite image;
}
