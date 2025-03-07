using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using static Define;

public class Managers : MonoBehaviour
{
    private static Managers s_instance = null;
    public static Managers Instance { get { return s_instance; } }

    private static UIManager s_uiManager = new UIManager();
    private static ResourceManager s_resourceManager = new ResourceManager();
    private static SceneManagerEx s_sceneManager = new SceneManagerEx();
    private static SoundManager s_soundManager = new SoundManager();
    private static LocalizationManager s_localizationManager = new LocalizationManager();

    private static CollectGenomeManager s_collectGenomeManager;
    private static TeleportManager s_teleportManager;
    private static CollectWordManager s_collectWordManager;
    private static ItemManager s_itemManager;
    private static WorkbenchManager s_workbenchManager;
    private static QuestManager s_questManager;
    private static APIManager s_apiManager;
    
    public static UIManager UI { get { Init(); return s_uiManager; } }
    public static ResourceManager Resource { get { Init(); return s_resourceManager; } }
    public static SceneManagerEx Scene { get { Init(); return s_sceneManager; } }
    public static SoundManager Sound {  get { Init(); return s_soundManager; } }
    public static LocalizationManager Localization { get { Init(); return s_localizationManager; } }
    
    public static CollectGenomeManager CollectGenome { get { Init(); return s_collectGenomeManager; } }
    public static TeleportManager Teleport { get { Init(); return s_teleportManager; }}
    public static CollectWordManager CollectWord { get { Init(); return s_collectWordManager; } }
    public static ItemManager Item { get { Init(); return s_itemManager;}}
    public static WorkbenchManager Workbench { get { Init(); return s_workbenchManager; } }
    public static QuestManager Quest { get { Init(); return s_questManager; } }
    public static APIManager API { get { Init(); return s_apiManager; } }


    private void Awake()
    {
#if !UNITY_EDITOR
        RNManager.SendMessageToMobileApp("Unity to RN : Start App");
#else
    Debug.Log("Start App");
#endif
    }

    private void Start() 
    {
        Init();
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
                go = new GameObject { name = "@Managers" };
            
            s_instance = Utils.GetOrAddComponent<Managers>(go);
            DontDestroyOnLoad(go);
            
            s_resourceManager.Init();
            s_sceneManager.Init();
            s_soundManager.Init();
            
            s_collectGenomeManager = InstantiateMonoBehaviourManager<CollectGenomeManager>();
            s_teleportManager = InstantiateMonoBehaviourManager<TeleportManager>();
            s_collectWordManager = InstantiateMonoBehaviourManager<CollectWordManager>();
            s_itemManager = InstantiateMonoBehaviourManager<ItemManager>();
            s_workbenchManager = InstantiateMonoBehaviourManager<WorkbenchManager>();
            s_questManager = InstantiateMonoBehaviourManager<QuestManager>();
            s_apiManager = InstantiateMonoBehaviourManager<APIManager>();
            s_localizationManager = InstantiateMonoBehaviourManager<LocalizationManager>();
            
            Application.targetFrameRate = 60;
        }
    }

    private static T InstantiateMonoBehaviourManager<T>() where T : class
    {
        GameObject managerObj = s_resourceManager.Instantiate($"Managers/@{typeof(T).ToString()}");
        return managerObj.GetComponent<T>();
    }

    private void OnDestroy()
    {
        AddressableManager.ReleaseHandles();
    }


    /*private void OnApplicationQuit()
    {
#if !UNITY_EDITOR    
        RNManager.SendMessageToMobileApp("Unity to RN (OnApplicationQuit) : Exit App");
#else
        Debug.Log("Exit App");
#endif
    }*/

}
