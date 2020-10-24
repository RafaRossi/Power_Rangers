using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerState
{
    Grounded,
    Jump,
    DoubleJump
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

    private float jumpHeight = 1.0f;
    private float jumpSpeed = -3.0f;
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

    [SerializeField] private Hero hero = null;
    [SerializeField] private Transform attackPoint = null;
    [SerializeField] private float attackRange = 5f;

    [SerializeField] private LayerMask attackMask = default;

    private List<AttackType> currentAttacks = new List<AttackType>();
    private List<Combo> currentCombos = new List<Combo>();

    [SerializeField] private PlayerState state = PlayerState.Grounded;

    private CharacterController _controller = null;
    private CharacterController Controller
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

    private void Start()
    {
        Init(hero.human);
    }

    public override void Init(Rangers character)
    {
        Morph(character);

        Health = maxHealth;
    }

    private void Update()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
        if(Input.GetButtonDown("Fire1") && canAttack)
        {
            StartCoroutine(Attack(AttackType.Light));
        }

        switch (state)
        {
            case PlayerState.Grounded:
                if (Input.GetButtonDown("Jump"))
                {
                    Jump();
                    state = PlayerState.Jump;
                }
                break;

            case PlayerState.Jump:
                if (Input.GetButtonDown("Jump"))
                {
                    Jump();
                    state = PlayerState.DoubleJump;
                }
                break;
        }

        if(Input.GetKeyDown(KeyCode.Return))
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
        }      
    }

    private void LateUpdate()
    {
        Move();
    }

    private void Morph(Rangers character)
    {
        maxHealth = character.maxHealth;

        physicalForce = character.physicalForce;
        baseArmor = character.baseArmor;
        aimPrecision = character.aimPrecision;
        dexterity = character.dexterity;
        movementSpeed = character.movementSpeed;
        attackSpeed = character.attackSpeed;

        rangerColor = character.rangerColor;

        if(CurrentForm == RangerForm.Human)
        {
            CurrentForm = RangerForm.Ranger;
        }
        else
        {
            CurrentForm = RangerForm.Human;
        }

        UpdateCombos(character);
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
        if (Controller.isGrounded && playerVelocity < 0)
        {
            playerVelocity = 0f;
            state = PlayerState.Grounded;
        }

        Controller.Move(moveDirection.normalized * Time.deltaTime * movementSpeed);

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
    }

    public override IEnumerator Attack(AttackType attackType)
    {
        currentAttacks.Add(attackType);
        CheckCombos();

        canAttack = false;

        Collider[] colliders = Physics.OverlapSphere(attackPoint.position, attackRange, attackMask);

        foreach(Collider collider in colliders)
        {
            if (collider.TryGetComponent(out IDamageble hitObject))
            {
                hitObject.TakeDamage(physicalForce);
            }
        }

        yield return new WaitForSeconds(attackSpeed);

        canAttack = true;
    }

    public override void TakeDamage(float damage)
    {
        Health -= damage - (baseArmor / (baseArmor + 100));
    }

    public override void Die()
    {

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
