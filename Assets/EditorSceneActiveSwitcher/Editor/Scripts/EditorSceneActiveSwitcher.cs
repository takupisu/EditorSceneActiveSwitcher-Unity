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
    // チェックボックス表示位置
    const float Width = 40f;

    static EditorSceneSwitcher () {
        EditorApplication.hierarchyWindowItemOnGUI += DrawComponentToggle;
    }

	static void DrawComponentToggle (int instanceID, Rect rect)
	{
		if(Application.isPlaying) {
			return;
		}

        if (EditorUtility.InstanceIDToObject(instanceID)) {
			return;
		}

        // シーンオブジェクト取得
		var miGetSceneByHandle = typeof(EditorSceneManager).GetMethod("GetSceneByHandle",BindingFlags.NonPublic | BindingFlags.Static);
		Scene s = (Scene)miGetSceneByHandle.Invoke(null, new object[]{instanceID});

        // チェックボックス表示位置
        rect.x += rect.width - Width;
        rect.width = Width;

        // チェックボックス表示
        if (s.isLoaded != GUI.Toggle(rect, s.isLoaded, ""))
        {
			if(s.isLoaded) {
				EditorSceneManager.SaveScene(s);
				EditorSceneManager.CloseScene(s, false);
			} else {
				EditorSceneManager.OpenScene(s.path, OpenSceneMode.Additive);
			}
		}
	}
}
#endif
