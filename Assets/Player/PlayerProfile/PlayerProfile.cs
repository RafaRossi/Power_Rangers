using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Profile", menuName = "Player Profile")]
public class PlayerProfile : ScriptableObject
{
    [SerializeField] public Hero currentHero = null;
    //[SerializeField] private List<Combo> currentCombos = new List<Combo>();
}
