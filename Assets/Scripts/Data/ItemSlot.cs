using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField]
    private Item defaultItem;
    private Item item;

    public Item Item
    {
        get { return item; }
        set
        {
            item = value;
            this.transform.GetChild(0).GetComponent<Image>().sprite = item.sprite;
        }
    }

    private int num = 0;

    public int Num
    {
        get { return num; }
        set
        {
            num = value;
            tmp.text = value.ToString();
        }
    }
    public TextMeshProUGUI tmp;

    public void SetDefault()
    {
        Item = defaultItem;
        tmp.text = "0";
    }
}
