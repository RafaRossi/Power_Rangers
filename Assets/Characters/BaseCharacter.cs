using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacter<T> : MonoBehaviour, IDamageble, IAttack where T : Characters
{
    public Action onHealthEnd;

    protected float maxHealth;
    private float _health;
    protected float Health
    {
        get => _health;
        set
        {
            _health = value;

            if(_health <= 0)
            {
                onHealthEnd();
            }
        }
    }

    protected float physicalForce;
    protected float baseArmor;
    protected float aimPrecision;
    protected float dexterity;
    protected float attackSpeed;

    protected float movementSpeed;
    protected bool canAttack = true;

    protected virtual void OnEnable()
    {
        onHealthEnd += Die;
    }

    protected virtual void OnDisable()
    {
        onHealthEnd -= Die;
    }

    public abstract void Init(T character);
    public abstract IEnumerator Attack(AttackType attackType);
    public abstract void TakeDamage(float damage);
    public abstract void Die();
}
