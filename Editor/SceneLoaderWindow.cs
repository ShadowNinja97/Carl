using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class SceneLoaderWindow : EditorWindow
{
    private Vector2 scrollPos;

    


    [MenuItem("Scenes/Scene Loader Window")]
    internal static void Init()
    {
        var window = (SceneLoaderWindow)GetWindow(typeof(SceneLoaderWindow), false, "Scene Loader");
        window.position = new Rect(window.position.xMin + 100f, window.position.yMin + 100f, 200f, 400f);
    }

    internal void OnGUI()
    {
        var selectedTextStyle = new GUIStyle(GUI.skin.GetStyle("Button"));
        selectedTextStyle.normal.textColor = Color.green;
        selectedTextStyle.alignment = TextAnchor.MiddleLeft;

        EditorGUILayout.BeginVertical();
        this.scrollPos = EditorGUILayout.BeginScrollView(this.scrollPos, false, false);


        GUILayout.Label("Scenes In Build", EditorStyles.boldLabel);
        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            var scene = EditorBuildSettings.scenes[i];
            var openScene = EditorSceneManager.GetActiveScene();
            string openSceneName = openScene.name;

            if (scene.enabled)
            {
                bool pressed;
                string sceneName = Path.GetFileNameWithoutExtension(scene.path);
                if (openSceneName==sceneName)
                {
                    pressed = GUILayout.Button(i + ": " + sceneName, selectedTextStyle);
                }
                else
                    pressed = GUILayout.Button(i + ": " + sceneName, new GUIStyle(GUI.skin.GetStyle("Button")) { alignment = TextAnchor.MiddleLeft });
                if (pressed)
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(scene.path);
                    }
                }
            }
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

    }
}
