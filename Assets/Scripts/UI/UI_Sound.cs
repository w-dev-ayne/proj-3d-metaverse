using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Sound : UI_Popup
{
    private Slider masterSlider;
    private Slider bgmSlider;
    private Slider effectSlider;

    private TextMeshProUGUI masterTMP;
    private TextMeshProUGUI bgmTMP;
    private TextMeshProUGUI effectTMP;
    
    enum Objects
    {
        MasterObject,
        BGMObject,
        EffectObject,
        MasterTMPObject,
        BGMTMPObject,
        EffectTMPObject,
    }

    enum Buttons
    {
        CloseButton
    }

    public override bool Init()
    {
        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));

        masterSlider = GetObject((int)Objects.MasterObject).GetComponent<Slider>();
        bgmSlider = GetObject((int)Objects.BGMObject).GetComponent<Slider>();
        effectSlider = GetObject((int)Objects.EffectObject).GetComponent<Slider>();
        
        masterTMP = GetObject((int)Objects.MasterTMPObject).GetComponent<TextMeshProUGUI>();
        bgmTMP = GetObject((int)Objects.BGMTMPObject).GetComponent<TextMeshProUGUI>();
        effectTMP = GetObject((int)Objects.EffectTMPObject).GetComponent<TextMeshProUGUI>();

        masterSlider.onValueChanged.AddListener(OnMasterVolumeValueChanged);
        bgmSlider.onValueChanged.AddListener(OnBGMVolumeValueChanged);
        effectSlider.onValueChanged.AddListener(OnEffectVolumeValueChanged);
        
        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton);

        masterSlider.value = Managers.Sound.MasterVolume;
        bgmSlider.value = Managers.Sound.BgmVolume;
        effectSlider.value = Managers.Sound.EffectVolume;
        
        GameObject.FindObjectOfType<vThirdPersonCamera>().lockCamera = true;
        
        if (base.Init() == false)
        {
            return false;
        }

        return true;
    }

    private void OnMasterVolumeValueChanged(float value)
    {
        Managers.Sound.MasterVolume = value;
        masterTMP.text = $"{((int)(value * 100)).ToString()} %";
    }
    
    private void OnBGMVolumeValueChanged(float value)
    {
        Managers.Sound.BgmVolume = value;
        bgmTMP.text = $"{((int)(value * 100)).ToString()} %";
    }
    
    private void OnEffectVolumeValueChanged(float value)
    {
        Managers.Sound.EffectVolume = value;
        effectTMP.text = $"{((int)(value * 100)).ToString()} %";
    }

    private void OnClickCloseButton()
    {
        GameObject.FindObjectOfType<vThirdPersonCamera>().lockCamera = false;
        ClosePopupUI();
    }
}
