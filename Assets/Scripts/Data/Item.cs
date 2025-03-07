using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/Item", order = int.MaxValue)]
public class Item : ScriptableObject
{
    public ItemDefine.ItemType type;
    public Sprite sprite;
    public string information;
}
