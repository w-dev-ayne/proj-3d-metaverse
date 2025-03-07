using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Help : UI_Popup
{
    enum Buttons
    {
        CloseButton
    }

    public override bool Init()
    {
        BindButton(typeof(Buttons));
        
        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton);
        
        GameObject.FindObjectOfType<vThirdPersonCamera>().lockCamera = true;
        
        if (base.Init() == false)
            return false;
        return true;
    }
    
    private void OnClickCloseButton()
    {
        GameObject.FindObjectOfType<vThirdPersonCamera>().lockCamera = false;
        ClosePopupUI();
    }
}
