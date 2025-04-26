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

        // Flip for left-facing movement
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            spriteRenderer.flipX = direction.x > 0;
        }

        // // Flip sprite when walking left
        // if (direction.x < -0.1f) spriteRenderer.flipX = true;
        // else if (direction.x > 0.1f) spriteRenderer.flipX = false;

        // Move toward waypoint
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f) currentWP++;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(health);

        if (health <= 0)
        {
            Destroy(gameObject);
            CollectMoney(25);
        }
    }

    void CollectMoney(int amount)    //Adds Money when enemy is destroyed. You can change the param to any val.
    {
        if (health <= 0)
        {
            manager.coins = manager.coins + amount;
        }
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
}
