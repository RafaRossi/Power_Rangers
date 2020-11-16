using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class AttackAnimatorController : StateMachineBehaviour
{
    private PlayerCharacter player = null;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if(!player)
       {
            player = animator.gameObject.GetComponent<PlayerCharacter>();
       }
        player.ResetAttack();
    }
}
