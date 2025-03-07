using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Android;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InitialLoadController : MonoBehaviour
{
    [SerializeField] private Image loadingBarImage;
    //[SerializeField] private RectTransform handle;
    //[SerializeField] private TextMeshProUGUI percentTmp;
    [SerializeField] private string preloadLabel;
    [SerializeField] private TextMeshProUGUI statusTMP;

    private AsyncOperationHandle downloadHandle;
    private object resultObj;

    private void Awake()
    {
#if UNITY_EDITOR
        //Managers.Scene.ChangeScene(Define.Scene.Main);
#endif
        
        
#if !UNITY_EDITOR
        PlayerPrefs.DeleteKey(Addressables.kAddressablesRuntimeDataPath);
#endif
        StartCoroutine(PreloadAsset());
    }
    
    private IEnumerator PreloadAsset()
    {
        statusTMP.text = "Loading Resource";
        
        AsyncOperationHandle<long> downloadSizeOperation = Addressables.GetDownloadSizeAsync(preloadLabel);
        
        yield return downloadSizeOperation;
        long downloadSize = downloadSizeOperation.Result;
        
        Addressables.Release(downloadSizeOperation);
        
        Debug.Log($"Previous Download Size : {downloadSize}");

        loadingBarImage.fillAmount = 0;
        
        if (downloadSize > 0)
        {
            downloadHandle = Addressables.DownloadDependenciesAsync(preloadLabel);
            
            while (!downloadHandle.IsDone)
            {
                DownloadStatus downloadStatus = downloadHandle.GetDownloadStatus();
                
                Debug.Log($"{downloadStatus.Percent.ToString("n2")}%");

                loadingBarImage.fillAmount = downloadStatus.Percent / 2;
                yield return null;
            }
            Addressables.Release(downloadHandle);
            
            Debug.Log("Done");
        }
        else
        {
            loadingBarImage.fillAmount = 0.5f;
        }

        StartLoadClips();
    }


    private void ChangeScene()
    {
        if (GameObject.FindObjectOfType<BaseScene>() != null) // Development 씬이 아니면
        {
            Managers.Scene.ChangeScene(Define.Scene.Main);
        }
    }
    
    public void StartLoadClips()
    {
        statusTMP.text = "오디오 클립을 다운로드 중입니다.";
        loadingBarImage.fillAmount = 0.5f;

        int clipCount = 0;
        int count = 0;
        Debug.Log($"Load Clip Start");
        IList<IResourceLocation> locations;
        Addressables.LoadResourceLocationsAsync("sound").Completed += (handle) =>
        {
            locations = handle.Result;
            clipCount = handle.Result.Count;

            foreach (IResourceLocation location in locations)
            {
                Addressables.LoadAssetAsync<AudioClip>(location).Completed += (audioHandle) =>
                {
                    AudioClip clip = audioHandle.Result;
                    Debug.Log(audioHandle.Result.name);
                    
                    if (!Managers.Sound.clips.ContainsKey(clip.name))
                    {
                        Managers.Sound.clips.Add(clip.name, clip);
                        loadingBarImage.fillAmount = 0.5f + ((float)count / (float)(2 * clipCount));
                        //Debug.Log($"{(count / clipCount)}");
                        Debug.Log($"Clip Add : {clip.name}");
                        count++;
                        Debug.Log($"Percent : {loadingBarImage.fillAmount}");
                    
                        if (clipCount == count)
                        {
                            loadingBarImage.fillAmount = 1.0f;
                            Debug.Log($"Load Clip Finished");
                            ChangeScene();
                        }
                    }
                };
            }
            Addressables.Release(handle);
        };
    }

    private void CaptureCurrentScreen()
    {
        
    }
}
