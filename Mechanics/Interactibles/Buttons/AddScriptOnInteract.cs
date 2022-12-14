using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AddScriptOnInteract : Interactable
{
    private GameObject player;
    public string script;

    [Header("Values")]//Used to assign multiple variables
    public int[] intValues;
    public float[] floatValues;
    public string[] stringValues;
    public TMPro.TextMeshProUGUI[] textObjects;
    public GameObject[] gameObjects;

    public override void Interact()
    {
        GetComponent<AudioSource>().Play();
        GetComponentInParent<Animator>().Play("BasicButtonClickAnim1");
        if (player.gameObject.GetComponent(Type.GetType(script)) != null)
            return;
        player.AddComponent(Type.GetType(script));
        AssignValues();
    }

    public void AssignValues()
    {
        if (script.Equals("TimeTrials"))
        {
            TimeTrials tt = player.GetComponent<TimeTrials>();
            tt.bronzeTime = floatValues[0];
            tt.silverTime = floatValues[1];
            tt.goldTime = floatValues[2];
            tt.timerDisplay = textObjects[0];
            tt.end = gameObjects[0];
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player.transform.parent != null)
            player = player.transform.parent.gameObject;
    }


}

#if UNITY_EDITOR
[CustomEditor(typeof(AddScriptOnInteract))]
public class AddScriptOnInteractEditor : Editor
{
    public override void OnInspectorGUI()
    {

        AddScriptOnInteract aSOI = (AddScriptOnInteract)target;

        aSOI.script = EditorGUILayout.TextField("Script Name", aSOI.script); //Leave as is, this determines the script name.

        if (aSOI.script.Equals("TimeTrials"))//If the script name is "TimeTrials" this code runs.
        {
            Array.Resize(ref aSOI.floatValues, 3); //Resizes the array to be only the needed amount.
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(aSOI.floatValues)), new GUIContent("Float Value: ")); //Draws the array
            Array.Resize(ref aSOI.textObjects, 1);
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(aSOI.textObjects)), new GUIContent("Text Object: "));
            Array.Resize(ref aSOI.gameObjects, 1);
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(aSOI.gameObjects)), new GUIContent("Game Object: "));

            serializedObject.ApplyModifiedProperties();
        }

    }
}
#endif