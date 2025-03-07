using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTo : MonoBehaviour
{
    public Transform target;
    public bool onlyRotateY = false;

    private void Start()
    {
        if (target == null)
            target = Camera.main.transform;
    }

    public virtual void OnUpdate()
    {
        if (target == null)
            target = Camera.main.transform;
        
        this.transform.LookAt(target);
        
        if (onlyRotateY)
        {
            this.transform.localEulerAngles = Vector3.up * this.transform.localEulerAngles.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdate();
    }
}
