using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class KeyRebinding : MonoBehaviour
{

    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public TextMeshProUGUI jumpT, sprintT, toggleSprintT, crouchT, slideT, shortenR, lengthenR, interactT;

    private GameObject currentKey;

    void Start()
    {
        keys.Add("Jump", KeyCode.Space);
        keys.Add("Sprint", KeyCode.LeftShift);
        keys.Add("Toggle Sprint", KeyCode.T);
        keys.Add("Crouch", KeyCode.C);
        keys.Add("Slide", KeyCode.LeftControl);
        keys.Add("Lengthen Rope", KeyCode.LeftControl);
        keys.Add("Shorten Rope", KeyCode.Space);
        keys.Add("Interact", KeyCode.E);

        jumpT.text = keys["Jump"].ToString();
        sprintT.text = keys["Sprint"].ToString();
        toggleSprintT.text = keys["Toggle Sprint"].ToString();
        crouchT.text = keys["Crouch"].ToString();
        slideT.text = keys["Slide"].ToString();
        lengthenR.text = keys["Lengthen Rope"].ToString();
        shortenR.text = keys["Shorten Rope"].ToString();
        interactT.text = keys["Interact"].ToString();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        if (currentKey != null)
        {
            Event e = Event.current;
            if(e.isKey)
            {
                keys[currentKey.name] = e.keyCode;
                currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                currentKey = null;
            }
        }
    }


    public void ChangeKey(GameObject clicked)
    {
        currentKey = clicked;
        currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "New Key...";
    }

    public void AssignKeys()
    {
        KeyBinds.jump = keys["Jump"];
        KeyBinds.sprint = keys["Sprint"];
        KeyBinds.toggleSprint = keys["Toggle Sprint"];
        KeyBinds.crouch = keys["Crouch"];
        KeyBinds.slide = keys["Slide"];
        KeyBinds.increaseRope = keys["Lengthen Rope"];
        KeyBinds.decreaseRope = keys["Shorten Rope"];
        KeyBinds.interact = keys["Interact"];
    }
}
