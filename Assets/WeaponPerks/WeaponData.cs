using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Primary,
    Secondary,
    Heavy
}

public enum WeaponPlaystyle
{
    stealty,
    singleTarget,
    balanced,
    areaOfEffect
}

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Weapon")]
public class WeaponData : ScriptableObject
{
    
    public string weaponName;
    public float baseDamage;
    public float baseReloadTime;
    public List<WeaponPerk> possiblePerks; // List of possible perks for this weapon
    public float range;
    public int basereserveAmmo;
    public int basemaxAmmoInMag;
    public bool isFullAuto;
    public float fireRate;
    public WeaponType weaponType;
    public WeaponPlaystyle weaponPlaystyle;
}
