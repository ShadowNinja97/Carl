using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class PlayerManager : MonoBehaviour
{
    public float maxHealth = 100f;

    public float health = 100f;

    public bool funnyLavaBounce;


    public Transform checkPoint;

    private Transform startCheckpoint;
    public Checkpoint currentCheckpoint;

    [NamedArray(new string[] { "Checkpoint", "Clip 2", "Clip 3" })]
    public AudioClip[] audioClips;

    public AudioMixerGroup[] mixers;

    private AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        health = maxHealth;
        startCheckpoint = checkPoint;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyBinds.forceRespawn))
        {
            if (checkPoint != null)
                KillPlayer();
        }

        if (Input.GetKeyDown(KeyBinds.forceRestartLevel))
        {
            if (checkPoint != null)
            {
                ResetCheckPoint();
                KillPlayer();
            }
        }

        //Temp
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Exit");
            Application.Quit();
        }
    }

    public void DamagePlayer(int damage, bool isLava)
    {
        health -= damage;

        if (health <= 0)
        {
            KillPlayer();
        }

        if (isLava && funnyLavaBounce)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.up * 30, ForceMode.Impulse);
        }
        else if (isLava)
        {
            KillPlayer();
        }

    }

    public void KillPlayer()
    {
        health = maxHealth;
        gameObject.transform.position = checkPoint.position;
        gameObject.transform.rotation = checkPoint.rotation;
        PlayerCam cam = GameObject.FindObjectOfType<PlayerCam>();
        cam.OverrideMousePos(checkPoint.eulerAngles.x, checkPoint.eulerAngles.y);
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

        /*TimeTrialPerfectCollectible ttpc = FindObjectOfType<TimeTrialPerfectCollectible>();
        if (ttpc != null)
        {
            TimeTrialPerfectCollectible.deaths++;
        }*/

        TryCatches();

        PlayerMovement pm = gameObject.GetComponent<PlayerMovement>();
        Swinging sw = gameObject.GetComponent<Swinging>();

        if (sw != null)
        {
            if (pm.swinging)
                sw.StopSwing();
        }
        CollectibleManager.ResetCollectibles();
        TimeTrials tt = gameObject.GetComponent<TimeTrials>();
        if (tt != null)
        {
            
            if (checkPoint.GetComponentInParent<Checkpoint>().isStartingCheckpoint)
            {
                /*if (ttpc != null)
                {
                    TimeTrialPerfectCollectible.deaths = 0;
                }*/
                tt.ResetTimer();
                CollectibleManager.FullReset();
            }
        }


    }

    public void SetCheckPoint(Transform newCheckPoint)
    {
        if (checkPoint != newCheckPoint)
        {
            audioSource.outputAudioMixerGroup = mixers[0];
            audioSource.clip = audioClips[0];
            audioSource.Play();
        }

        checkPoint = newCheckPoint;
        CollectibleManager.SaveCollectible();
        Checkpoint cp = newCheckPoint.gameObject.GetComponentInParent<Checkpoint>();
        if (cp != null)
        {
            currentCheckpoint = cp;
        }
    }

    public void ResetCheckPoint()
    {
        checkPoint = startCheckpoint;
    }

    private void TryCatches()
    {
        try
        {
            EnemySpawner[] es = FindObjectsOfType<EnemySpawner>();
            foreach (EnemySpawner enemy in es)
                enemy.ResetSpawns();

        }
        catch { }

        try
        {
            FindObjectOfType<ResetDoors>().ResetDoor();
        }
        catch { }
    }
}
