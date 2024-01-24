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
}
