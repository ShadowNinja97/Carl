using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DoorToggleButton : Interactable
{
    [Header("Door Settings")]
    public GameObject door;
    private Animator anim;
    
    public bool standardDoorAnimation = true;
    [HideInInspector]
    public string doorAnimNameOpen;
    [HideInInspector]
    public string doorAnimNameClose;

    public bool buttonHasAnimation = true;
    [HideInInspector]
    public bool defaultAnimation = true;
    [HideInInspector]
    public string animationName;

    public bool doorOpen = false;


    private void Start()
    {
        anim = door.GetComponent<Animator>();

    }

    public void OpenDoor()
    {
        doorOpen = true;
        if (standardDoorAnimation)
            anim.Play("StandardDoorOpening");
        else
            anim.Play(doorAnimNameOpen);

    }

    public void CloseDoor()
    {
        if (standardDoorAnimation)
            anim.Play("StandardDoorClosing");
        else
            anim.Play(doorAnimNameClose);
        doorOpen = false;
    }

    public override void Interact()
    {
        GetComponent<AudioSource>().Play();
        Debug.Log("Interacted");
        StartCoroutine(DoorToggle());
        if (defaultAnimation)
            GetComponentInParent<Animator>().Play("BasicButtonClickAnim1");
        else if(buttonHasAnimation)
            GetComponentInParent<Animator>().Play(animationName);
    }

    IEnumerator DoorToggle()
    {
        Debug.Log("Toggling");
        if (doorOpen)
        {
            CloseDoor();
            doorOpen = false;
        }
        else
        {
            OpenDoor();
            doorOpen = true;
        }
        yield return new WaitForSeconds(1);
        
        
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DoorToggleButton))]
public class DoorToggleButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DoorToggleButton dTB = (DoorToggleButton)target;

        if (!dTB.standardDoorAnimation)
        {
            dTB.doorAnimNameOpen = EditorGUILayout.TextField("Door Open Animation", dTB.doorAnimNameOpen);
            dTB.doorAnimNameClose = EditorGUILayout.TextField("Door Close Animation", dTB.doorAnimNameClose);
        }

        if (dTB.buttonHasAnimation)
        {
            dTB.defaultAnimation = EditorGUILayout.Toggle("Default Animations", dTB.defaultAnimation);

            if (!dTB.defaultAnimation)
                dTB.animationName = EditorGUILayout.TextField("Button Animation Name", dTB.animationName);
        }


    }
}
#endif