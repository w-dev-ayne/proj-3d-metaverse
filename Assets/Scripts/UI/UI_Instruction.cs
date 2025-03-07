using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Instruction : UI_Popup
{
    enum Objects
    {
        TextObject
    }
    
    public override bool Init()
    {
        BindObject(typeof(Objects));
        
        SetText();
        
        Invoke("ClosePopupUI", 3.0f);
        
        if (base.Init() == false)
        {
            return false;
        }
        return true;
    }

    private void SetText()
    {
        if (Managers.Quest.currentQuestSet.currentQuest == null)
        {
            GetObject((int)Objects.TextObject).GetComponent<TextMeshProUGUI>().text = "모든 퀘스트를 완료하였습니다.";
        }
        else
        {
            GetObject((int)Objects.TextObject).GetComponent<TextMeshProUGUI>().text =
                Managers.Quest.currentQuestSet.currentQuest.info;
        }
    }

    public void SetText(string message)
    {
        GetObject((int)Objects.TextObject).GetComponent<TextMeshProUGUI>().text =
            message;
    }
}
