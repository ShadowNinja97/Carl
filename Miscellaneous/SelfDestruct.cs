using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float lifetime;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destruct", lifetime);
    }

    void Destruct() {
        Destroy(gameObject);
    }
}
