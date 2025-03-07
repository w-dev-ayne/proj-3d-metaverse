using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DoTweenScroll : MonoBehaviour
{
    [HideInInspector] public bool isMoving = false;
    
    [SerializeField] private int pages = 6;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float moveDistance;

    private int currentPage = 0;
    
    public void DoScroll(bool isForward)
    {
        if (isForward)
        {
            if (currentPage == pages - 1)
            {
                return;
            }
        }
        else
        {
            if (currentPage == 0)
            {
                return;
            }
        }
        DoMove(isForward);
    }

    private void DoMove(bool isForward)
    {
        if (isForward)
        {
            currentPage++;
        }
        else
        {
            currentPage--;
        }
        isMoving = true;
        float movingDistance = isForward ? -moveDistance : moveDistance;
        this.transform.DOLocalMoveX(this.transform.localPosition.x + movingDistance, duration).onComplete += () =>
        {
            isMoving = false;
        };
    }

    public int GetCurrentPage()
    {
        return this.currentPage;
    }
    
    // Start is called before the first frame update
    void OnEnable()
    {
        currentPage = 0;
    }
}
