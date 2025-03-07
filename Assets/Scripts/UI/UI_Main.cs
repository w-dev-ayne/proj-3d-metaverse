using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Main : UI_Popup
{
    enum Objects
    {
        ScoreObject,
        LogoObject,
        CollectLogoObject,
        GenoObject,
        CollectItemObject
    }

    enum Buttons
    {
        LeaderBoardButton,
        QuestButton,
        VolumeButton,
        HelpButton,
        ExitButton,
        InventoryButton
    }

    public override bool Init()
    {
        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));
        
        GetButton((int)Buttons.LeaderBoardButton).gameObject.BindEvent(OnClickLeaderBoardButton);
        GetButton((int)Buttons.QuestButton).gameObject.BindEvent(OnClickQuestButton);
        GetButton((int)Buttons.VolumeButton).gameObject.BindEvent(OnClickVolumeButton);
        GetButton((int)Buttons.HelpButton).gameObject.BindEvent(OnClickHelpButton);
        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton);
        GetButton((int)Buttons.InventoryButton).gameObject.BindEvent(OnClickInventoryButton);

        GetObject((int)Objects.CollectLogoObject).SetActive(false);
        GetObject((int)Objects.CollectItemObject).SetActive(false);
        
        if (base.Init() == false)
        {
            return false;
        }

        return true;
    }

    public void OnCollectGenome(int score)
    {
        Transform logo = GetObject((int)Objects.CollectLogoObject).transform;

        logo.position = EventSystem.current.currentSelectedGameObject? EventSystem.current.currentSelectedGameObject.transform.position : Vector2.zero;
        logo.gameObject.SetActive(true);
        logo.DOMove(GetObject((int)Objects.LogoObject).transform.position, 1.0f).onComplete += () =>
        {
            this.IncreaseScore(score);
            logo.gameObject.SetActive(false);
        };
        
        //this.IncreaseScore(score);
    }

    private void IncreaseScore(int score)
    {
        GetObject((int)Objects.ScoreObject).GetComponent<TextMeshProUGUI>().text = score.ToString();
    }

    private void OnClickLeaderBoardButton()
    {
        Managers.UI.ShowPopupUI<UI_LeaderBoard>();
    }

    private void OnClickQuestButton()
    {
        Managers.UI.ShowPopupUI<UI_Quest>();
    }

    private void OnClickVolumeButton()
    {
        Managers.UI.ShowPopupUI<UI_Sound>();
    }

    private void OnClickHelpButton()
    {
        Managers.UI.ShowPopupUI<UI_Help>();
    }

    private void OnClickExitButton()
    {
        Managers.UI.ShowPopupUI<UI_Exit>();
    }

    private void OnClickInventoryButton()
    {
        Managers.UI.ShowPopupUI<UI_Inventory>();
    }

    public void OnCollectItem(ItemBehaviour itemBehaviour)
    {
        float duration = 0.5f;
        Transform logo = GetObject((int)Objects.CollectItemObject).transform;
        
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(itemBehaviour.transform.position);

        // 스크린 좌표를 픽셀 좌표로 변환
        Vector2 pixelPosition = new Vector2(screenPoint.x, screenPoint.y);

        logo.localScale = Vector3.one;
        logo.position = pixelPosition;
        logo.GetComponent<Image>().sprite = itemBehaviour.item.sprite;
        logo.gameObject.SetActive(true);
        logo.DOMove(GetButton((int)Buttons.InventoryButton).transform.position, duration).onComplete += () =>
        {
            logo.gameObject.SetActive(false);
        };
        logo.DOScale(Vector3.zero, duration);
    }

    public void OnCollectItem(Item item)
    {
        float duration = 0.75f;
        Transform logo = GetObject((int)Objects.CollectItemObject).transform;

        Vector2 pixelPosition = Input.mousePosition;

        logo.localScale = Vector3.one;
        logo.position = pixelPosition;
        logo.GetComponent<Image>().sprite = item.sprite;
        logo.gameObject.SetActive(true);
        logo.DOMove(GetButton((int)Buttons.InventoryButton).transform.position, duration).onComplete += () =>
        {
            logo.gameObject.SetActive(false);
        };
        logo.DOScale(Vector3.zero, duration);
    }
    
    public void OnCollectGenoWord(string word)
    {
        Debug.Log("OnCollectMethod");
        GetObject((int)Objects.GenoObject).transform.Find(word).GetChild(0).gameObject.SetActive(true);
        
        //GetObject((int)Objects.GenoObject).transform.Find(word).GetComponent<DOTweenAnimation>().DOPlay();
    }
}
