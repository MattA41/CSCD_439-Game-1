using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    public GameObject impactPrefab;

    [Header("Properties")]
    public float speed = 10f;
    public int damage = 10;
    private Transform target;
    public bool isAOE;
    public float radius;
    
    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 direction = (target.position - transform.position).normalized;
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        // Rotate bullet to face the target + offset by 90 degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        // Check distance
        if (Vector2.Distance(transform.position, target.position) < .5f)
        {   
            if (isAOE)
            {
                aoeAttack();
            }
            else
            {
                // Null check after damage, in case it destroys the enemy
                if (target != null && target.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    enemy.TakeDamage(damage);
                }
            }
            if (impactPrefab != null)
            {
                GameObject impact = Instantiate(impactPrefab, transform.position, Quaternion.identity);
                Destroy(impact, 0.5f);
            }

            // Destroy projectile after hitting
            Destroy(gameObject);
        }
    }
    

    public void aoeAttack(){
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(gameObject.transform.position, radius);

        foreach (var hit in hitColliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if(enemy != null)
            {
                Debug.Log("hit object AOE "+ hit);
                enemy.TakeDamage(damage);
            }
        }
    }
}
