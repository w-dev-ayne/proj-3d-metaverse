using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToNpc : RotateTo
{
    // Update is called once per frame
    public override void OnUpdate()
    {
        if (target == null)
            return;
        
        //Debug.Log(Vector3.Distance(this.transform.position, target.position));
        if (Vector3.Distance(this.transform.position, target.position) < 10)
        {
            return;
        }
        
        this.transform.LookAt(target);
        
        if (onlyRotateY)
        {
            this.transform.localEulerAngles = Vector3.up * this.transform.localEulerAngles.y;
        }
    }
}
