using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Room : MonoBehaviour
{
    private bool init = false;
    
    public Define.Place place;
    public Transform portal;
    
    public Transform completeCamera;
    public Transform destination;
    public Animator[] completeAnimator;

    void Start()
    {
        if (place == Define.Place.Hall)
        {
            if (!init)
            {
                Managers.Quest.StartQuest();
                init = true;
            }
        }
    }

    private void OnDisable()
    {
        switch (place)
        {
            case Define.Place.Hall:
                Managers.Sound.PlayAudioClip(Define.Sound.Bgm ,SoundDefine.LabSceneBGM);
                break;
            case Define.Place.Lab1:
                Managers.Sound.PlayAudioClip(Define.Sound.Bgm ,SoundDefine.MainSceneBGM);
                break;
        }
    }
    
    public void MoveCamera()
    {
        GameObject.FindObjectOfType<vThirdPersonCamera>().lockCamera = true;
        Transform camera = GameObject.FindObjectOfType<vThirdPersonCamera>().transform;
        completeCamera.position = camera.position;
        completeCamera.rotation = camera.rotation;
        
        completeCamera.gameObject.SetActive(true);
        camera.gameObject.SetActive(false);

        float duration = 2.0f;
        
        completeCamera.DORotate(destination.eulerAngles, duration);
        completeCamera.DOMove(destination.position, duration).onComplete += () =>
        {
            completeAnimator[0].SetTrigger("Start");
            completeAnimator[1].SetTrigger("Start");
        };
    }
    
    public void ResetCamera()
    {
        Debug.Log("Reset Camera");
        
        Transform camera = GameObject.FindObjectOfType<vThirdPersonCamera>(true).transform;
        
        float duration = 2.0f;
        
        completeCamera.DORotate(camera.eulerAngles, duration);
        completeCamera.DOMove(camera.position, duration).onComplete += () =>
        {
            completeCamera.gameObject.SetActive(false);
            camera.gameObject.SetActive(true);
            GameObject.FindObjectOfType<vThirdPersonCamera>().lockCamera = false;

            InstructionManager.CurrentInstruction = "1 DNA를 획득하였습니다.";
            Managers.UI.ShowPopupUI<UI_DNA>();
        };
    }
}