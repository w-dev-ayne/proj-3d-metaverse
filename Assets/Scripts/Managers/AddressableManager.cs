using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Android;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;

public static class AddressableManager
{
    /// <summary>
    /// Addressable로 생성되는 모든 handle 정보 이 리스트에 저장.
    /// </summary>
    private static List<AsyncOperationHandle> handles = new List<AsyncOperationHandle>();


    public static void AddHandle(AsyncOperationHandle handle)
    {
        handles.Add(handle);
    }
    
    /// <summary>
    /// Addressable handle 전부 Release
    /// </summary>
    public static void ReleaseHandles()
    {
        foreach (AsyncOperationHandle handle in handles)
        {
            if (handle.IsValid())
            {
                Addressables.Release(handle);
                Debug.Log($"{handle} is destroyed.");    
            }
        }
    }
}