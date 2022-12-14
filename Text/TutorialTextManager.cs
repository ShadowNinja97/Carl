using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TutorialTextManager : MonoBehaviour
{
    [Header("Keybind Texts")]
    public TextMeshPro movementText;
    public TextMeshPro interactText;
    public TextMeshPro sprintText;
    public TextMeshPro toggleSprintText;
    public TextMeshPro crouchText;
    public TextMeshPro jumpText;
    public TextMeshPro slideText;

    public void Start()
    {
        interactText.text = "Press \"" + KeyBinds.interact + "\" to interact.";
        sprintText.text = "Hold \"" + KeyBinds.sprint + "\" to sprint.";
        toggleSprintText.text = "Press \"" + KeyBinds.toggleSprint + "\" to toggle sprinting.";
        crouchText.text = "Press \"" + KeyBinds.crouch + "\" to crouch.";
        jumpText.text = "Press \"" + KeyBinds.jump + "\" to jump.";
        slideText.text = "Press \"" + KeyBinds.slide + "\" to slide.";
    }

}
