using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public QuestSet currentQuestSet;

    public List<Quest> subQuests;
    public QuestSet sampleQuestSet;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        currentQuestSet = sampleQuestSet;
        
        //Invoke("StartQuestSet", 2.0f);
    }

    public void StartQuest()
    {
        Invoke("StartQuestSet", 2.0f);
    }

    
    // 현재 퀘스트 Set 초기 시작
    private void StartQuestSet()
    {
        currentQuestSet.SetEventZones();
        currentQuestSet.StartQuestSet();
    }

    
    // 현재 퀘스트 Set의 현재 퀘스트 완료(종료)
    public void CompleteCurrentQuest()
    {
        currentQuestSet.CompleteQuest();
    }
}
