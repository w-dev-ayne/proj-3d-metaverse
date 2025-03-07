using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class LocalizationManager : MonoBehaviour
{
    public TMP_FontAsset krFont;
    public TMP_FontAsset engFont;
    
    public Define.Language language = Define.Language.Korean;
    private TextMeshProUGUI[] tmps;
    
    public void ChangeLanguage(Define.Language language)
    {
        this.language = language;
    }

    public void OnPopup(Transform popup)
    {
        LocalizationTmp[] tmps = popup.gameObject.GetComponentsInChildren<LocalizationTmp>(true);
        switch (language)
        {
            case Define.Language.Korean:
                foreach (LocalizationTmp tmp in tmps)
                {
                    tmp.tmp.font = krFont;
                }
                break;
            case Define.Language.English:
                foreach (LocalizationTmp tmp in tmps)
                {
                    tmp.tmp.font = engFont;
                }
                break;
            default:
                break;
        }
    }
}
