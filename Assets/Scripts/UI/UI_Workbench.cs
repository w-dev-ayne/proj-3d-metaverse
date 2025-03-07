using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Workbench : UI_Popup
{
    public Workbench workBench;

    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite[] waterSprites;

    private int prevItemCount = 0;
    
    //private List<ItemDefine.ItemType> insertedItemType = new List<ItemDefine.ItemType>();
    
    enum Objects
    {
        WorkbenchSlotsObject,
        InventorySlotsObject,
        SunObject,
        WaterObject,
        WaterImageObject
    }

    enum Buttons
    {
        CloseButton,
        UndoButton,
        CombineButton
    }

    public override bool Init()
    {
        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));
        
        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton);
        GetButton((int)Buttons.UndoButton).gameObject.BindEvent(OnClickUndoButton);
        GetButton((int)Buttons.CombineButton).gameObject.BindEvent(OnClickCombineButton);

        this.workBench = Managers.Workbench.currentWorkbench;

        GetObject((int)Objects.SunObject).SetActive(this.workBench.type == Define.WorkbenchType.Sun);
        GetObject((int)Objects.WaterObject).SetActive(this.workBench.type != Define.WorkbenchType.Sun);
        
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
        RefreshInventorySlots();
    }

    
    // 인벤토리 슬롯 새로고침
    private void RefreshInventorySlots()
    {
        Transform slots = GetObject((int)Objects.InventorySlotsObject).transform;

        int count = 0;
        
        Debug.Log($"Item Num : {Managers.Item.items.Keys.Count}");
        
        foreach (Item item in Managers.Item.items.Keys)
        {
            ItemSlot slot = slots.GetChild(count).GetComponent<ItemSlot>();
            slot.Item = item;
            slot.Num = Managers.Item.items[item];

            slots.GetChild(count).GetComponent<Button>().onClick.RemoveAllListeners();
            slots.GetChild(count).GetComponent<Button>().onClick.AddListener(() => OnClickInventorySlotButton(slot.Item));
            
            count++;
        }
        
        // 삭제된 아이템 있으면 슬롯 초기화
        if(prevItemCount != Managers.Item.items.Keys.Count)
        {
            for (int i = Managers.Item.items.Keys.Count; i < prevItemCount; i++)
            {
                slots.GetChild(i).GetComponent<ItemSlot>().SetDefault();
                slots.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            }

            prevItemCount = Managers.Item.items.Keys.Count;
        }

        TextMeshProUGUI tmp = this.workBench.type == Define.WorkbenchType.Sun
            ? GetObject((int)Objects.SunObject).transform.GetChild(0).GetComponent<TextMeshProUGUI>()
            : GetObject((int)Objects.WaterImageObject).transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        string status = (this.workBench.type == Define.WorkbenchType.Sun) ? 
            $"{(((float)this.workBench.insertedItems.Count / (float)Workbench.MAX_ITEM) * 100).ToString("n1")}" :
            $"{(((float)this.workBench.insertedItems.Count / (float)Workbench.MAX_ITEM) * 100).ToString("n1")} %";

        if (this.workBench.type == Define.WorkbenchType.Water)
        {
            GetObject((int)Objects.WaterImageObject).GetComponent<Image>().sprite =
                waterSprites[(int)((workBench.insertedItems.Count - 1) / 2)];
        }
        
        tmp.text = status;
    }

    private void OnClickInventorySlotButton(Item item)
    {
        workBench.PushItem(item);
        RefreshSlots();
        RefreshInventorySlots();
        
        /*if (this.workBench.type == Define.WorkbenchType.Water)
        {
            this.workBench.dnaAnims[0].SetTrigger("Forward");
            this.workBench.dnaAnims[1].SetTrigger("Forward");
        }*/
    }

    private void OnClickUndoButton()
    {
        workBench.PopItem();
        RefreshSlots();
        RefreshInventorySlots();

        /*if (this.workBench.type == Define.WorkbenchType.Water)
        {
            this.workBench.dnaAnims[0].SetTrigger("Backward");
            this.workBench.dnaAnims[1].SetTrigger("Backward");
        }*/
    }

    private void OnClickCombineButton()
    {
        workBench.CombineItems();
        RefreshSlots();
        GetObject((int)Objects.SunObject).SetActive(this.workBench.type == Define.WorkbenchType.Sun);
        GetObject((int)Objects.WaterObject).SetActive(this.workBench.type != Define.WorkbenchType.Sun);
        
        if(this.workBench.type == Define.WorkbenchType.Done)
            ClosePopupUI();
    }

    
    // 인큐베이터 슬롯 새로고침
    private void RefreshSlots()
    {
        Transform slots = GetObject((int)Objects.WorkbenchSlotsObject).transform;

        int count = 0;
        foreach (Item item in workBench.insertedItems)
        {
            slots.GetChild(count).GetChild(0).GetComponent<Image>().sprite = item.sprite;
            count++;
        }

        for (int i = count; count < Workbench.MAX_ITEM; count++)
        {
            slots.GetChild(count).GetChild(0).GetComponent<Image>().sprite = defaultSprite;
        }
    }

    private void OnClickCloseButton()
    {
        ClosePopupUI();
    }
}
