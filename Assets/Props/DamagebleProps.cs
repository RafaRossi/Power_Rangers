using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagebleProps : MonoBehaviour, IDamageble
{
    [SerializeField] private float resistance = 100;

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void TakeDamage(float damage)
    {
        resistance -= damage;

        print(resistance);

        if (resistance <= 0)
        {
            resistance = 0;

            Die();
        }
    }
}
