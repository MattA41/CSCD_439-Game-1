using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class TowerScript : MonoBehaviour
{

    public int damage = 10;
    public float attackCooldown = 1f;
    private float lastAttackTime = 0f;

    private List<GameObject> enemiesInRange = new List<GameObject>();
    private LineRenderer lineRenderer;
    
    public GameObject bulletPrefab;
    public Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = true;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesInRange.Count > 0 && Time.time >= lastAttackTime + attackCooldown)
        {
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
