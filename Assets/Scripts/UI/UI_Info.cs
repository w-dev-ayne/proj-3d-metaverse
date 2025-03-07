using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Info : UI_Popup
{
    enum Objects
    {
        tmpObject
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
        if (InstructionManager.onClickOKButton != null)
        {
            InstructionManager.onClickOKButton.Invoke();
            InstructionManager.onClickOKButton = null;
        }
        ClosePopupUI();
    }

    private void SetText()
    {
        GetObject((int)Objects.tmpObject).GetComponent<TextMeshProUGUI>().text = InstructionManager.CurrentInstruction;
    }
}
