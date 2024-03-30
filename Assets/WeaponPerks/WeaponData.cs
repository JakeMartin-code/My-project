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
    
    public string weaponName;
    public float baseDamage;
    public float baseReloadTime;
    public List<WeaponPerk> possiblePerks; // List of possible perks for this weapon
    public List<float> perkValues; // Additional values for each perk
    public List<float> perkDurations;
    public float range;
    public int basereserveAmmo;
    public int basemaxAmmoInMag;
    public bool isFullAuto;
    public float fireRate;
}
