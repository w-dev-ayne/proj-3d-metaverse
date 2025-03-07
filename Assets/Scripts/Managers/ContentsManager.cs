using System;
using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.Util;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ContentsManager : MonoBehaviour
{
    private List<GameObject> objs = new List<GameObject>();
    
    private float deltaTime = 0f;

    [SerializeField] private int size = 25;
    [SerializeField] private Color color = Color.white;
    [SerializeField] private GameObject loadingObj;
    [SerializeField] private GameObject fadeObj;
    
    
    private void Awake()
    {
        Application.targetFrameRate = 60;

        Addressables.InitializeAsync(true).Completed += (handle) => LoadAssets();
    }

    private void LoadAssets()
    {
        int count = 0;
        //Invoke("OffLoading", 1.0f);
        
        Debug.Log("Start Load Assets");
        
        Addressables.LoadResourceLocationsAsync("init").Completed +=
            (handle) =>
            {
                var locations = handle.Result;
                int maxCount = locations.Count;
                    
                foreach (var location in locations)
                {
                    Addressables.InstantiateAsync(location).Completed += handle =>
                    {
                        objs.Add(handle.Result);
                        
                        if (handle.Result.name.Contains("Lab"))
                        {
                            handle.Result.gameObject.SetActive(false);
                        }
                        else if (handle.Result.name.Contains("tage"))
                        {
                            Managers.Teleport.currentRoom = handle.Result.GetComponent<Room>();
                        }
                        else if (handle.Result.name.Contains("Character"))
                        {
                            handle.Result.transform.position = new Vector3(2, 60, 30);
                            
                            GameObject.Find("JumpButton").BindEventOnPointerDown(() =>
                            {
                                handle.Result.GetComponent<vThirdPersonController>().Jump();
                            });
                        }

                        count++;

                        if (count == maxCount)
                        {
                            OnLoadInitObjects();
                        }
                        
                        Managers.Teleport.rooms = FindObjectsByType<Room>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                    };    
                }
            };
    }

    private void OnLoadInitObjects()
    {
        NPCController[] npcs = FindObjectsOfType<NPCController>(true);

        foreach (NPCController npc in npcs)
        {
            npc.GetComponent<RotateToNpc>().target = FindObjectOfType<CharacterTriggerController>().transform;
        }
                            
        loadingObj.SetActive(false);
        fadeObj.SetActive(true);
        
        //Managers.Sound.PlayAudioClip(Define.Sound.Bgm, SoundDefine.MainSceneBGM);
    }
    
    private void OffLoading()
    {
        loadingObj.SetActive(false);
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }
    
    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(30, 30, Screen.width, Screen.height);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = size;
        style.normal.textColor = color;

        float ms = deltaTime * 1000f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.} FPS ({1:0.0} ms)", fps, ms);

        GUI.Label(rect, text, style);
    }

    private void OnDestroy()
    {
        foreach (GameObject obj in objs)
        {
            Addressables.ReleaseInstance(obj);
        }
    }
}
