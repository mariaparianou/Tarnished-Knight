using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D coll;
    private float movement;
    public float speed;
    private bool isMoving;
    public int enemyHealth = 50;
    int currentHealth;
    public Transform attackPoint;
    public float attackRange;
    public float followRange;
    public float destroyDelay = 3f;
    public LayerMask playerLayer;
    private Transform playerObj;
    public float attackDistance;
    private bool hasAttacked;
    public float attackCooldown;
    private bool hasDied;
    public LevelManager levelManager;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = enemyHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        playerObj = GameObject.FindGameObjectWithTag("Player").transform;
        levelManager = LevelManager.FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasDied)
        {
            FollowPlayer();
            AttackPlayer();
        }

        attackCooldown += Time.deltaTime;
        if (attackCooldown >= 2) { hasAttacked = false; }

        movement = rb.velocity.x;
        if (movement < 0) { gameObject.transform.localScale = new Vector3(1, 1, 1); }
        else if (movement > 0) { gameObject.transform.localScale = new Vector3(-1, 1, 1); }
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        attackCooldown = 0;

        if (currentHealth <= 0)
        {
            Debug.Log("hasDied");
            animator.SetTrigger("Died");
            Invoke("DestroyObject", destroyDelay);
            hasDied = true;
            levelManager.GoblinDeathSound();

        }
        else{levelManager.GoblinHitSound();}
    }
    private void DestroyObject()
    {
        Destroy(gameObject);
    }
    private void FollowPlayer()
    {
        Collider2D[] player = Physics2D.OverlapCircleAll(transform.position, followRange, playerLayer);
        foreach (Collider2D playerCollider in player)
        {
            if (playerCollider.CompareTag("Player"))
            {
                if (player.Length > 0)
                {
                    Vector3 targetPosition = new Vector3(playerObj.position.x, transform.position.y, transform.position.z);
                    Vector3 direction = (targetPosition - transform.position).normalized;
                    rb.velocity = direction * speed;
                }
            }
        }
    }
    private void AttackPlayer()
    {

        Collider2D[] player = Physics2D.OverlapCircleAll(transform.position + new Vector3(0, 0.5f, 0), attackRange, playerLayer);
        foreach (Collider2D playerCollider in player)
        {
            if (playerCollider.CompareTag("Player"))
            {
                if (player.Length > 0)
                {
                    rb.velocity = Vector3.zero;
                    if (!hasAttacked)
                    {
                        animator.SetTrigger("Attack");
                        hasAttacked = true;
                        attackCooldown = 0;
                    }
                }
            }
        }
    }
    public void Attack()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach (Collider2D player in hitPlayer)
        {
            player.GetComponent<Player>().TakeDamage(10);
            Debug.Log("tookDamage");

        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, followRange);
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }

}
