using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    private IEnumerator Co()
    {
        WaitForSeconds oneSecond = new WaitForSeconds(1.0f);

        int count = 0;
        while (true)
        {
            Debug.Log($"Count : {count} | Answer : {(int)((count-1) / 2)}");
            yield return oneSecond;
            count++;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Co());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
