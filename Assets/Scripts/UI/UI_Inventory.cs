using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_Inventory : UI_Popup
{
    private Dictionary<ItemDefine.ItemType, ItemSlot> insertedItem = new Dictionary<ItemDefine.ItemType, ItemSlot>();
    //private List<ItemDefine.ItemType> insertedItemType = new List<ItemDefine.ItemType>();
    
    enum Objects
    {
        SlotsObject
    }

    enum Buttons
    {
        CloseButton
    }

    public override bool Init()
    {
        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));
        
        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton);
        
        Initialize();
        
        if (base.Init() == false)
        {
            return false;
        }

        return true;
    }

    // items에 따라 인벤토리 초기화
    private void Initialize()
    {
        Transform slots = GetObject((int)Objects.SlotsObject).transform;

        int count = 0;
        foreach (Item item in Managers.Item.items.Keys)
        {
            ItemSlot slot = slots.GetChild(count).GetComponent<ItemSlot>();
            slot.Item = item;
            slot.Num = Managers.Item.items[item];
            //slots.GetChild(count).GetComponent<Button>().onClick?.AddListener(() => OnClickInventorySlotButton(slot.Item));
            count++;
        }
    }

    private void OnClickCloseButton()
    {
        ClosePopupUI();
    }
}
