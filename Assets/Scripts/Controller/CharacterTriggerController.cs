using System;
using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Events;

public class CharacterTriggerController : MonoBehaviour
{
    public CharacterTrigger currentTriggerZone;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CharacterTrigger>(out CharacterTrigger zone))
        {
            currentTriggerZone = zone;
            zone.onCharacterIn.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<CharacterTrigger>(out CharacterTrigger zone))
        {
            currentTriggerZone = null;
            zone.onCharacterOut.Invoke();
        }
    }

    
    // 캐릭터 위치 강제 변경
    public void ResetPosition(Vector3 position, UnityAction onPositionSet = null)
    {
        StartCoroutine(CoResetPosition(position, onPositionSet));
    }

    private IEnumerator CoResetPosition(Vector3 position, UnityAction onPositionSet = null)
    {
        this.GetComponent<vThirdPersonInput>().enabled = false;

        //yield return null;
        
        this.transform.position = position;

        //Transform 연산 대기 ( Unity Life Cycle )
        yield return new WaitForFixedUpdate();
        
        this.GetComponent<vThirdPersonInput>().enabled = true;
        
        onPositionSet?.Invoke();
    }
}
