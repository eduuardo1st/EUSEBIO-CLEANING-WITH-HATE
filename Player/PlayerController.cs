using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Configura��o de Vida")]
    private bool isDead = false;
    [SerializeField] private int vidaMaxima = 10;
    [SerializeField] private float rollSpeed = 10f;
    [SerializeField] private float rollInvulnerabilityDuration = 0.5f;
    private float normalSpeed;
    public static int vidaAtual;
    private bool ataqueJaExecutado = false;
    private bool _isRolling;
    private float tempoParado = 0f;

    [Header("Configuracao de Ataque")]
    [SerializeField] private UnityEngine.UI.Image CooldownBar;
    [SerializeField] private float attackCooldown = 0.6f;
    private float nextAttackTime = 0f;
    public float moveSpeed = 1f;
    public SwordAttack swordAttack;
    Vector2 mouseDirection;
    Vector2 lastMovementDirection = Vector2.down;
    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;
    bool canMove = true;
    private bool podeReceberInput = true;
    private bool isInvulnerable = false;

    [Header("Configura��o de Estamina")]
    [SerializeField] private int maxRolls = 3;
    [SerializeField] private float rollRechargeTime = 7f;
    private int currentRolls;
    private float rollRechargeTimer = 0f;
    [SerializeField] private UnityEngine.UI.Image staminaBar;

    [Header("UI da Barra de Vida")]
    [SerializeField] private UnityEngine.UI.Image healthBar;
    public bool isRolling
    {
        get => _isRolling; set => _isRolling = value;
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        normalSpeed = moveSpeed;

        AtualizarVidaUI();
        currentRolls = maxRolls;
        UpdateStaminaUI();
    }

    void Update()
    {
        if (!podeReceberInput) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene("Fase 4 - BOSS");
        }

        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f;

        Vector3 direction = mouseWorldPos - transform.position;
        direction.z = 0f;

        mouseDirection = direction.normalized;

        Debug.DrawLine(transform.position, mouseWorldPos, Color.red);


        HandleRollRecharge();
        UpdateAttackCooldownUI();
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            if (movementInput.sqrMagnitude > 0.01f)
            {
                if (isRolling)
                {
                    Vector2 rollDirection = movementInput.sqrMagnitude > 0.01f ? movementInput : lastMovementDirection;

                    animator.SetFloat("AxisX", movementInput.x);
                    animator.SetFloat("AxisY", movementInput.y);

                    rb.MovePosition(rb.position + rollDirection * moveSpeed * Time.fixedDeltaTime);
                }

                else
                {
                    lastMovementDirection = movementInput.normalized;
                    rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
                    animator.SetFloat("AxisX", movementInput.x);
                    animator.SetFloat("AxisY", movementInput.y);
                    animator.SetInteger("Movimento", 1);

                    tempoParado = 0f;
                }

            }
            else
            {
                tempoParado += Time.fixedDeltaTime;

                if (tempoParado >= 6f)
                {
                    animator.SetInteger("Movimento", 2);
                }
                else
                {
                    animator.SetInteger("Movimento", 0);
                }
            }
        }
    }

    public void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    public void OnFire(InputValue value)
    {
        if (!podeReceberInput) return;

        if (Time.time < nextAttackTime) return;

        nextAttackTime = Time.time + attackCooldown;

        animator.SetTrigger("swordAttack");

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseDirection = (mouseWorldPos - (Vector2)transform.position).normalized;

        animator.SetFloat("AttackX", mouseDirection.x);
        animator.SetFloat("AttackY", mouseDirection.y);

        if (CooldownBar != null) CooldownBar.gameObject.SetActive(true);
    }

    public void SetPodeReceberInput(bool valor)
    {
        podeReceberInput = valor;
    }

    public void SwordAttack()
    {
        if (ataqueJaExecutado) return;

        ataqueJaExecutado = true;

        Vector2 direction = mouseDirection;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        angle = (angle + 360f) % 360f;

        if (angle >= 45f && angle < 135f)
        {
            swordAttack.AttackUp();
        }
        else if (angle >= 135f && angle < 225f)
        {
            swordAttack.AttackLeft();
        }
        else if (angle >= 225f && angle < 315f)
        {
            swordAttack.AttackDown();
        }
        else
        {
            swordAttack.AttackRight();
        }
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }

    public void OnHit(float dano)
    {
        if (!isInvulnerable && !isDead) 
        {
            animator.SetTrigger("hit");

            vidaAtual -= Mathf.RoundToInt(dano);
            vidaAtual = Mathf.Clamp(vidaAtual, 0, vidaMaxima);

            AtualizarVidaUI();

            if (vidaAtual <= 0 && !isDead) 
            {
                isDead = true; 
                XpManager.ResetXpToCheckpoint();
                animator.SetTrigger("death");
                LockMovement();

                SetPodeReceberInput(false);

                StartCoroutine(ReiniciarCenaAposMorte(2f));
            }
        }
    }
    private IEnumerator ReiniciarCenaAposMorte(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetarVida();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void AtualizarVidaUI()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)vidaAtual / vidaMaxima;
        }
        else
        {
            Debug.LogWarning("A Image da barra de vida n�o foi atribu�da no PlayerController!");
        }
    }

    public void ResetarAtaque()
    {
        ataqueJaExecutado = false;
        UnlockMovement();
    }

    public void ResetarVida()
    {
        vidaAtual = vidaMaxima;
        isDead = false;
        AtualizarVidaUI();
    }

    public void OnRoll(InputValue value)
    {
        if (!podeReceberInput || _isRolling || currentRolls <= 0) return;

        currentRolls--;
        UpdateStaminaUI();

        _isRolling = true;
        moveSpeed = rollSpeed;
        animator.SetTrigger("isRoll");
        StartCoroutine(RollInvulnerabilityRoutine());
    }

    private IEnumerator RollInvulnerabilityRoutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(rollInvulnerabilityDuration);
        isInvulnerable = false;
        _isRolling = false;
        moveSpeed = normalSpeed;
    }

    public void StartRollInvulnerability()
    {
        isInvulnerable = true;
    }

    public void EndRollInvulnerability()
    {
        isInvulnerable = false;
        _isRolling = false;
        moveSpeed = normalSpeed;
    }

    private void HandleRollRecharge()
    {
        if (currentRolls < maxRolls)
        {
            rollRechargeTimer += Time.deltaTime;
            if (rollRechargeTimer >= rollRechargeTime)
            {
                currentRolls++;
                currentRolls = Mathf.Clamp(currentRolls, 0, maxRolls);
                rollRechargeTimer = 0f;
                UpdateStaminaUI();
            }
        }
    }

    private void UpdateStaminaUI()
    {
        if (staminaBar != null)
        {
            staminaBar.fillAmount = (float)currentRolls / maxRolls;
        }
        else
        {
            Debug.LogWarning("A Image da barra de estamina n�o foi atribu�da no PlayerController!");
        }
    }

    private void UpdateAttackCooldownUI()
    {
        if (CooldownBar != null)
        {
            float restante = nextAttackTime - Time.time;

            if (restante > 0)
            {
                if (!CooldownBar.gameObject.activeSelf)
                    CooldownBar.gameObject.SetActive(true);

                CooldownBar.fillAmount = restante / attackCooldown;
            }
            else
            {
                if (CooldownBar.gameObject.activeSelf)
                    CooldownBar.gameObject.SetActive(false);
            }
        }
    }
}