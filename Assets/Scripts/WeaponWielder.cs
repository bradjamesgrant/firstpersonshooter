using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponWielder : MonoBehaviour
{
    public Transform playerCamera;
    public Transform gunHolder;
    private Vector3 gunPos;
    public int ammoInRound;
    private bool isRotationLocked = true;
    private bool isFiring = false;
    private bool isReloading = false;
    public float recoilTime = 5.0f;
    public float reloadTime = 30.0f;
    private Weapon currentGun = null;
    public float recoilStrength = 70.0f; //recoil angle
    void Start()
    {
        InputManager.instance.Shoot += Fire;
        InputManager.instance.Reload += Reload;
        currentGun = gunHolder.GetComponentInChildren<Weapon>();
        gunPos = gunHolder.localPosition;

    }

    void Update()
    {
        if (isRotationLocked)
            gunHolder.rotation = playerCamera.rotation;
    }

    public void Fire()
    {
        if (currentGun.currentRoundsInClip > 0 && !isFiring & !isReloading)
        {
            isRotationLocked = false;
            isFiring = true;
            StartCoroutine(GunRecoil());
            currentGun.currentRoundsInClip -= 1;
        }

    }

    public void Reload()
    {
        isRotationLocked = false;
        Debug.Log("Reloading");
        //reload if: not already reloading, has rounds in chamber needing to be filled, and has adequate reserve ammo
        if (!isReloading && !isFiring && currentGun.currentRoundsInClip<currentGun.maxRoundsInClip && currentGun.reserveAmmo>0)
        {
            isReloading = true;
            StartCoroutine(SpinGun());

        }
    }

    IEnumerator SpinGun()
    {

        for (int i = 0; i < reloadTime; i++)
        {
            gunHolder.RotateAround(currentGun.transform.position, -transform.right, 360.0f / reloadTime);
            yield return new WaitForEndOfFrame();
        }
        isReloading = false;
        isRotationLocked = true;
        int ammoNeedingFilled = currentGun.maxRoundsInClip - currentGun.currentRoundsInClip;
        int ammoCanFill = Mathf.Min(currentGun.reserveAmmo, ammoNeedingFilled);
        currentGun.currentRoundsInClip += ammoCanFill;
        currentGun.reserveAmmo -= ammoCanFill;
        gunHolder.localPosition = gunPos;
    }

    IEnumerator GunRecoil()
    {
        for (int i = 0; i < recoilTime; i++)
        {
            gunHolder.RotateAround(currentGun.transform.position, -transform.right, recoilStrength / recoilTime);
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < recoilTime; i++)
        {
            gunHolder.RotateAround(currentGun.transform.position, transform.right, recoilStrength / recoilTime);
            yield return new WaitForEndOfFrame();
        }
        isFiring = false;
        isRotationLocked = true;
        gunHolder.localPosition = gunPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(gunHolder.position, gunHolder.position + gunHolder.transform.forward * 5f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(currentGun.transform.position, currentGun.transform.position + currentGun.transform.forward * 5f);
    }
}
