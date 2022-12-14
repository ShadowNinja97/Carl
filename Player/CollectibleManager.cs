using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CollectibleManager : MonoBehaviour
{

    [NamedArray(new string[] { "Star 1", "Star 2", "Star 3", "Star P" })]
    public GameObject[] starIconsInit;
    public static GameObject[] starIcons = new GameObject[4];

    [NamedArray(new string[] { "Star R", "Star B", "Star P", "Star G", "Star W" })]
    public Sprite[] icons;

    
    public static bool[] hasCollectibles = { false, false, false, false };
    private static bool[] savedCollectibles = { false, false, false, false };

    private MirrorCore mc;

    private void Start()
    {
        InitializeStatic();
        try
        {
            mc = FindObjectOfType<MirrorCore>();
        } catch { }
    }

    private void InitializeStatic()
    {
        for(int i = 0; i < 4; i++)
        {
            starIcons[i] = starIconsInit[i];
        }
    }


    private void Update()
    {
        EnableIcons();
        if (mc != null)
            MirrorIcons();
    }

    public static void SaveCollectible()
    {
        for (int i = 0; i < 4; i++)
        {
            if (hasCollectibles[i] && !savedCollectibles[i])
                savedCollectibles[i] = true;
        }
    }

    public static void ResetCollectibles()
    {
        Collectible[] colls = FindObjectsOfType<Collectible>();
        for (int i = 0; i < 4; i++)
        {
            if (hasCollectibles[i] && !savedCollectibles[i])
            {
                starIcons[i].SetActive(false);
                hasCollectibles[i] = false;
                colls[i].soundPlay = true;
                colls[i].perfectCollected = false;
            }

            colls[i].deaths++;
        }
        
        
    }

    public static void FullReset()
    {
        hasCollectibles = new bool[4];
        savedCollectibles = new bool[4];
        foreach (GameObject obj in starIcons)
            obj.SetActive(false);
        Collectible[] colls = FindObjectsOfType<Collectible>();
        foreach (Collectible coll in colls)
            coll.deaths = 0;
    }

    public void MirrorIcons()
    {
        if(MirrorCore.inMirrorWorld)
        {
            foreach (GameObject obj in starIcons)
            {
                obj.GetComponent<Image>().sprite = icons[4];
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                starIcons[i].GetComponent<Image>().sprite = icons[i];
            }
        }
    }

    private void EnableIcons()
    {
        for (int i = 0; i < 4; i++)
        {
            starIcons[i].SetActive(hasCollectibles[i]);
        }
    }
}
