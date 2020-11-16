using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerState
{
    Idle,
    Grounded,
    Jump,
    DoubleJump,
    Attacking
}

public enum RangerForm
{
    Ranger, Human
}

public class PlayerCharacter : BaseCharacter<Rangers>
{
    private float playerVelocity;

    private readonly float maxEnergy = 100;
    private float _energy = 50;
    protected float Energy
    {
        get => _energy;
        set
        {
            _energy = value;
            
            if(_energy >= maxEnergy)
            {
                _energy = maxEnergy;
            }

            if(_energy <= 0)
            {
                onEnergyEnd();
            }
        }
    }
    private float energyGainRate = 2f;
    public Action onEnergyEnd;

    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float jumpSpeed = -3.0f;
    private Color rangerColor;

    private RangerForm currentForm = RangerForm.Human;
    public RangerForm CurrentForm
    {
        get => currentForm;
        set
        {
            if(value != currentForm)
            {
                currentForm = value;

                if(currentForm == RangerForm.Human)
                {
                    StartCoroutine(RegainEnergy());
                }
            }
        }
    }

    private Vector3 moveDirection = Vector3.zero;

    [SerializeField] private Transform attackPoint = null;
    [SerializeField] private float attackRange = 3f;

    [SerializeField] private LayerMask attackMask = default;

    private List<AttackType> currentAttacks = new List<AttackType>();
    private List<Combo> currentCombos = new List<Combo>();

    [SerializeField] private PlayerState _state = PlayerState.Grounded;
    private PlayerState State
    {
        get
        {
            return _state;
        }
        set
        {
            _state = value;
        }
    }

    private CharacterController _controller = null;
    protected CharacterController Controller
    {
        get
        {
            if (!_controller)
            {
                _controller = GetComponent<CharacterController>();
            }
            return _controller;
        }
    }

    public Action OnPlayerDie = delegate { };

    protected override float Health 
    { 
        get => base.Health;
        set
        {
            base.Health = value;

            GameManager.Instance.UpdateHUD(Health);
        }
    }

    private bool isGrounded = true;

    public override void Init(Rangers character)
    {
        Morph(character);

        Health = maxHealth;
    }

    private void Update()
    {
        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        switch (State)
        {
            case PlayerState.Grounded:

                if (Input.GetButtonDown("Fire1") && canAttack)
                {
                    StartCoroutine(Attack(AttackType.Light));
                }
                if (Input.GetButtonDown("Fire2") && canAttack)
                {
                    StartCoroutine(Attack(AttackType.Heavy));
                }

                if (Input.GetButtonDown("Jump"))
                {
                    Jump();
                    State = PlayerState.Jump;
                }
                break;

            case PlayerState.Jump:
                if (Input.GetButtonDown("Jump"))
                {
                    Jump();
                    State = PlayerState.DoubleJump;
                }
                break;
            case PlayerState.Attacking:
                if (Input.GetButtonDown("Fire1") && canAttack)
                {
                    StartCoroutine(Attack(AttackType.Light));
                }
                if (Input.GetButtonDown("Fire2") && canAttack)
                {
                    StartCoroutine(Attack(AttackType.Heavy));
                }
                break;
        }

        /*if(Input.GetKeyDown(KeyCode.Return))
        {
            switch (currentForm)
            {
                case RangerForm.Human:
                    if(Energy >= maxEnergy)
                        Morph(hero.ranger);
                    break;

                case RangerForm.Ranger:
                    Morph(hero.human);
                    break;
            }
        }*/      
    }

    private void LateUpdate()
    {
        if(isGrounded != CheckGrounded())
        {
            Animator.SetBool("Fall", isGrounded);
        }
        isGrounded = CheckGrounded();

        Move();
    }

    private bool CheckGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.2f, 1 << LayerMask.NameToLayer("Ground"));
    }

    private void Morph(Rangers character)
    {
        maxHealth = character.maxHealth;

        physicalForce = character.physicalForce;
        baseArmor = character.baseArmor;
        aimPrecision = character.aimPrecision;
        dexterity = character.dexterity;
        movementSpeed = character.movementSpeed;
        attackSpeed = 0.1f;

        rangerColor = character.rangerColor;

        Material[] _materials = GetComponentInChildren<SkinnedMeshRenderer>().materials;
        _materials[0] = character.characterMaterial;
        GetComponentInChildren<SkinnedMeshRenderer>().materials = _materials;

        if (CurrentForm == RangerForm.Human)
        {
            CurrentForm = RangerForm.Ranger;
        }
        else
        {
            CurrentForm = RangerForm.Human;
        }
    }
    private IEnumerator RegainEnergy()
    {
        while (Energy < maxEnergy && CurrentForm == RangerForm.Human)
        {
            yield return new WaitForSeconds(energyGainRate);

            Energy += energyGainRate;
        }
    }

    private void Move()
    {
        float speed = movementSpeed;

        if(State == PlayerState.Attacking)
        {
            speed /= 5;
        }else
            if (isGrounded && playerVelocity <= 0)
            {
                playerVelocity = 0f;
                State = PlayerState.Grounded;
            }

        Controller.Move(moveDirection.normalized * Time.deltaTime * speed);
        Animator.SetFloat("Speed", moveDirection.normalized.magnitude);

        if (moveDirection != Vector3.zero)
        {
            gameObject.transform.forward = moveDirection;
        }

        playerVelocity += Physics.gravity.y * Time.deltaTime;
        Controller.Move(new Vector3(0, playerVelocity, 0) * Time.deltaTime);
    }

    private void Jump()
    {
        float value = Mathf.Sqrt(jumpHeight * jumpSpeed * Physics.gravity.y);
        playerVelocity = value;

        Animator.SetTrigger("Jump");
    }

    public override IEnumerator Attack(AttackType attackType)
    {
        State = PlayerState.Attacking;
        currentAttacks.Add(attackType);

        Animator.SetTrigger(attackType.ToString());

        canAttack = false;
        
        yield return new WaitForSeconds(attackSpeed);

        canAttack = true;
    }

    public void Heal(float health)
    {
        Health += health;
    }

    public void VerifyAttack()
    {
        Collider[] colliders = Physics.OverlapSphere(attackPoint.position, attackRange, attackMask);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out IDamageble hitObject))
            {
                hitObject.TakeDamage(physicalForce);
                AudioManager.Instance.PlayPunch();
            }
        }
    }

    public void ResetAttack()
    {
        State = PlayerState.Idle;
    }

    public override void Die()
    {
        enabled = false;

        Animator.enabled = false;

        Controller.enabled = false;

        GameManager.Instance.OnPlayerDie();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private bool CheckCombos()
    {
        bool isComboAvailable = false;

        foreach(Combo combo in currentCombos)
        {
            if(combo.attacks == currentAttacks)
            {
                isComboAvailable = true;
                break;
            }
        }
        return isComboAvailable;
    }

    private void UpdateCombos(Rangers ranger)
    {
        currentCombos.Clear();

        foreach(Combo combo in ranger.availableCombos)
        {
            if(combo.comboType == ComboType.Unlocked)
            {
                currentCombos.Add(combo);
            }
        }
    }

    private void UnlockCombo(Combo combo)
    {
        combo.comboType = ComboType.Unlocked;
    }
}
