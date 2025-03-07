using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Fade : UI_Popup
{
    public override bool Init()
    {
        Managers.Sound.PlayAudioClip(Define.Sound.Effect, SoundDefine.TeleportEffect);
        if (base.Init() == false)
            return false;
        return true;
    }

    public void OnFade()
    {
        Managers.Teleport.Teleport();
    }

    public void OnFadeOut()
    {
        ClosePopupUI();
    }
}
