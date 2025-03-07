using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_InfoPopup : UI_Popup
{
    enum Objects
    {
        SubjectObject,
        ContentObject
    }

    enum Buttons
    {
        CollectGenomeButton
    }

    public override bool Init()
    {
        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));
        
        GetButton((int)Buttons.CollectGenomeButton).gameObject.BindEvent(OnClickCollectGenomeButton, "none");
        SetContentMessageNPC();
        
        GameObject.FindObjectOfType<vThirdPersonCamera>().lockCamera = true;
        
        if (base.Init() == false)
            return false;
        return true;
    }

    private void SetContentMessageNPC()
    {
        SetContentMessage(GameObject.FindObjectOfType<NPCController>().GetCurrentMessage());
        
        float totalTextHeight = 0f;

        TextMeshProUGUI textElement = GetObject((int)Objects.ContentObject).GetComponent<TextMeshProUGUI>();
        RectTransform content = GetObject((int)Objects.ContentObject).transform.parent.parent.GetComponent<RectTransform>();
        
        totalTextHeight += textElement.preferredHeight;

        // Content의 크기를 조정하여 모든 텍스트를 수용할 수 있도록 합니다.
        content.sizeDelta = new Vector2(content.sizeDelta.x, totalTextHeight);
    }

    private void SetContentMessage(string message)
    {
        GetObject((int)Objects.ContentObject).GetComponent<TextMeshProUGUI>().text = message;
    }

    private void OnClickCollectGenomeButton()
    {
        GameObject.FindObjectOfType<vThirdPersonCamera>().lockCamera = false;
        Managers.Quest.CompleteCurrentQuest();

        NPCController npc = GameObject.FindObjectOfType<NPCController>();
        
        npc.IncreaseIndex();

        if (npc.disappearOnComplete)
        {
            npc.Disappear();
        }
        
        ClosePopupUI();
    }
}
