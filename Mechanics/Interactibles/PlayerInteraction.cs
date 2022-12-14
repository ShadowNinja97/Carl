using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public static Interactable focus;
    public Interactable[] interactables;
    //public KeyCode interactButton = KeyCode.E;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 3))
        {
            Interactable Interactable = hit.collider.GetComponent<Interactable>();
            interactables = hit.collider.GetComponents<Interactable>();
            if(Interactable != null)
            {
                focus = Interactable;
              
            }
            if(Interactable == null)
            {
                focus = null;
            }
        }

        else
        {

            focus = null;
            interactables = new Interactable[0];
        }

        if (focus != null && Input.GetKeyDown(KeyBinds.interact))
        {
            //focus.Interact();

            foreach (Interactable interact in interactables)
            {
                interact.Interact();
            }
            Debug.Log("Interacting");
        }
    }


}
