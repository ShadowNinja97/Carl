using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class WaypointSystem : MonoBehaviour
{
    public enum Mode
    {
        Looped,
        BackAndForth,
        Once
    }

    public Mode mode;

    [System.Flags]
    public enum Settings
    {
        PauseAtDesination = 1,
        ClearStartAndEnd = 2,
        ShowInGame = 4,
        Other = 8
    }

    public Settings settings;
    
    public enum Draw
    {
        Wire,
        Filled
    }

    public Draw draw;
    

    public bool isReversed = false;

    [SerializeField]
    private Material lineMat;

    private void OnDrawGizmos()
    {

        foreach (Transform t in transform)
        {
            Gizmos.color = Color.blue;
            if (draw == Draw.Wire)
            {
                if (settings.HasFlag(Settings.ClearStartAndEnd) && t == transform.GetChild(0))
                    Gizmos.color = Color.green;
                if (settings.HasFlag(Settings.ClearStartAndEnd) && t == transform.GetChild(transform.childCount-1))
                    Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(t.position, 1f);
            }
            else
            {
                if (settings.HasFlag(Settings.ClearStartAndEnd) && t == transform.GetChild(0))
                    Gizmos.color = Color.green;
                if (settings.HasFlag(Settings.ClearStartAndEnd) && t == transform.GetChild(transform.childCount - 1))
                    Gizmos.color = Color.red;
                Gizmos.DrawSphere(t.position, 1f);
            }
        }
        Gizmos.color = Color.red;

        for (int i = 0; i < transform.childCount -1; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);    
        }

        if(mode == Mode.Looped)
        {
            Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position, transform.GetChild(0).position);
        }

        

    }

    public Transform GetNextWaypoint(Transform currentWaypoint)
    {
        
        if(currentWaypoint.GetSiblingIndex() < transform.childCount - 1 && !isReversed)
        {
            return transform.GetChild(currentWaypoint.GetSiblingIndex() + 1);
        }
        else if(currentWaypoint.GetSiblingIndex() > 0 && isReversed)
        {
            return transform.GetChild(currentWaypoint.GetSiblingIndex() - 1);
        }
        else
        {
            if(mode == Mode.Looped)
            {
                return transform.GetChild(0);
            }
            else if (mode == Mode.BackAndForth)
            {
                isReversed = !isReversed;
                if(isReversed)
                    return transform.GetChild(currentWaypoint.GetSiblingIndex() - 1);
                else
                    return transform.GetChild(currentWaypoint.GetSiblingIndex() + 1);
            }
            else if (mode == Mode.Once)
            {
                isReversed = !isReversed;
                return null;
            }
        }

        return null;
    }


    public Transform SetInitialWaypoint(int pos)
    {
        return transform.GetChild(pos);
    }

    public int GetMaxWaypoint()
    {
        return transform.childCount-1;
    }


    private void Start()
    {

        if (settings.HasFlag(Settings.ShowInGame))
        {
            LineRenderer lr = gameObject.AddComponent<LineRenderer>();
            lr.startWidth = 0.05f;
            lr.endWidth = 0.05f;
            lr.startColor = Color.black;
            lr.endColor = Color.white;
            lr.positionCount = transform.childCount;
            lr.material = lineMat;
            for (int i = 0; i < transform.childCount; i++)
            {
                lr.SetPosition(i, transform.GetChild(i).position);
            }
            
            if(mode == Mode.Looped)
            {
                /*
                lr.positionCount = transform.childCount + 1;
                lr.SetPosition(transform.childCount, transform.GetChild(0).position);
                */
                lr.loop = true;
            }
        }
    }

}
