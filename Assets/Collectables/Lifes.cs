using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifes : MonoBehaviour
{
    public float amount = 5f;

    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter playerCharacter = null;

        if(other.TryGetComponent(out playerCharacter))
        {
            playerCharacter.Heal(amount);

            Destroy(gameObject);
        }
    }
}
