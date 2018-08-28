#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Reflection;

/// <summary>
/// Hierarchy上にチェックボックスでシーンのLoad、UnLoad状態を切り替えれる機能
/// </summary>
[InitializeOnLoad]
public class EditorSceneSwitcher : Editor {

	static EditorSceneSwitcher () {
		EditorApplication.hierarchyWindowItemOnGUI += (instanceID, rect) => {
			DrawComponentIcons (instanceID, rect);
		};
	}

	static void DrawComponentIcons (int instanceID, Rect rect)
	{
		if(Application.isPlaying) {
			return;
		}

		// チェックボックス表示位置
		rect.x += rect.width - 40;
		rect.width = 40;

		if(EditorUtility.InstanceIDToObject(instanceID)) {
			return;
		}

		var setups = EditorSceneManager.GetSceneManagerSetup();
		if(null == setups || setups.Length <= 0) {
			return;
		}

		var miGetSceneByHandle = typeof(EditorSceneManager).GetMethod("GetSceneByHandle",BindingFlags.NonPublic | BindingFlags.Static);
		Scene s = (Scene)miGetSceneByHandle.Invoke(null, new object[]{instanceID});

		if (s.isLoaded != GUI.Toggle(rect,s.isLoaded,"")){
			for (int i = 0; i < setups.Length; i++){
				if (setups[i].path == s.path){
					setups[i].isLoaded = !s.isLoaded;
					if(s.isLoaded) {
						EditorSceneManager.SaveScene(s);
						EditorSceneManager.CloseScene(s, false);
					} else {
						EditorSceneManager.OpenScene(s.path, OpenSceneMode.Additive);
					}
					break;
				}
			}
		}
	}
}
#endif
