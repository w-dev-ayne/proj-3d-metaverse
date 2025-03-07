using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LocalizationTmp : MonoBehaviour
{
    [Multiline(3)]
    [SerializeField] private string kor;
    [Multiline(3)]
    [SerializeField] private string eng;
    public TextMeshProUGUI tmp;

    private void OnEnable()
    {
        if (kor == String.Empty)
            return;
        
        switch (Managers.Localization.language)
        {
            case Define.Language.Korean:
                tmp.text = kor;
                break;
            case Define.Language.English:
                tmp.text = eng;
                break;
            default:
                break;
        }
    }
}
