using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageble
{
    void TakeDamage(float damage);
    void Die();
}

public interface IAttack
{
    IEnumerator Attack(AttackType attackType);
}
