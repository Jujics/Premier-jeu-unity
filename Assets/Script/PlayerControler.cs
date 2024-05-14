using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControler : MonoBehaviour
{
    [Header("Component")]
    Rigidbody2D rb;
    Animator anim;

    [Header("Stat")]
    [SerializeField]
    float moveSpeed;

    [Header("Attack")]
    private float attackTime;
    [SerializeField] float timeBetweenAttack;
    private bool CanMove;
    [SerializeField] Transform checkEnemy;
    public LayerMask whatIsEnemy;
    public int maxHealth = 20;
    public int currentHealth;
    public HealthBar healthBar;
    public int damage = 1;




    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        CanMove = false;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time >= attackTime)
            {
                rb.velocity = Vector2.zero;
                anim.SetTrigger("attack");
                StartCoroutine(Delay());

                IEnumerator Delay()
                {
                    CanMove = false;
                    yield return new WaitForSeconds(.5f);
                    CanMove = true;
                }
                attackTime = Time.time + timeBetweenAttack;
            }
        }
    }


    private void FixedUpdate()
    {
        if(CanMove)
        Move();
    }





    void Move()
    {
        if(Input.GetAxis("Horizontal") > 0.1)
            {
                anim.SetFloat("lasth", 1);
                anim.SetFloat("lastv", 0);
            }
        else if(Input.GetAxis("Horizontal") < -0.1)
        {
          anim.SetFloat("lasth", -1);
          anim.SetFloat("lastv", 0);
        }
        if(Input.GetAxis("Verticale") > 0.1)
            {
                anim.SetFloat("lasth", 0);
                anim.SetFloat("lastv", 1);
            }
        else if(Input.GetAxis("Verticale") < -0.1)
        {
          anim.SetFloat("lasth", 0);
          anim.SetFloat("lastv", -1);
        }

        if(Input.GetAxis("Horizontal") > 0.1)
        {
            checkEnemy.position = new Vector3(transform.position.x + 1, transform.position.y,0);
        }
        else if(Input.GetAxis("Horizontal") < -0.1)
        {
            checkEnemy.position = new Vector3(transform.position.x - 1, transform.position.y,0);
        }
        if (Input.GetAxis("Vertical") > 0.1)
        {
            checkEnemy.position = new Vector3(transform.position.x, transform.position.y + 1,0);
        }
        else if (Input.GetAxis("Vertical") < -0.1)
        {
            checkEnemy.position = new Vector3(transform.position.x, transform.position.y - 1,0);
        }


        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(x, y) * moveSpeed * Time.fixedDeltaTime;

        rb.velocity.Normalize();

        if(x != 0 || y != 0)
        {
            anim.SetFloat("Horizontal", x);
            anim.SetFloat("Verticale", y);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    public void OnAttack()
    {
      Collider2D[] enemy = Physics2D.OverlapCircleAll(checkEnemy.position, 0.5f, whatIsEnemy);
      foreach (var enemy_ in enemy)
      {
        enemy_.GetComponent<Enemy>().TakeDamage(damage);
      }
    }



}
