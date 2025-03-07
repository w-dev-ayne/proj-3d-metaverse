using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Teleport : UI_Popup
{
    enum Objects
    {
        ContentObject
    }

    enum Buttons
    {
        ForwardButton,
        BackwardButton,
        CloseButton,
        EnterButton
    }

    public override bool Init()
    {
        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));
        
        GetButton((int)Buttons.ForwardButton).gameObject.BindEvent(OnClickForwardButton);
        GetButton((int)Buttons.BackwardButton).gameObject.BindEvent(OnClickBackwardButton);
        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton);
        GetButton((int)Buttons.EnterButton).gameObject.BindEvent(OnClickEnterButton);

        if (base.Init() == false)
        {
            return false;
        }

        return true;
    }

    private void OnClickForwardButton()
    {
        GetObject((int)(Objects.ContentObject)).GetComponent<DoTweenScroll>().DoScroll(true);
    }

    private void OnClickBackwardButton()
    {
        GetObject((int)(Objects.ContentObject)).GetComponent<DoTweenScroll>().DoScroll(false);
    }

    private void OnClickCloseButton()
    {
        ClosePopupUI();
    }

    private void OnClickEnterButton()
    {
        int currentPage = GetObject((int)(Objects.ContentObject)).GetComponent<DoTweenScroll>().GetCurrentPage();
        
        Managers.Teleport.StartTeleport((Define.Place)(currentPage));
        
        ClosePopupUI();
    }
}
