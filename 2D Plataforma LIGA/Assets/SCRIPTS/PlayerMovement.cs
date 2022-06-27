using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimentação")]
    private float hor;
    [SerializeField] private float speed;

    [Header("Pulo")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float raycastGround;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;

    //Booleans
    private bool isDead = false;

    [Header("Knockback")]
    [SerializeField] private float knockbackForce;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer visuals;
    private bool isFacingRight = true;

    [Header("Joystick")]
    [SerializeField] private FixedJoystick joystick;

    [Header("Componentes")]
    [SerializeField] private BoxCollider2D box2D;
    private PlayerHealth health;
    private Animator anim;
    private Rigidbody2D rb;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        box2D = GetComponent<BoxCollider2D>();
        health = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        //Pega o valor da horizontal do joystick e armazena na variavel hor caso não esteja morto
        if (!isDead)
            hor = joystick.Horizontal;
        else
            hor = 0;

        //Como é feito com os inputs do teclado
        //hor = Input.GetAxisRaw("Horizontal");

        isGrounded = CheckGround();

        if (hor > 0)
        {
            isFacingRight = true;
        }
        else if (hor < 0)
        {
            isFacingRight = false;
        }

        Flip();
        AnimatorControl();
        NoFriction();
    }

    private void FixedUpdate()
    {
        //Script que mexe com 2D Physics no FixedUpdate para não ter discrepância em caso de mudança de FPS

        //Responsável pela movimentação, move baseado no valor hor que é armazenado do joystick
        rb.velocity += new Vector2(hor * speed * Time.fixedDeltaTime, 0);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //Ao colidir com a tag Obstacle leva um pequeno empurrão
        if (other.collider.CompareTag("Obstacle"))
        {
            Knockback(other.gameObject);
        }
    }

    private bool CheckGround()
    {
        //Cria um raycast em formato de quadrado que vai do meio do Box Collider do player até o fim + o valor da variável raycastGround
        RaycastHit2D ray = Physics2D.BoxCast(box2D.bounds.center, box2D.bounds.size, 0f, Vector2.down, raycastGround, groundLayer);
        //Caso detecte algum collider da tag Ground então retorna true
        return ray.collider != null;
    }

    private void NoFriction()
    {
        //Função para dar um pouco mais de controle na movimentação do player, removendo totalmente a deslizada que
        //ele pode dar ao soltar os controles, então ao soltar ele faz o rigidbody ficar completamente em 0, parando instantaneamente
        if (rb.velocity.x != 0 && hor == 0 && isGrounded)
        {
            rb.velocity = Vector3.zero;
        }
    }

    //Os tipos de pulo que são chamados nos botões da UI
    public void Jump()
    {
        if (isGrounded && !isDead)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void LowJump()
    {
        if (isGrounded && !isDead)
        {
            rb.AddForce(Vector2.up * (jumpForce / 1.5f), ForceMode2D.Impulse);
        }
    }

    //Controla os parâmetros no Animator do player
    private void AnimatorControl()
    {
        anim.SetFloat("Horizontal", Mathf.Abs(hor));

        anim.SetBool("isJumping", !isGrounded);

        anim.SetBool("isDead", isDead);

        if (!isGrounded && !isDead)
        {
            if (rb.velocity.y < 0)
            {
                anim.SetBool("isFalling", true);
            }
            else
            {
                anim.SetBool("isFalling", false);
            }
        }
        else
        {
            anim.SetBool("isFalling", false);
        }
    }

    private void Flip()
    {
        if (isFacingRight)
        {
            visuals.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            visuals.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void Knockback(GameObject other)
    {
        //Faz o player levar dano
        health.Damage();

        //Calcula a direção do empurrão baseado no objeto que foi colidido, sempre sendo um vetor mais na diagonal
        if (other.transform.position.x < transform.position.x)
        {
            rb.velocity = new Vector2(+knockbackForce, knockbackForce);
        }
        else
        {
            rb.velocity = new Vector2(-knockbackForce, knockbackForce);
        }
    }

    public void SetDead()
    {
        isDead = true;
    }

    //Desenha os Gizmos de até onde o raycast que detecta o chão vai
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(box2D.bounds.center + new Vector3(box2D.bounds.extents.x, 0), Vector2.down * (box2D.bounds.extents.y + raycastGround));
        Gizmos.DrawRay(box2D.bounds.center - new Vector3(box2D.bounds.extents.x, 0), Vector2.down * (box2D.bounds.extents.y + raycastGround));
    }
}
