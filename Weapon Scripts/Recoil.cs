using UnityEngine;

public class Recoil : MonoBehaviour
{
    [SerializeField] private Camera recoilCam;
    private GunAbstract gun;
    private bool inADS;

    // Rotations
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    // ADS Recoil https://www.youtube.com/watch?v=geieixA4Mqc

    // Settings
    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        gun = recoilCam.GetComponent<CameraPrompt>().heldObject.GetComponent<GunAbstract>();

        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void RecoilFire() {
        if (inADS) {
            Debug.Log("In ADS");
            targetRotation += new Vector3(gun.ADSRecoilX, Random.Range(-gun.ADSRecoilY, gun.ADSRecoilY), Random.Range(-gun.ADSRecoilZ, gun.ADSRecoilZ));
        }
        else {
            Debug.Log("Not in ADS");
            targetRotation += new Vector3(gun.recoilX, Random.Range(-gun.recoilY, gun.recoilY), Random.Range(-gun.recoilZ, gun.recoilZ));
        }
    }
    public void setADS(bool ADS) {
        inADS = ADS;
    }
}
