using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEBullet : MonoBehaviour
{
    [Header("References")]
    public GameObject explosionPrefab;

    [Header("Properties")]
    public float speed = 7f; // Slightly slower than single bullet if you want
    public int damage = 8;   // Maybe slightly lower damage since it hits multiple enemies
    public float explosionRadius = 2.5f;
    public float lifetime = 3f; // Safety timeout

    private Vector2 moveDirection;
    private Vector2 targetPosition;
    private bool hasTargetPosition = false;

    private void Start()
    {
        Destroy(gameObject, lifetime); // Auto destroy if misses
    }

    public void SetTargetPosition(Vector2 position)
    {
        targetPosition = position;
        moveDirection = (targetPosition - (Vector2)transform.position).normalized;
        hasTargetPosition = true;
    }

    void Update()
    {
        if (!hasTargetPosition) return;

        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        // Optional: rotate toward movement
        if (moveDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }

        // Check if reached target position approximately
        if (Vector2.Distance(transform.position, targetPosition) < 0.2f)
        {
            Explode();
        }
    }

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;

        // Rotate sprite to face moving direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy") && hit.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(damage);
            }
        }

        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 0.5f);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Debug only: visualize explosion radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
