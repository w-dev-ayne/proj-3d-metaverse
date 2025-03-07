using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.EventSystems;

public class TeleportManager : MonoBehaviour
{
    public Define.Place currentPlace = Define.Place.Hall;
    public Room[] rooms;
    public Room currentRoom;
    
    public Define.Place destination = Define.Place.None;
    
    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void StartTeleport(Define.Place destination)
    {
        this.destination = destination;

        Managers.UI.ShowPopupUI<UI_Fade>();
    }

    public void Teleport()
    {
        foreach (Room room in rooms)
        {
            if (room.place == this.destination)
            {
                currentRoom = room;
            }
            else
            {
                room.gameObject.SetActive(false);
            }
        }

        SetCharacterPosition();

        currentPlace = destination;
        destination = Define.Place.None;
    }

    private void SetCharacterPosition()
    {
        //Transform character = GameObject.FindWithTag("Player").transform;
        //Vector3 moveVector = character.position - currentRoom.portal.transform.position;
        GameObject.FindObjectOfType<CharacterTriggerController>().ResetPosition(currentRoom.portal.position, () =>
        {
            currentRoom.portal.GetComponent<CharacterTrigger>().init = false;
            currentRoom.gameObject.SetActive(true);
        });
    }
}
