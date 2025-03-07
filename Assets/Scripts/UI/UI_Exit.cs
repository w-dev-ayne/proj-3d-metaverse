using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Exit : UI_Popup
{
    enum Buttons
    {
        YesButton,
        CloseButton
    }

    public override bool Init()
    {
        BindButton(typeof(Buttons));
        
        GetButton((int)Buttons.YesButton).gameObject.BindEvent(OnClickYesButton);
        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton);
        

        if (base.Init() == false)
            return false;
        return true;
    }

    private void OnClickYesButton()
    {
#if !UNITY_EDITOR && UNITY_IOS    
        RNManager.SendMessageToMobileApp("Unity to RN (OnClickExitButton) : Exit App (No Actual Exit)");
#endif
        Application.Quit();
        ClosePopupUI();
    }

    private void OnClickCloseButton()
    {
        ClosePopupUI();
    }
}
