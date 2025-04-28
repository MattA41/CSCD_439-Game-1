using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

//[ExecuteAlways]
public class TowerScript : MonoBehaviour
{


    [Header("Properties")]
    public int damage = 10;
    public float attackCooldown = 1f;
    public float attackRange = 4f;

    [Header("Attack Properties")]
    public bool isAOE = false;
    public float radius = 1.5f; // default radius for AOE bullets



    [Header("References")]
    public Transform firePoint;
    public GameObject bulletPrefab;

    [Header("Settings")]
    public bool canRotateWeapon = true;

    private List<GameObject> enemiesInRange = new List<GameObject>();
    private float lastAttackTime = 0f;
    private Animator weaponAnimator;
    private Transform weaponTransform;



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
        weaponTransform = transform.Find("Weapon").transform;
    }

    // Update is called once per frame
    void Update()
    {

        // Clean up dead or missing enemies
        enemiesInRange.RemoveAll(enemy => enemy == null);

        if (enemiesInRange.Count > 0)
        {

            if (canRotateWeapon) RotateTowardsTarget(enemiesInRange[0].transform);

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Attack(enemiesInRange[0]);
                lastAttackTime = Time.time;
            }

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
            bulletScript.isAOE = isAOE;
            bulletScript.radius = radius;
            bulletScript.damage = damage;

        }
    }

    private void RotateTowardsTarget(Transform target)
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        weaponTransform.rotation = Quaternion.RotateTowards(weaponTransform.rotation, targetRotation, 700f * Time.deltaTime);
    }
}
