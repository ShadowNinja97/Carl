using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;

    public enum SpawnMode
    {
        Point,
        Area
    }
    public SpawnMode mode;

    [System.Flags]
    public enum SpawnSettings
    {
        RespawnOnReset = 1, //Respawn if player dies.
        RespawnOnDeath = 2, //Respawn if enemy dies. (UNIMPLEMENTED)
        SpawnContinuously = 4
    }

    public SpawnSettings settings;

    [Tooltip("Only useful if the mode is set to Area. Allows for enemies to spawn anywhere in the box.")]
    public Vector3 spawnArea;

    [Space(15)]
    [Tooltip("How many enemies are spawned upon loading.")]
    public int spawnCount;
    [Tooltip("Only useful if Settings contains SpawnContinuously.")]
    public int maxSpawn;
    private int currentCount;
    [Tooltip("Only useful if Settings contains SpawnContinuously.")]
    public float spawnDelay;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if (mode == SpawnMode.Point)
            Gizmos.DrawWireSphere(transform.position, 0.25f);
        if (mode == SpawnMode.Area)
            Gizmos.DrawWireCube(transform.position, spawnArea);
    }


    private void Start()
    {
        currentCount = 0;

        if (mode == SpawnMode.Point)
        {
            PointSpawn();
        }
        else if (mode == SpawnMode.Area)
        {
            for (int i = 0; i < 10; i++)
                AreaSpawn();
        }

        //ResetSpawns();

        if (settings.HasFlag(SpawnSettings.SpawnContinuously))
            InvokeRepeating("ContinuousSpawn", spawnDelay, spawnDelay);
    }

    private void Update()
    {
        currentCount = transform.childCount;
    }


    public void PointSpawn()
    {
        var newEnemy = Instantiate(enemy, transform.position, transform.rotation);
        newEnemy.transform.parent = gameObject.transform;
    }


    public void AreaSpawn()
    {

        Vector3 randomDisplacement = new Vector3();
        randomDisplacement.x = Random.Range((float)(-0.5 * spawnArea.x), (float)(0.5 * spawnArea.x));
        //randomDisplacement.y = Random.Range((float)(-0.5 * spawnArea.y), (float)(0.5 * spawnArea.y));
        randomDisplacement.z = Random.Range((float)(-0.5 * spawnArea.z), (float)(0.5 * spawnArea.z));

        Vector3 spawnHere = new Vector3(transform.position.x+randomDisplacement.x, transform.position.y+1, transform.position.z+randomDisplacement.z);

        var newEnemy = Instantiate(enemy, spawnHere, transform.rotation);
        newEnemy.transform.parent = gameObject.transform;

    }

    public void ResetSpawns()
    {
        Debug.Log("Test");
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        Debug.Log("Test2");

        if (settings.HasFlag(SpawnSettings.RespawnOnReset))
        {
            Debug.Log("Test3");
            if (mode == SpawnMode.Point)
            {
                PointSpawn();
            }
            else if (mode == SpawnMode.Area)
            {
                Debug.Log("Test4");
                for (int i = 0; i < spawnCount; i++)
                {
                    Debug.Log("Test5." + i);
                    AreaSpawn();
                }
            }
        }
    }

    private void ContinuousSpawn()
    {
        StartCoroutine(RepeatingSpawn());
    }

    IEnumerator RepeatingSpawn()
    {
        Debug.LogWarning("Test1");
        if (currentCount==maxSpawn)
        {
            yield return null;
        }
        Debug.LogWarning("Test2");
        if (mode == SpawnMode.Point)
        {
            PointSpawn();
        }

        if (mode == SpawnMode.Area)
        {
            AreaSpawn();
        }

    }


}
