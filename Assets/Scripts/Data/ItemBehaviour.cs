using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ItemBehaviour : MonoBehaviour
{
    public Item item;
    
    [SerializeField] private int coolTime = 5;
    [SerializeField] private bool isUIItem = false;
    private TextMeshProUGUI timerText;

    private bool isPicked = false;

    private bool isCool
    {
        set
        {
            if (value)
            {
                Disable();
            }
            else
            {
                Enable();
            }
        }
    }

    private Animation animation;

    void Awake()
    {
        if (isUIItem)
            return;
        animation = this.GetComponent<Animation>();
        timerText = this.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        this.transform.GetChild(0).GetComponent<Canvas>().worldCamera = Camera.main;
    }

    public void Pick()
    {
        if (isPicked)
            return;
     
        isPicked = true;
        OnPickCompleted();
        
        //animation.Play();
    }

    public void OnPickCompleted()
    {
        this.GetComponent<DoTweenAppear>().Disappear(() =>
        {
            Managers.UI.FindPopup<UI_Main>().OnCollectItem(this);
            Managers.Item.CollectItem(this.item);
            
            if(this.item.type == ItemDefine.ItemType.Sun || this.item.type == ItemDefine.ItemType.Water)
                Managers.Item.CheckIsComplete(this.item);
            
            this.transform.localScale = Vector3.one;
            this.GetComponent<Renderer>().enabled = false;
            
            Managers.Sound.PlayAudioClip(Define.Sound.Effect, SoundDefine.CollectItemEffect);
            
            isCool = true;
        });
    }

    private IEnumerator CoCoolTimer()
    {
        this.transform.Find("Timer").gameObject.SetActive(true);
        WaitForSeconds oneSecond = new WaitForSeconds(1.0f);

        int timer = coolTime;
        timerText.text = timer.ToString();
        
        while (timer != 0)
        {
            timer -= 1;
            timerText.text = timer.ToString();
            yield return oneSecond;
        }
        
        this.transform.Find("Timer").gameObject.SetActive(false);
        isCool = false;
    }

    
    // 아이템 디스폰
    private void Disable()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
        if(this.gameObject.activeInHierarchy)
            StartCoroutine(CoCoolTimer());
    }

    
    // 아이템 스폰
    private void Enable()
    {
        this.transform.localScale = Vector3.zero;
        this.GetComponent<Renderer>().enabled = true;
        
        isPicked = false;
        this.transform.GetChild(0).gameObject.SetActive(true);
        this.transform.GetChild(0).Find("RoundImage").GetComponent<Image>().fillAmount = 0;
        this.GetComponent<DoTweenAppear>().DoPlay();
        StopAllCoroutines();
    }
    
}
