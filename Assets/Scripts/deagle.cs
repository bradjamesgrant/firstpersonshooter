using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deagle : Weapon
{
    private void Start()
    {

        maxRoundsInClip = 6;
        currentRoundsInClip = maxRoundsInClip;
        startReserveAmmo = 30;
        reserveAmmo = startReserveAmmo;
    }
}
