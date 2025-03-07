using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkbenchManager : MonoBehaviour
{
    public Workbench currentWorkbench;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
