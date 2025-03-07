using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup : UI_Base
{
    public delegate void loadHandler();
    public event loadHandler onImageLoaded;
    
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.UI.SetCanvas(gameObject, true);
        
        return true;
    }

    private void OnImageLoad()
    {
        this.transform.GetChild(0).gameObject.SetActive(true);
    }

    public virtual void ClosePopupUI()
    {
        Managers.UI.ClosePopupUI(this);
    }

    private void OnDestroy()
    {
        onImageLoaded -= OnImageLoad;
    }
}
