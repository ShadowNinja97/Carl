using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWaypoint : MonoBehaviour
{
    public float pauseDuration;

    public enum Draw
    {
        Wire,
        Filled,
        ClearWire,
        ClearFilled
    }

    public Draw draw;

    private void OnDrawGizmos()
    {

        foreach (Transform t in transform)
        {
            Gizmos.color = Color.blue;
            if (draw == Draw.Wire || draw == Draw.ClearWire)
            {
                if (draw==Draw.ClearWire&& t == transform.GetChild(0))
                    Gizmos.color = Color.green;
                if (draw == Draw.ClearWire && t == transform.GetChild(transform.childCount - 1))
                    Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(t.position, 1f);
            }
            else
            {
                if (draw == Draw.ClearFilled && t == transform.GetChild(0))
                    Gizmos.color = Color.green;
                if (draw == Draw.ClearFilled && t == transform.GetChild(transform.childCount - 1))
                    Gizmos.color = Color.red;
                Gizmos.DrawSphere(t.position, 1f);
            }
        }
        Gizmos.color = Color.red;

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
        }

    }

    public Transform GetNextWaypoint(Transform currentWaypoint)
    {

        if (currentWaypoint.GetSiblingIndex() < transform.childCount - 1)
        {
            return transform.GetChild(currentWaypoint.GetSiblingIndex() + 1);
        }
        else
        {
          
            //return transform.GetChild(0);
            
        }

        return null;
    }


    public Transform SetInitialWaypoint(int pos)
    {
        return transform.GetChild(pos);
    }

    public int GetMaxWaypoint()
    {
        return transform.childCount - 1;
    }
}
