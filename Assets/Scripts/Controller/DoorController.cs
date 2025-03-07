using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isOpen = false;
    private Animation animation;

    void Awake()
    {
        this.animation = this.GetComponent<Animation>();
    }
    
    public void OnEnterDoor()
    {
        if (!isOpen)
        {
            animation["DoorAnimation"].speed = 1f;
            animation["DoorAnimation"].time = 0;
            animation.Play("DoorAnimation");
            isOpen = true;
            
            Managers.Sound.PlayAudioClip(Define.Sound.Effect, SoundDefine.DoorOpenEffect);
        }
        else
        {
            animation["DoorAnimation"].speed = -1f;
            animation["DoorAnimation"].time = animation["DoorAnimation"].length;
            animation.Play("DoorAnimation");
            isOpen = false;
        }
    }
}
