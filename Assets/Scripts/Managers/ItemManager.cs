using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemManager : MonoBehaviour
{
    public Dictionary<Item, int> items = new Dictionary<Item, int>();

    [SerializeField] private GameObject sunItemPrefab;
    [SerializeField] private GameObject waterItemPrefab;

    private GameObject[] instantiateArr = new GameObject[GENERATE_NUM];
    
    private const int GENERATE_NUM = 5;
    private const int COLLECT_NUM = 8;
    private RandomZone[] itemParent;
    private Dictionary<int, List<int>> nums = new Dictionary<int, List<int>>();
    
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    
    public void StartCollectSun()
    {
        GenerateItems(sunItemPrefab);
    }
    
    public void StartCollectWater()
    {
        GenerateItems(waterItemPrefab);
    }
    
    // 아이템 무작위 위치에 생성
    private void GenerateItems(GameObject itemPrefab)
    {
        itemParent = GameObject.FindObjectsOfType<RandomZone>(true);
        instantiateArr = new GameObject[GENERATE_NUM];

        for (int index = 0; index < itemParent.Length; index++)
        {
            nums[index] = new List<int>();
        }
        
        for (int i = 0; i < GENERATE_NUM; i++)
        {
            int randomParent = Random.Range(0, itemParent.Length);
            //int randomChild = Random.Range(0, itemParent[randomParent].transform.childCount);
            int randomChild = GenerateRandomNumberRecursively(randomParent);
            GameObject item = Instantiate(itemPrefab, itemParent[randomParent].transform.GetChild(randomChild));
            item.transform.localPosition = Vector3.up;
            instantiateArr[i] = item;
        }
    }

    
    // 난수 생성 재귀 함수 (이전에 생성된 난수와 겹치지 않도록)
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
    
    private void Initialize()
    {
        
    }

    public void DeleteItem(Item item)
    {
        if (items.ContainsKey(item))
        {
            if (items[item] == 1)
            {
                items.Remove(item);
            }
            else
            {
                items[item] -= 1;   
            }
        }
        else
        {
            return;
        }
    }

    public void CollectItem(Item item)
    {
        if (items.ContainsKey(item))
        {
            items[item] += 1;
        }
        else
        {
            items[item] = 1;
        }
    }

    public void CheckIsComplete(Item itemType)
    {
        if (items[itemType] >= COLLECT_NUM)
        {
            Managers.Quest.CompleteCurrentQuest();
            TurnOffAllItems();
        }
    }

    private void TurnOffAllItems()
    {
        foreach (GameObject item in instantiateArr)
        {
            item.SetActive(false);
        }
    }
    
}
