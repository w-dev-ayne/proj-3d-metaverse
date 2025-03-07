using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workbench : MonoBehaviour
{
    public Stack<Item> insertedItems = new Stack<Item>();
    public const int MAX_ITEM = 8;
    public Define.WorkbenchType type;
    public Animator[] dnaAnims;
    
    public void StartWorkbench()
    {
        insertedItems = new Stack<Item>();
        Managers.Workbench.currentWorkbench = this;
        Managers.UI.ShowPopupUI<UI_Workbench>();
        Invoke("CompleteCurrentQuest", 0.5f);
        
    }

    public void CompleteCurrentQuest()
    {
        Managers.Quest.CompleteCurrentQuest();
    }

    public void PushItem(Item item)
    {
        if (insertedItems.Count == MAX_ITEM)
            return;
        
        insertedItems.Push(item);
        Managers.Item.DeleteItem(item);
    }

    public void PopItem()
    {
        if (insertedItems.Count == 0)
            return;
        
        Managers.Item.CollectItem(insertedItems.Pop());
    }

    public void CombineItems()
    {
        if (insertedItems.Count != MAX_ITEM)
            return;

        switch (type)
        {
            case Define.WorkbenchType.Sun:
                this.type = Define.WorkbenchType.Water;
                insertedItems = new Stack<Item>();
                Managers.Quest.CompleteCurrentQuest();
                break;
            case Define.WorkbenchType.Water:
                /*dnaAnims[0].SetTrigger("Start");
                dnaAnims[1].SetTrigger("Start");*/
                //Managers.UI.FindPopup<UI_Workbench>().ClosePopupUI();
                this.type = Define.WorkbenchType.Done;
                //Managers.Quest.currentQuestSet.CompleteQuestSet();
                Invoke("FinishQuest", 0.2f);
                //Managers.Quest.CompleteCurrentQuest();
                break;
            default:
                break;
        }
    }

    private void FinishQuest()
    {
        Managers.Quest.CompleteCurrentQuest();
    }
}
