using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D coll;
    private float movement;
    public float speed;
    private bool isMoving;
    public float jumpForce;
    private bool isGrounded;
    public Transform attackPoint;
    public float attackRange;
    public LayerMask enemyLayer;
    public int playerHealth = 100;
    public int currentHealth;
    public LevelManager levelManager;
    public GameObject deathUi;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();

        currentHealth = playerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        movement = Input.GetAxisRaw("Horizontal") * speed;

        rb.velocity = new Vector2(movement, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(new Vector2(rb.velocity.x, jumpForce * 100));

        }

        if (rb.velocity.magnitude > 0.1f)
        {
            isMoving = true;
            ;
        }
        else
        {
            isMoving = false;
        }

        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isGrounded", isGrounded);
        if (movement < 0) { gameObject.transform.localScale = new Vector3(-1, 1, 1); }
        else if (movement > 0) { gameObject.transform.localScale = new Vector3(1, 1, 1); }

        if (Input.GetButtonDown("Fire1") && isGrounded)
        {
            Attack();
            levelManager.SwordSound();
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;

        }

    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
    private void Attack()
    {
        animator.SetTrigger("isAttacking");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("we hit enemy");
            enemy.GetComponent<Enemy>().TakeDamage(25);
        }
    }
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Debug.Log("hasDied");
            animator.SetTrigger("Died");
            YouDied();

        }
    }

    public void YouDied()
    {
               
        deathUi.SetActive(true);
        Time.timeScale = 0;

    }
}
