using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayAmmoText : MonoBehaviour
{

    private Text text;
    private GameObject player;
    private Weapon currentWeapon;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        player = GameObject.Find("Player");
        currentWeapon = player.GetComponentInChildren<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = currentWeapon?.currentRoundsInClip + " | " + currentWeapon?.reserveAmmo;
    }
}
