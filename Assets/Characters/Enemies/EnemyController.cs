using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class EnemyController : BaseCharacter<Enemy>
{
    [SerializeField] private Enemy enemy = null;
    [SerializeField] private float minAttackDistance = 0.1f;
    [SerializeField] private Transform attackPoint = null;
    [SerializeField] private float attackRange = 1;
    [SerializeField] private LayerMask attackMask = default;

    [SerializeField] private NavMeshAgent _agent = null;
    private NavMeshAgent Agent
    {
        get
        {
            if(!_agent)
            {
                _agent = GetComponent<NavMeshAgent>();
            }
            return _agent;
        }
    }

    public PlayerCharacter player;

    public Action<Enemy> OnEnemyDie = delegate { };

    private void Start()
    {
        Init(enemy);
        StartCoroutine(Act());
    }

    private void Update()
    {
        Animator.SetFloat("Speed", Agent.velocity.normalized.magnitude);
    }

    IEnumerator Act()
    {
        float time = UnityEngine.Random.Range(7f, 15f);
        yield return new WaitForSeconds(time);

        StartCoroutine(Attack(AttackType.Light));

        StartCoroutine(Act());
    }

    public override IEnumerator Attack(AttackType attackType)
    {
        while(Vector3.Distance(player.transform.position, transform.position) > minAttackDistance)
        { 
            if(Agent)Agent.SetDestination(player.transform.position);
            yield return null;
        }

        Animator.SetTrigger("Attack");
    }

    public void VerifyAttack()
    {
        Collider[] colliders = Physics.OverlapSphere(attackPoint.position, attackRange, attackMask);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out IDamageble hitObject))
            {
                hitObject.TakeDamage(physicalForce);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public override void Die()
    {
        Animator.enabled = false;

        GetComponent<CapsuleCollider>().enabled = false;
        OnEnemyDie(enemy);

        Instantiate(GameManager.Instance.healthPrefab, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        Destroy(gameObject, 2f);
    }

    public override void Init(Enemy character)
    {
        enemy = character;
        maxHealth = character.maxHealth;

        physicalForce = character.physicalForce;
        baseArmor = character.baseArmor;

        Health = character.maxHealth;

        Material[] _materials = GetComponentInChildren<SkinnedMeshRenderer>().materials;
        _materials[0] = character.characterMaterial;
        GetComponentInChildren<SkinnedMeshRenderer>().materials = _materials;
    }
}
