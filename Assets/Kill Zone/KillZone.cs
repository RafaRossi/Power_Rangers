using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player;
        EnemyController enemy;
        if(other.TryGetComponent(out player))
        {
            player.Die();
        }else if(other.TryGetComponent(out enemy))
        {
            enemy.Die();
        }
    }
}
