using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuButtons : MonoBehaviour
{

    public GameObject KeybindPanel;
    public GameObject LevelPanel;

    public AudioClip[] clips;

    public void ToggleLevelPanel()
    {
        KeybindPanel.SetActive(false);
        LevelPanel.SetActive(!LevelPanel.activeSelf);
    }


    public void LoadScene(int x)
    {
        GetComponent<KeyRebinding>().AssignKeys();
        SceneManager.LoadScene(x);
    }

    public void Button2()
    {
        GetComponent<AudioSource>().clip = clips[1];
        GetComponent<AudioSource>().Play();
        //GetComponent<AudioSource>().clip = clips[0];

    }

    public void ToggleKeybindPanel()
    {
        LevelPanel.SetActive(false);
        KeybindPanel.SetActive(!KeybindPanel.activeSelf);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
