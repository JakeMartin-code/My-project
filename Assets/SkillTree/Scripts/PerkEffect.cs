using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PerkEffect : ScriptableObject
{
    
  public abstract void ApplyEffect(GameObject player);
  public abstract void RemoveEffect(GameObject player);



}
