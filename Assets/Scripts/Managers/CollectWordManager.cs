using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectWordManager : MonoBehaviour
{
    private Dictionary<string, bool> collected = new Dictionary<string, bool>();
    public Quest quest;

    public GameObject[] words;
    private RandomZone[] itemParent;
    private Dictionary<int, List<int>> nums = new Dictionary<int, List<int>>();

    void Awake()
    {
        DontDestroyOnLoad(this);
        Init();
    }

    private void Init()
    {
        collected["G"] = false;
        collected["E"] = false;
        collected["N"] = false;
        collected["O"] = false;
    }

    public void GenerateWords()
    {
        itemParent = GameObject.FindObjectsOfType<RandomZone>(true);

        for (int index = 0; index < itemParent.Length; index++)
        {
            nums[index] = new List<int>();
        }
        
        for (int i = 0; i < words.Length; i++)
        {
            int randomParent = Random.Range(0, itemParent.Length);
            //int randomChild = Random.Range(0, itemParent[randomParent].transform.childCount);
            int randomChild = GenerateRandomNumberRecursively(randomParent);
            GameObject item = Instantiate(words[i], itemParent[randomParent].transform.GetChild(randomChild));
            item.transform.localPosition = Vector3.up;
            item.transform.localScale = Vector3.one * 500;
        }
    }
    
    private int GenerateRandomNumberRecursively(int parentNum)
    {
        int randomChild = Random.Range(0, itemParent[parentNum].transform.childCount);

        if (nums[parentNum].Contains(randomChild))
        {
            return GenerateRandomNumberRecursively(parentNum);
        }
        else
        {
            nums[parentNum].Add(randomChild);
            return randomChild;
        }
    }

    public void CollectWord(Define.CollectWord word)
    {
        Debug.Log($"Collect : {word.ToString()}");
        collected[word.ToString()] = true;
        Managers.UI.FindPopup<UI_Main>().OnCollectGenoWord(word.ToString());

        if (IsComplete())
        {
            Managers.Quest.CompleteCurrentQuest();
            //quest.state = Define.QuestState.Completed;
        }
    }

    private bool IsComplete()
    {
        foreach (bool collected in collected.Values)
        {
            if (!collected)
                return false;
        }

        return true;
    }
}
