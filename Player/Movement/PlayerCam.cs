using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform camHolder;

    float xRotation;
    float yRotation;

    public KeyCode sensitivityDown = KeyCode.DownArrow;
    public KeyCode sensitivityUp = KeyCode.UpArrow;

    public Camera weaponCam;

    public bool interuption;

    private void Start()
    {
        sensX = SettingsManager.mouseSensitivity;
        sensY = SettingsManager.mouseSensitivity;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(sensitivityUp))
        {
            sensX += 50;
            sensY += 50;
        }
        if(Input.GetKeyDown(sensitivityDown))
        {
            sensX -= 50;
            sensY -= 50;
        }
        if (sensX < 50)
        {
            sensX = 50;
            sensY = 50;
        }


        //Mouse Input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
        

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);


        //Rotate Camera and Orientation
        //if (!interuption)
        //{
            
            camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        //}

    }

    public void DoFov(float endValue, float time)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, time);
        weaponCam.DOFieldOfView(endValue, time);
    }

    public void DoTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
        weaponCam.transform.DOLocalRotate(new Vector3(0,0,zTilt), 0.25f);
    }

    public void OverrideMousePos(float a, float b)
    {
        xRotation = a;
        yRotation = b;
    }

    public float getFov() {
        return weaponCam.fieldOfView;
    }
}
