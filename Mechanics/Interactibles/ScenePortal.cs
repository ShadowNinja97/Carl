using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortal : MonoBehaviour
{
    [Scene] public int sceneIndex;

    private void OnTriggerEnter(Collider other)
    {

        SceneManager.LoadScene(sceneIndex);
    }
}
