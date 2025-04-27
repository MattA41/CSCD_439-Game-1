using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOETower : TowerBase
{
    protected override void Attack()
    {
        weaponAnimator?.SetTrigger("Attack");
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        AOEBullet bulletScript = bullet.GetComponent<AOEBullet>();

        // AOE bullet does NOT need a specific target!
        bulletScript.damage = damage;
        bulletScript.explosionRadius = 2.5f; // Example

        // Set bullet direction toward first enemy if available
        if (enemiesInRange.Count > 0)
        {
            bulletScript.SetTargetPosition(enemiesInRange[0].transform.position);
        }
    }
}
