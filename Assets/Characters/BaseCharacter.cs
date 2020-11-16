using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacter<T> : MonoBehaviour, IDamageble, IAttack where T : Characters
{
    public Action onHealthEnd;

    protected float maxHealth;
    private float _health;
    protected virtual float Health
    {
        get => _health;
        set
        {
            _health = value;

            if(_health > maxHealth)
            {
                _health = maxHealth;
            }

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

    private Animator _animator = null;
    protected Animator Animator
    {
        get
        {
            if (!_animator)
            {
                _animator = GetComponent<Animator>();
            }
            return _animator;
        }
    }

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

    public virtual void TakeDamage(float damage)
    {
        Health -= damage - (baseArmor / (baseArmor + 100));
        Animator.SetTrigger("Damage");
    }

    public abstract void Die();
}
