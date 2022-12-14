using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineData : MonoBehaviour
{
    public static Color outlineColor;
    public static int outlineWidth;

    public Color color;
    public int width;

    public static bool outlineOn = false;

    private void Start()
    {
        outlineColor = color;
        outlineWidth = width;
    }

    public static void EnableOutline(GameObject obj)
    {
        if (!outlineOn)
        {
            outlineOn = true;
            OutlineScript scr = obj.AddComponent<OutlineScript>();
            scr.OutlineColor = outlineColor;
            scr.OutlineWidth = outlineWidth;
            scr.OutlineMode = OutlineScript.Mode.OutlineAll;
            Debug.LogWarning("Check2");
        }
    }

    public static void DisableOutline(GameObject obj)
    {
        try
        {
            Debug.LogWarning("Check3");
            outlineOn = false;
            Destroy(obj.GetComponent<OutlineScript>());
            Debug.Log("Check4");
        }
        catch { }
    }

}
