using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_DNA: UI_Popup
{
    enum Objects
    {
        tmpObject,
        ItemObject
    }
    enum Buttons
    {
        OKButton
    }
    public override bool Init()
    {
        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));
        GetButton((int)Buttons.OKButton).gameObject.BindEvent(OnClickOKButton);
        
        SetText();
        
        return base.Init();
    }

    private void OnClickOKButton()
    {
        Item item = GetObject((int)Objects.ItemObject).GetComponent<ItemBehaviour>().item;
        Managers.Item.CollectItem(item);
        Managers.UI.FindPopup<UI_Main>().OnCollectItem(item);
        ClosePopupUI();
    }

    private void SetText()
    {
        GetObject((int)Objects.tmpObject).GetComponent<TextMeshProUGUI>().text = InstructionManager.CurrentInstruction;
    }
}
