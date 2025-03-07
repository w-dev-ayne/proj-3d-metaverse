using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class UIManager
{
	int _order = -20;

	Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();

	public const string Button_Sound = "Pop Low";

	public UI_Scene SceneUI { get; private set; }

	public GameObject Root
	{
		get
		{
			GameObject root = GameObject.Find("@UI_Root");
			if (root == null)
				root = new GameObject { name = "@UI_Root" };
			
			return root;
		}
	}

	public void SetCanvas(GameObject go, bool sort = true)
	{
		Canvas canvas = Utils.GetOrAddComponent<Canvas>(go);
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		canvas.overrideSorting = true;
		if (sort)
		{
			canvas.sortingOrder = _order;
			_order++;
		}
		else
		{
			canvas.sortingOrder = 0;
		}
	}

	public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
	{
		if (string.IsNullOrEmpty(name))
			name = typeof(T).Name;

		GameObject prefab = Managers.Resource.Load<GameObject>($"Prefabs/UI/SubItem/{name}");

		GameObject go = Managers.Resource.Instantiate(prefab);
		if (parent != null)
			go.transform.SetParent(parent);

		go.transform.localScale = Vector3.one;
		go.transform.localPosition = prefab.transform.position;

		return Utils.GetOrAddComponent<T>(go);
	}

	public T ShowSceneUI<T>(string name = null) where T : UI_Scene
	{
		if (string.IsNullOrEmpty(name))
			name = typeof(T).Name;

		GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
		T sceneUI = Utils.GetOrAddComponent<T>(go);
		SceneUI = sceneUI;

		go.transform.SetParent(Root.transform);

		return sceneUI;
	}

	public void ShowPopupUI<T>(string name = null, Transform parent = null) where T : UI_Popup
	{
		name = typeof(T).Name;
		Debug.Log($"Name : {name}");
		Addressables.LoadAssetAsync<GameObject>(name).Completed += handle =>
		{
			AddressableManager.AddHandle(handle);
			Addressables.InstantiateAsync(name).Completed += iHandle =>
			{
				AddressableManager.AddHandle(iHandle);
				T popup = Utils.GetOrAddComponent<T>(iHandle.Result);
				_popupStack.Push(popup);
				
				if (parent != null)
					popup.transform.SetParent(parent);
				else if (SceneUI != null)
					popup.transform.SetParent(SceneUI.transform);
				else
					popup.transform.SetParent(Root.transform);

				popup.transform.localScale = Vector3.one;
				popup.transform.localPosition = Vector3.zero;
				
				Managers.Localization.OnPopup(popup.transform);
			};
		};
	}

	public T FindPopup<T>() where T : UI_Popup
	{
		return _popupStack.Where(x => x.GetType() == typeof(T)).FirstOrDefault() as T;
	}


	public T PeekPopupUI<T>() where T : UI_Popup
	{
		if (_popupStack.Count == 0)
			return null;

		return _popupStack.Peek() as T;
	}

	public void ClosePopupUI(UI_Popup popup)
	{
		if (_popupStack.Count == 0)
			return;
		
		if(_popupStack.Peek() != popup)
		{
			Stack<UI_Popup> peekedStack = new Stack<UI_Popup>();

			int orderCount = 0;
			
			while (_popupStack.Peek() != popup)
			{
				UI_Popup poppedPopup = _popupStack.Pop();
				peekedStack.Push(poppedPopup);
				poppedPopup.GetComponent<Canvas>().sortingOrder -= 1;
				orderCount++;
			}
			
			ClosePopupUI();

			while (peekedStack.Count != 0)
			{
				UI_Popup peekedPopup = peekedStack.Pop();
				_popupStack.Push(peekedPopup);
				//peekedPopup.GetComponent<Canvas>().sortingOrder -= orderCount;
			}
			
		}
		else
		{
			ClosePopupUI();	
		}
	}

	public void ClosePopupUI()
	{
		if (_popupStack.Count == 0)
			return;
		
		UI_Popup popup = _popupStack.Pop();
		Debug.Log($"Close {popup}");

		if (popup.transform.GetComponentInChildren<DoTweenAppear>() != null)
		{
			//popup.GetComponent<Canvas>().sortingOrder -= 1;
			popup.transform.GetComponentInChildren<DoTweenAppear>().Disappear(() =>
			{
				Addressables.ReleaseInstance(popup.gameObject);
				popup = null;
			});
			_order--;
		}
		else
		{
			Addressables.ReleaseInstance(popup.gameObject);
			popup = null;
			_order--;
		}
	}

	public void CloseAllPopupUI()
	{
		while (_popupStack.Count > 0)
			ClosePopupUI();
	}
	

	public void Clear()
	{
		CloseAllPopupUI();
		SceneUI = null;
	}
}
