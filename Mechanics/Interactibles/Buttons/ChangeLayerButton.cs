using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLayerButton : Interactable
{
    [SerializeField]
    public LayerSwaper[] LayerSwaps;

    [System.Serializable]
    public struct LayerSwaper
    {
        [Tooltip("Only use one layer.")]
        public LayerMask layer;
        public GameObject[] objects;
    }

    public override void Interact()
    {
        foreach (LayerSwaper ls in LayerSwaps)
        {
            foreach (GameObject gObj in ls.objects)
            {
                gObj.layer = (int)Mathf.Log(ls.layer.value, 2);
            }
        }
    }

   
}
