using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Quest Data", menuName = "Scriptable Object/Quest Data", order = int.MaxValue)]
public class Quest : ScriptableObject
{
    public Define.QuestType type;
    public Define.QuestState state;
    public string title = "Title";
    public string info = "퀘스트 설명";
    public Sprite icon;
    [Multiline(3)] public string engInfo;
    [Multiline(3)] public string korInfo;
    public int eventZoneID = 0;
    
    [HideInInspector]public EventStateZone eventZone;
    public ItemDefine.ItemType collectItemType = ItemDefine.ItemType.None;
    
    private bool isComplete = false;

    public void Start()
    {
        switch (Managers.Localization.language)
        {
            case Define.Language.Korean:
                info = korInfo;
                break;
            case Define.Language.English:
                info = engInfo;
                break;
            default:
                break;
        }
        
        switch (collectItemType)
        {
            case ItemDefine.ItemType.Sun:
                Managers.Item.StartCollectSun();
                break;
            case ItemDefine.ItemType.Water:
                Managers.Item.StartCollectWater();
                break;
            case ItemDefine.ItemType.Geno:
                Managers.CollectWord.GenerateWords();
                break;
            default:
                break;
        }
        Managers.UI.ShowPopupUI<UI_Instruction>();
        if(eventZone != null)
            eventZone.ChangeState(Define.QuestState.OnGoing);
        state = Define.QuestState.OnGoing;
    }

    public void Complete()
    {
        if(eventZone != null)
            eventZone.ChangeState(Define.QuestState.Completed);
        //Managers.CollectGenome.CollectGenome();
        Managers.Sound.PlayAudioClip(Define.Sound.Effect, SoundDefine.CompleteQuestEffect);
        Debug.Log("퀘스트 완료");
        state = Define.QuestState.Completed;
    }
}