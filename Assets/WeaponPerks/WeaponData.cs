using System.Collections.Generic;
using UnityEngine;

public enum WeaponPerk
{
    DamageBoostAfterKill,
    StealthShot,
    Freeze
}

public enum WeaponType
{
    Tactical,
    runAndGun,
    magic
}


[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Weapon")]
public class WeaponData : ScriptableObject
{
    /*
    public string weaponName;
    public int baseDamage;
    public float baseReloadTime;
    public WeaponPerk perkType;
    public WeaponType WeaponType;
    public float perkValue; // Additional value for specific perks (like duration of damage boost)
    public float range;
    public int basereserveAmmo;
    //public int basecurrentAmmoInMag;
    public int basemaxAmmoInMag;
    // Add more properties as needed
    */

    public string weaponName;
    public float baseDamage;
    public float baseReloadTime;
    public List<WeaponPerk> possiblePerks; // List of possible perks for this weapon
    public List<float> perkValues; // Additional values for each perk
    public List<float> perkDurations;
    public float range;
    public int basereserveAmmo;
    public int basemaxAmmoInMag;
}
