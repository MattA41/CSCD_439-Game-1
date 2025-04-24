using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

//[ExecuteAlways]
public class TowerScript : MonoBehaviour
{

    
    [Header("Properties")]
    public int damage = 10;
    public float attackCooldown = 1f;
    public float attackRange = 4f;
    

    [Header("References")]
    public Transform firePoint;
    public GameObject bulletPrefab;

    private List<GameObject> enemiesInRange = new List<GameObject>();
    private float lastAttackTime = 0f;
    private Animator weaponAnimator;

   private void Awake()
{
    CircleCollider2D collider = GetComponent<CircleCollider2D>();
    if (collider == null)
    {
        Debug.LogError("Tower is missing CircleCollider2D! Please add one to the prefab.");
        return;
    }

    // Ensure it's a trigger so enemies can enter range
    collider.isTrigger = true;

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
        weaponAnimator = transform.Find("Weapon").GetComponent<Animator>();
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
            weaponAnimator?.SetTrigger("Attack"); 
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.SetTarget(enemy.transform);
            bulletScript.damage = damage;

        }
    }
}
