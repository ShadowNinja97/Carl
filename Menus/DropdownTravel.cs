using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
public class DropdownTravel : MonoBehaviour
{
    public TMP_Dropdown levelsList;


    private string[] sceneNames;

    void Start()
    {
        int sceneNumber = SceneManager.sceneCountInBuildSettings;
        sceneNames = new string[sceneNumber];

        for (int i = 0; i < sceneNumber; i++)
        {
            sceneNames[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
        }
        
        levelsList.ClearOptions();
        List<string> levelOptions = new List<string>();

        foreach (string str in sceneNames)
        {
            levelOptions.Add(str);
        }
        levelsList.AddOptions(levelOptions);
        levelsList.RefreshShownValue();

    }


    public void StartScene()
    {
        SceneManager.LoadScene(levelsList.value);
    }
}
