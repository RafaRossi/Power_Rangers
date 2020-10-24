using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : ScriptableObject
{
    [Header("Gameplay Attributes")]
    public float maxHealth;

    public float physicalForce;
    public float baseArmor;
    public float aimPrecision;
    public float dexterity;
    public float movementSpeed;
    public float attackSpeed = 0.5f;

    public Texture3D texture;
    public GameObject characterModel;

    public List<Combo> availableCombos = new List<Combo>();

    [Header("Character Information")]
    public string characterFullName;

    public string characterBackstory;

    public float characterHeight;
    public float characterWidth;
    public float characterWeight;
}
