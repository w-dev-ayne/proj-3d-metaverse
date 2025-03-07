using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class DoTweenAppear : MonoBehaviour
{
    [SerializeField] private bool playOnEnable = true;
    [SerializeField] private float scale = 1.2f;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float scaleFactor = 1.0f;

    public UnityEvent onAppeared;

    public void DoPlay()
    {
        Vector3 originScale = Vector3.one * scaleFactor;
        
        this.transform.localScale = Vector3.zero;
        this.transform.DOScale(originScale * scale, duration/2).onComplete += () =>
        {
            this.transform.DOScale(originScale, duration / 2).onComplete += () =>
            {
                if(onAppeared != null)
                    onAppeared.Invoke();
            };
        };
    }

    public void Disappear(UnityAction onDisappear)
    {
        Vector3 originScale = Vector3.one * scaleFactor;
        
        this.transform.DOScale(originScale * scale, duration/2).onComplete += () =>
        {
            this.transform.DOScale(Vector3.zero, duration / 2).onComplete += () =>
            {
                if(onDisappear != null)
                    onDisappear.Invoke();
            };
        };
    }

    private void OnEnable()
    {
        if(playOnEnable)
            DoPlay();
    }
}
