using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 10.0f;
    public PlayerManager manager;
    public int health = 100;
    public GameObject[] waypoints;
    int currentWP = 0;
    public int worth = 25;

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWP >= waypoints.Length) return;

        Vector3 targetPos = waypoints[currentWP].transform.position;
        Vector3 direction = (targetPos - transform.position).normalized;

        // Set animation parameters
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0.01f)
                spriteRenderer.flipX = true;  // walking right
            else if (direction.x < -0.01f)
                spriteRenderer.flipX = false; // walking left
        }

        // Move toward waypoint
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f) currentWP++;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // Already dead, ignore further damage

        health -= damage;
        Debug.Log(health);

        if (health <= 0)
        {
            Die();
        }
    }

    void CollectMoney(int amount)    //Adds Money when enemy is destroyed. You can change the param to any val.
    {
        manager.coins += amount;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Goal" && manager.health != 0)
        {
            manager.health = --manager.health;
            Debug.Log("goal reached " + manager.health);
            Destroy(this.gameObject);
        }
    }

    private void Die()
    {
        if (isDead) return; // Protect double calls
        isDead = true;

        animator.SetTrigger("Die");
        speed = 0f;
        Debug.Log("You got: $" + manager.coins);
        CollectMoney(worth);
        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
