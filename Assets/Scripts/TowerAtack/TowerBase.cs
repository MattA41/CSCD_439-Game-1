using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{

    [Header("Properties")]
    public int damage = 10;
    public float attackCooldown = 1f;
    public float attackRange = 4f;

    [Header("References")]
    public Transform firePoint;
    public GameObject bulletPrefab;

    [Header("Settings")]
    public bool canRotateWeapon = true;

    protected List<GameObject> enemiesInRange = new List<GameObject>();
    protected float lastAttackTime = 0f;
    protected Animator weaponAnimator;
    protected Transform weaponTransform;

    protected virtual void Awake()
    {
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        if (collider == null)
        {
            Debug.LogError("Tower is missing CircleCollider2D! Please add one to the prefab.");
            return;
        }

        collider.isTrigger = true;
        collider.radius = attackRange;

        Transform rangeVisual = transform.Find("RangeVisual");
        if (rangeVisual != null)
        {
            float spriteRadius = 0.5f;
            float scaleFactor = attackRange / spriteRadius;
            rangeVisual.localScale = Vector3.one * scaleFactor;
        }
    }

    protected virtual void Start()
    {
        weaponAnimator = transform.Find("Weapon").GetComponent<Animator>();
        weaponTransform = transform.Find("Weapon").transform;
    }

    protected virtual void Update()
    {
        enemiesInRange.RemoveAll(enemy => enemy == null);

        if (enemiesInRange.Count > 0)
        {
            if (canRotateWeapon)
                RotateTowardsTarget(enemiesInRange[0].transform);

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    protected virtual void Attack()
    {
        // Default attack - overridden by subclasses
        if (enemiesInRange.Count > 0)
        {
            weaponAnimator?.SetTrigger("Attack");
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.SetTarget(enemiesInRange[0].transform);
            bulletScript.damage = damage;
        }
    }

    protected void RotateTowardsTarget(Transform target)
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        weaponTransform.rotation = Quaternion.RotateTowards(weaponTransform.rotation, targetRotation, 700f * Time.deltaTime);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }
}
