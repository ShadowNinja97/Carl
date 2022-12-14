using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP_Enemy : MonoBehaviour
{

    public GameObject player;

    public float range;
    public float speed;
    

    public bool following;

    Quaternion rotGoal;
    Vector3 direction;
    public float rotateSpeed = 0.1f;
    public bool justLook = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        following = CanFollow();
        MoveTowardsPlayer();

        
    }

    public bool CanFollow()
    {
        
        if(Vector3.Distance(transform.position, player.transform.position)<range)
        {
            return true;
        }
        return false;
    }

    public void MoveTowardsPlayer()
    {
        if (following)
        {
            if(!justLook)
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

            direction = (player.transform.position - transform.position).normalized;
            direction.y = 0;
            rotGoal = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotGoal, rotateSpeed);
        }
    }

    
}
