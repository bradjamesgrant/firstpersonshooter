using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public int maxRoundsInClip;
    public int currentRoundsInClip;
    public int startReserveAmmo;
    public int reserveAmmo;
}
