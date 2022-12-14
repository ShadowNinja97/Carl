using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public KeyCode pauseKey = KeyCode.Escape;

    public GameObject pausePanel;

    [HideInInspector]
    public bool pauseGame;

    private void Start()
    {
        pauseGame = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            pauseGame = !pauseGame;
            pausePanel.SetActive(pauseGame);
            if (pauseGame)
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        
    }

    public void SetScene(int sceneIndex)
    {
        Time.timeScale = 1f;
        pauseGame = false;
        SceneManager.LoadSceneAsync(sceneIndex);
        
    }

    public void Options()
    {
        //Later
    }

    public void Resume()
    {
        pauseGame = !pauseGame;
        pausePanel.SetActive(pauseGame);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
