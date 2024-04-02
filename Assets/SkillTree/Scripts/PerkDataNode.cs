using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "SlidePerkEffect", menuName = "PerkEffects/PerkNode")]
public class PerkDataNode : ScriptableObject
{
    [field: SerializeField] public string perkID { get; private set; }
    public string perkName;
    public string description;
    public int cost; 
    public PerkEffect perkEffect;
    public List<PerkDataNode> prerequisites;


    private void OnValidate()
    {
#if     UNITY_EDITOR
        perkID = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
