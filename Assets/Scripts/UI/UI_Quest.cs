using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Quest : UI_Popup
{
    [SerializeField] private Color uncompletedColor;
    [SerializeField] private Color ongoingColor;
    [SerializeField] private Color completedColor;
    
    [SerializeField] private GameObject questPrefab;

    [SerializeField] private Sprite cryptoSprite;
    [SerializeField] private Sprite dnaSprite;
    
    private Transform dailyParent;
    private Transform subParent;
    
    enum Objects
    {
        DailyContentObject,
        SubContentObject,
        QuestInfoObject,
        TitleObject,
        InfoObject,
    }
    
    enum Buttons
    {
        CloseButton,
        QuestInfoCloseButton
    }

    public override bool Init()
    {
        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));
        
        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton);
        GetButton((int)Buttons.QuestInfoCloseButton).gameObject.BindEvent(OnClickQuestInfoCloseButton);
        
        GameObject.FindObjectOfType<vThirdPersonCamera>().lockCamera = true;
        
        dailyParent = GetObject((int)Objects.DailyContentObject).transform;
        subParent = GetObject((int)Objects.SubContentObject).transform;
        GetObject((int)Objects.QuestInfoObject).SetActive(false);
        
        Initialize();

        if (base.Init() == false)
            return false;
        return true;
    }

    private void Initialize()
    {
        List<Quest> allQuest = Managers.Quest.currentQuestSet.quests;
        allQuest.AddRange(Managers.Quest.subQuests);

        foreach (Quest quest in allQuest)
        {
            Image questButton = Instantiate(questPrefab).GetComponent<Image>();
            questButton.sprite = quest.icon;
            
            if (quest.type == Define.QuestType.Daily)
            {
                questButton.transform.SetParent(dailyParent);
            }
            else if (quest.type == Define.QuestType.Sub)
            {
                questButton.transform.SetParent(subParent);
            }

            switch (quest.state)
            {
                case Define.QuestState.UnCompleted:
                    questButton.color = uncompletedColor;
                    break;
                case Define.QuestState.OnGoing:
                    questButton.color = ongoingColor;
                    break;
                case Define.QuestState.Completed:
                    questButton.color = completedColor;
                    break;
            }

            questButton.transform.localScale = Vector3.one;
            questButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                ShowQuestInfoUI(quest);
            });
        }

        int count = 0;
        foreach (Quiz quiz in QuizManager.quizzes.Values)
        {
            if (count == 4)
                break;
            
            Image questButton = Instantiate(questPrefab).GetComponent<Image>();
            questButton.sprite = quiz.realType == Define.QuizType.Crypto ? cryptoSprite : dnaSprite;
            
            questButton.transform.SetParent(dailyParent);

            questButton.color = (quiz.isSolved) ? completedColor : ongoingColor;

            questButton.transform.localScale = Vector3.one;
            count++;
        }

        RectTransform dailyRect = dailyParent.GetComponent<RectTransform>();
        RectTransform subRect = subParent.GetComponent<RectTransform>();

        Debug.Log($"Spacing : {dailyParent.GetComponent<HorizontalLayoutGroup>().spacing}");
        dailyRect.sizeDelta = new Vector2((questPrefab.GetComponent<RectTransform>().sizeDelta.x * dailyParent.childCount) +
                                          (dailyParent.GetComponent<HorizontalLayoutGroup>().spacing *
                                           (dailyParent.childCount + 1)), questPrefab.GetComponent<RectTransform>().sizeDelta.y);
        subRect.sizeDelta = new Vector2((questPrefab.GetComponent<RectTransform>().sizeDelta.x * subParent.childCount) + (subParent.GetComponent<HorizontalLayoutGroup>().spacing *
            (subParent.childCount + 1)) , questPrefab.GetComponent<RectTransform>().sizeDelta.y);
    }
    
    private void OnClickCloseButton()
    {
        GameObject.FindObjectOfType<vThirdPersonCamera>().lockCamera = false;
        ClosePopupUI();
    }

    private void ShowQuestInfoUI(Quest quest)
    {
        GetObject((int)Objects.QuestInfoObject).SetActive(true);
        GetObject((int)Objects.TitleObject).GetComponent<TextMeshProUGUI>().text = quest.title;
        GetObject((int)Objects.InfoObject).GetComponent<TextMeshProUGUI>().text = quest.info;
    }

    private void OnClickQuestInfoCloseButton()
    {
        GetObject((int)Objects.QuestInfoObject).SetActive(false);
    }
}
