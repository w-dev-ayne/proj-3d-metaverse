using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventStateZone : MonoBehaviour
{
    public Define.QuestState state = Define.QuestState.UnCompleted;

    public int id = 0;

    public bool isQuiz = false;
    
    private GameObject noticeObj;
    
    [DrawIf("isQuiz", true)]
    public GameObject waitObject;
    private const int COOL_TIME = 60;
    public int currentCoolTime = COOL_TIME;

    private Quiz quiz;

    private void Awake()
    {
        noticeObj = this.transform.GetChild(0).gameObject;
        if (isQuiz)
        {
            ChangeState(Define.QuestState.OnGoing);
        }
        else
        {
            ChangeState(Define.QuestState.UnCompleted);   
        }
    }

    private void OnClickQuestButton()
    {
        Debug.Log("Quest Complete");
    }

    public void ChangeState(Define.QuestState state)
    {
        this.state = state;

        switch (this.state)
        {
            case Define.QuestState.UnCompleted:
                this.GetComponent<Collider>().enabled = false;
                noticeObj.SetActive(false);
                break;
            case Define.QuestState.OnGoing:
                this.GetComponent<Collider>().enabled = true;
                noticeObj.SetActive(true);
                break;
            case Define.QuestState.Completed:
                this.GetComponent<Collider>().enabled = false;
                noticeObj.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void StartCoolTimer()
    {
        quiz = QuizManager.currentQuiz;
        noticeObj.SetActive(false);
        waitObject.SetActive(true);
        StartCoroutine(CoStartCoolTimer());
    }

    private IEnumerator CoStartCoolTimer()
    {
        TextMeshProUGUI tmp = waitObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        WaitForSeconds oneSecond = new WaitForSeconds(1);
        currentCoolTime = COOL_TIME;
        tmp.text = currentCoolTime.ToString();

        while (currentCoolTime != 0)
        {
            yield return oneSecond;
            currentCoolTime -= 1;
            quiz.coolTime = currentCoolTime;
            tmp.text = currentCoolTime.ToString();
        }
        
        CompleteTimer();
    }

    private void CompleteTimer()
    {
        this.quiz.isCoolTime = false;
        noticeObj.SetActive(true);
        waitObject.SetActive(false);
    }
}
