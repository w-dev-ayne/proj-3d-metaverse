using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Quiz : UI_Popup
{
    private Quiz quiz;
    
    enum Objects
    {
        SubjectObject,
        ContentObject,
        QuizObject,
        DescriptionObject
    }

    enum Buttons
    {
        OButton,
        XButton,
        QuizButton,
        ReviewButton,
        CloseButton
    }

    public override bool Init()
    {
        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));
        
        GetButton((int)Buttons.OButton).gameObject.BindEvent(() => OnClickAnswerButton(Define.QuizAnswer.O), "none");
        GetButton((int)Buttons.XButton).gameObject.BindEvent(() => OnClickAnswerButton(Define.QuizAnswer.X), "none");
        GetButton((int)Buttons.QuizButton).gameObject.BindEvent(OnClickQuizButton);
        GetButton((int)Buttons.ReviewButton).gameObject.BindEvent(OnClickReviewButton);
        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton);
        
        InitializeText(QuizManager.currentQuiz);
        
        GetObject((int)Objects.QuizObject).SetActive(false);
        GameObject.FindObjectOfType<vThirdPersonCamera>().lockCamera = true;
        
        Managers.Sound.PlayAudioClip(Define.Sound.Effect, SoundDefine.QuizPopupEffect);

        if (base.Init() == false)
            return false;
        return true;
    }

    public void InitializeText(Quiz quiz)
    {
        this.quiz = quiz;

        TextMeshProUGUI contentTMP = GetObject((int)Objects.ContentObject).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI descriptionTMP = GetObject((int)Objects.DescriptionObject).GetComponent<TextMeshProUGUI>();
        
        contentTMP.text = this.quiz.quiz;
        GetObject((int)Objects.SubjectObject).GetComponent<TextMeshProUGUI>().text = this.quiz.title;
        descriptionTMP.text = this.quiz.info;

        RectTransform contentContent = contentTMP.transform.parent.parent.GetComponent<RectTransform>();
        RectTransform descriptionContent = descriptionTMP.transform.parent.parent.GetComponent<RectTransform>();;

        contentContent.sizeDelta = new Vector2(contentContent.sizeDelta.x, contentTMP.preferredHeight);
        descriptionContent.sizeDelta = new Vector2(contentContent.sizeDelta.x, descriptionTMP.preferredHeight);
    }

    private void OnClickAnswerButton(Define.QuizAnswer answer)
    {
        EventStateZone zone = FindObjectOfType<CharacterTriggerController>().currentTriggerZone.GetComponent<EventStateZone>();
        // 정답 처리
        if (this.quiz.IsCorrect(answer))
        {
            Managers.Sound.PlayAudioClip(Define.Sound.Effect, SoundDefine.AnswerEffect);
            if (zone.isQuiz)
            {
                this.quiz.Solved();
                zone.ChangeState(Define.QuestState.Completed);
                InstructionManager.CurrentInstruction = "축하합니다! 정답입니다.\n10 point를 획득했습니다";
                Managers.CollectGenome.CollectGenome();
                Managers.UI.ShowPopupUI<UI_Info>();
            }
            else
            {
                if (Managers.Quest.currentQuestSet.currentQuest.eventZone.TryGetComponent<CharacterTrigger>(out CharacterTrigger trigger))
                {
                    if (trigger.isIncubator)
                    {
                        trigger.eventType = Define.TriggerEventType.Workbench;
                        trigger.SetEvent();

                        GameObject.FindObjectOfType<NPCController>(false).Appear();
                    }
                }
                this.quiz.Solved();
                Managers.Quest.CompleteCurrentQuest();
            }
        }
        // 오답 처리
        else
        {   
            Managers.Sound.PlayAudioClip(Define.Sound.Effect, SoundDefine.WrongEffect);
            if (Managers.Quest.currentQuestSet.currentQuest != null && Managers.Quest.currentQuestSet.currentQuest.eventZone.TryGetComponent<CharacterTrigger>(out CharacterTrigger trigger))
            {
                if (trigger.isIncubator)
                {
                    InstructionManager.CurrentInstruction = "다시 시도해주세요.";
                    Managers.UI.ShowPopupUI<UI_Info>();
                    GetObject((int)Objects.QuizObject).SetActive(false);
                    return;
                }
            }
            
            GameObject.FindObjectOfType<vThirdPersonCamera>().lockCamera = false;
            ClosePopupUI();
            InstructionManager.CurrentInstruction = "다시 시도해주세요.";
            Managers.UI.ShowPopupUI<UI_Info>();
            this.quiz.isCoolTime = true;
            zone.StartCoolTimer();
        }
    }

    private void OnClickQuizButton()
    {
        GetObject((int)Objects.QuizObject).SetActive(true);
    }

    private void OnClickReviewButton()
    {
        GetObject((int)Objects.QuizObject).SetActive(false);
    }

    private void OnClickCloseButton()
    {
        GameObject.FindObjectOfType<vThirdPersonCamera>().lockCamera = false;
        ClosePopupUI();
    }

    void OnDestroy()
    {
        GameObject.FindObjectOfType<vThirdPersonCamera>().lockCamera = false;
        GetObject((int)Objects.QuizObject).SetActive(false);
    }
}
