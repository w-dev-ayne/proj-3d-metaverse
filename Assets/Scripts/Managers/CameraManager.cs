using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public void OnAnimationFinished()
    {
        Managers.Teleport.currentRoom.ResetCamera();
    }
}
