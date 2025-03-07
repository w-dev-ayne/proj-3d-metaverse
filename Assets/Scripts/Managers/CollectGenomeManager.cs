using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectGenomeManager : MonoBehaviour
{
    private int collectedGeNome = 0;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    
    public void CollectGenome()
    {
        collectedGeNome += 10;
        Managers.UI.FindPopup<UI_Main>().OnCollectGenome(this.collectedGeNome);
    }
}