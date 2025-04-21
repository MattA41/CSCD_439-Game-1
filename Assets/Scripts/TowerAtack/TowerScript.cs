using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[ExecuteAlways]
public class TowerScript : MonoBehaviour
{

    public int damage = 10;
    public float attackCooldown = 1f;

    public float attackRange = 4f;
    private float lastAttackTime = 0f;

    private List<GameObject> enemiesInRange = new List<GameObject>();

    public GameObject bulletPrefab;

    private void Awake()
    {
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        // Sync collider to attack range
        collider.radius = attackRange;

        // Sync range visual scale
        Transform rangeVisual = transform.Find("RangeVisual");
        if (rangeVisual != null)
        {
            float spriteRadius = 0.5f; // assuming the circle sprite is 1 unit wide
            float scaleFactor = attackRange / spriteRadius;
            rangeVisual.localScale = Vector3.one * scaleFactor;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesInRange.Count > 0 && Time.time >= lastAttackTime + attackCooldown)
        {
            Debug.Log(enemiesInRange.Count);
            Attack(enemiesInRange[0]);
            lastAttackTime = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger entered: " + other.gameObject.name);
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.gameObject);
            Debug.Log("Enemy entered range: " + other.gameObject.name);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
            Debug.Log("Enemy exited range: " + other.gameObject.name);
        }
    }

    private void Attack(GameObject enemy)
    {
        if (enemy != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.SetTarget(enemy.transform);
            bulletScript.damage = damage;

        }
    }
}
