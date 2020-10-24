using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Combo", menuName = "Combo")]
public class Combo : ScriptableObject
{
    public string comboName;
    public string comboDescription;
    public ComboType comboType = ComboType.Locked;

    public List<AttackType> attacks = new List<AttackType>();
}
