using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PerkPlayStyle
{
    Combat,
    Interaction,
    Stealth,
    AreaOfEffect,
    Debuff,
    SingleTargetDamage
}

public enum EngagementRange
{
    Close,
    Far
}

[CreateAssetMenu(fileName = "PerkDataNode", menuName = "PerkEffects/PerkNode")]
public class PerkDataNode : ScriptableObject
{
    [field: SerializeField] public string perkID { get; private set; }
    public string perkName;
    public string description;
    public int cost; 
    public PerkEffect perkEffect;
    public List<PerkDataNode> prerequisites;
    public PerkPlayStyle playStyle;
    public EngagementRange engagementRange;
    public List<PerkDataNode> cantBeActiveWith;  // List of PerkDataNode that cannot be active simultaneously


    private void OnValidate()
    {
#if     UNITY_EDITOR
        perkID = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
