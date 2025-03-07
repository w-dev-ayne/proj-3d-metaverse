using System;
using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class CharacterTrigger : MonoBehaviour
{
    public Define.TriggerEventType eventType;
    
    [DrawIf("eventType", Define.TriggerEventType.Quiz)]
    public int quizNum = 0;
    [DrawIf("eventType", Define.TriggerEventType.Quiz)]
    public bool isIncubator = false;
    
    [DrawIf("eventType", Define.TriggerEventType.Teleport)]
    public Define.Place destination;
    public bool init = false;
    
    [DrawIf("eventType", Define.TriggerEventType.Collect)]
    public Define.CollectWord word;

    [DrawIf("isIncubator", true)]
    public Workbench workbench;
    
    public UnityAction onCharacterIn;
    public UnityAction onCharacterOut;

    public UnityEvent triggerEvent;
    public UnityEvent triggerOutEvent;
    
    
    private void Awake()
    {
        onCharacterIn += OnCharacterIn;
        onCharacterOut += OnCharacterOut;
        
        SetEvent();
    }

    public void SetEvent()
    {
        triggerEvent.RemoveAllListeners();
        triggerOutEvent.RemoveAllListeners();
        
        // TODO: interface로 분리하기
        switch (eventType)
        {
            case Define.TriggerEventType.Info:
                triggerEvent.AddListener(() =>
                {
                    Managers.UI.ShowPopupUI<UI_InfoPopup>();
                });
                triggerOutEvent.AddListener(() =>
                {
                    if (Managers.UI.FindPopup<UI_InfoPopup>() == null)
                    {
                        return;
                    }
                    GameObject.FindObjectOfType<vThirdPersonCamera>().lockCamera = false;
                    Managers.UI.ClosePopupUI();
                });
                break;
            case Define.TriggerEventType.Quiz:
                triggerEvent.AddListener(() =>
                {
                    if (isIncubator)
                    {
                        InstructionManager.CurrentInstruction = "부화기가 잠겨 있습니다.\n 잠금을 해제하려면 문제를 풀어야 합니다.";
                        InstructionManager.onClickOKButton = () =>
                        {
                            QuizManager.ShowQuiz(quizNum);
                        };
                        Managers.UI.ShowPopupUI<UI_Info>();
                    }
                    else
                    {
                        QuizManager.ShowQuiz(quizNum);    
                    }
                });
                triggerOutEvent.AddListener(() =>
                {
                    if (Managers.UI.FindPopup<UI_Quiz>() == null)
                    {
                        return;
                    }
                    GameObject.FindObjectOfType<vThirdPersonCamera>().lockCamera = false;
                    Managers.UI.ClosePopupUI();
                });
                break;
            case Define.TriggerEventType.Teleport:
                triggerEvent.AddListener(() =>
                {
                    if (!init)
                        init = true;
                    else
                    {
                        //Managers.UI.ShowPopupUI<UI_Teleport>();
                        Managers.Teleport.StartTeleport(this.destination);   
                    }
                });
                break;
            case Define.TriggerEventType.Collect:
                triggerEvent.AddListener(() =>
                {
                    this.gameObject.SetActive(false);
                    Managers.CollectWord.CollectWord(this.word);
                });
                break;
            case Define.TriggerEventType.Workbench:
                triggerEvent.AddListener(() =>
                {
                    this.workbench.StartWorkbench();
                });
                break;
            case Define.TriggerEventType.FailZone:
                triggerEvent.AddListener(() =>
                {
                    Debug.Log("FailZone");

                    Managers.Teleport.StartTeleport(Managers.Teleport.currentPlace);
                });
                break;
            case Define.TriggerEventType.Item:
                triggerEvent.AddListener(() =>
                {
                    GetComponent<ItemBehaviour>().Pick();
                });
                break;
            default:
                break;
        }
    }

    private void OnCharacterIn()
    {
        triggerEvent.Invoke();
        Debug.Log("On Character In");
    }

    private void OnCharacterOut()
    {
        triggerOutEvent.Invoke();
        Debug.Log("On Character Out");  
    }

    private void OnDestroy()
    {
        onCharacterIn -= OnCharacterIn;
        onCharacterOut -= OnCharacterOut;
    }
}
