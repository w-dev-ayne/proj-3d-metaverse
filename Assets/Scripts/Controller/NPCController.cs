using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Serialization;

public class NPCController : MonoBehaviour
{
    public bool disappearOnComplete = false;
    
    [SerializeField] private string[] korMessages;
    [SerializeField] private string[] engMessages;

    private int index = 0;

    public void IncreaseIndex()
    {
        index++;
    }
    
    public string GetCurrentMessage()
    {
        return korMessages[index];
    }

    public string GetNextMessage()
    {
        index++;
        switch (Managers.Localization.language)
        {
            case Define.Language.English:
                return engMessages[index];
                break;
            case Define.Language.Korean:
                return korMessages[index];
                break;
            default:
                return korMessages[index];
                break;
        }
    }

    public void Appear()
    {
        foreach (Transform child in this.transform)
        {
            if (child.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
            {
                if (child.TryGetComponent<CharacterTrigger>(out CharacterTrigger trigger))
                {
                    continue;
                }
                renderer.enabled = true;
            }
            else if (child.TryGetComponent<ParticleSystem>(out ParticleSystem particle))
            {
                particle.Play();
            }
        }
    }

    public void Disappear()
    {
        foreach (Transform child in this.transform)
        {
            if (child.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
            {
                renderer.enabled = false;
            }
            else if (child.TryGetComponent<ParticleSystem>(out ParticleSystem particle))
            {
                particle.Play();
            }
        }
    }

    public int GetIndex()
    {
        return this.index;
    }
}
