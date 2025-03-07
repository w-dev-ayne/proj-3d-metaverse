using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestSet Data", menuName = "Scriptable Object/QuestSet Data", order = int.MaxValue)]
public class QuestSet : ScriptableObject
{
    public int index = 0;
    public List<Quest> quests;
    public Quest currentQuest;

    
    // 현재 퀘스트 완료 후 다음 퀘스트 진행
    public void CompleteQuest()
    {
        currentQuest.Complete();
        index++;

        if (index == quests.Count)
        {
            CompleteQuestSet();
            return;
        }
        currentQuest = quests[index];
        
        StartNextQuest();
    }

    
    // 퀘스트에 이벤트 존 할당
    public void SetEventZones()
    {
        currentQuest = null;
        
        EventStateZone[] zones = FindObjectsByType<EventStateZone>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        
        foreach (Quest quest in this.quests)
        {
            foreach (EventStateZone zone in zones)
            {
                if(zone.id == -1)
                    continue;
                if (zone.id == quest.eventZoneID)
                {
                    quest.eventZone = zone;
                }
            }
        }
    }

    
    // 이 퀘스트 Set 시작
    public void StartQuestSet()
    {
        foreach (Quest quest in quests)
        {
            quest.state = Define.QuestState.UnCompleted;
        }
        index = 0;
        currentQuest = quests[index];
        StartNextQuest();
    }

    public void StartNextQuest()
    {
        currentQuest.Start();
    }

    
    // 이 퀘스트 Set 완료(종료)
    public void CompleteQuestSet()
    {
        currentQuest = null;
        InstructionManager.CurrentInstruction = "모든 퀘스트 완료! \n DNA를 획득하였습니다.";

        InstructionManager.onClickOKButton += () =>
        {
            Managers.Teleport.currentRoom.MoveCamera();
        };
        
        Managers.UI.ShowPopupUI<UI_Info>();
        Debug.Log("모든 퀘스트 완료");
        Managers.Teleport.currentRoom.portal.gameObject.SetActive(true);
    }
}
