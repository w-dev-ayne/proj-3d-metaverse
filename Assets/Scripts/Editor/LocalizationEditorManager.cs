using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class LocalizationEditorManager : MonoBehaviour
{
    public TMP_FontAsset font;
    public void AddComponents()
    {
        TextMeshProUGUI[] tmps = FindObjectsByType<TextMeshProUGUI>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (TextMeshProUGUI tmp in tmps)
        {
            if (!tmp.TryGetComponent<LocalizationTmp>(out LocalizationTmp lTmp))
            {
                tmp.AddComponent<LocalizationTmp>();
            }
            else
            {
                lTmp.tmp = tmp;
                lTmp.tmp.font = font;
                
                ApplyChanges(FindRootParent(tmp.gameObject).gameObject);
            }
        }
    }
    
    private void ApplyChanges(GameObject modifiedPrefab)
    {
        // 수정된 프리팹이 유효한지 확인
        if (PrefabUtility.IsPartOfPrefabAsset(modifiedPrefab))
        {
            // 수정된 프리팹의 원본 프리팹을 가져옴
            GameObject originalPrefab = PrefabUtility.GetCorrespondingObjectFromSource(modifiedPrefab);

            // 원본 프리팹이 있는지 확인
            if (originalPrefab != null)
            {
                // 수정된 프리팹의 변화를 원본 프리팹에 적용
                PrefabUtility.SaveAsPrefabAsset(modifiedPrefab, PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(originalPrefab));
            }
            else
            {
                Debug.LogWarning("원본 프리팹을 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("이 객체는 프리팹의 일부가 아닙니다.");
        }
    }
    
    private Transform FindRootParent(GameObject obj)
    {
        Transform parentTransform = obj.transform.parent;

        // 부모가 없을 때까지 계속 탐색
        while (parentTransform != null && parentTransform.parent != null)
        {
            parentTransform = parentTransform.parent;
        }

        return parentTransform;
    }
}

[CustomEditor(typeof(LocalizationEditorManager))]
public class LocalizationManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LocalizationEditorManager editorManager = (LocalizationEditorManager)target;

        if (GUILayout.Button("Add Components"))
        {
            editorManager.AddComponents();
        }
    }
}
