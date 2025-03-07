using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_LeaderBoard : UI_Popup
{
    public GameObject rankPrefab;

    enum Objects
    {
        ContentObject
    }
    
    enum Buttons
    {
        CloseButton
    }

    public override bool Init()
    {
        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton);
        GameObject.FindObjectOfType<vThirdPersonCamera>().lockCamera = true;
        
        InitializeRank();
        
        if (base.Init() == false)
            return false;
        
        return true;
    }

    private void InitializeRank()
    {
        LeaderBoard data = Managers.API.leaderBoardData;
        
        Transform contentObject = GetObject((int)Objects.ContentObject).transform;
        
        foreach (UserScore userScore in data.userScores)
        {
            Transform rank = Instantiate(rankPrefab, contentObject).transform;
            rank.Find("Rank").GetComponent<TextMeshProUGUI>().text = userScore.rank.ToString();
            rank.Find("Name").GetComponent<TextMeshProUGUI>().text = userScore.name;
            rank.Find("Point").GetComponent<TextMeshProUGUI>().text = userScore.point.ToString();
        }

        int spacing = (int)contentObject.GetComponent<VerticalLayoutGroup>().spacing;
        int prefabHeight = (int)rankPrefab.GetComponent<RectTransform>().sizeDelta.y;
        int height = (contentObject.childCount - 1) * spacing + (contentObject.childCount * prefabHeight);
        
        RectTransform contentRect = contentObject.GetComponent<RectTransform>();

        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, height);
    }

    private void OnClickCloseButton()
    {
        GameObject.FindObjectOfType<vThirdPersonCamera>().lockCamera = false;
        ClosePopupUI();
    }
    
}
