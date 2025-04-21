using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    private Transform target;
    public string damageType = "magic";

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Debug.Log("target Was null");
            Destroy(gameObject);
            return;
        }
        
        Vector2 direction = (target.position - transform.position).normalized;
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
        
        // Rotate bullet to face the target + offset by 90 degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);
        
        // Optionally: destroy if close enough
        if (Vector2.Distance(transform.position, target.position) < .5f)
        {
            if (target.TryGetComponent<Enemy>(out Enemy enemy))
            {
                if(enemy.TakeDamageType == damageType)
                {
                    enemy.TakeDamage(damage);
                    Destroy(this.gameObject);
                }
                
            }
            //Destroy(gameObject);
        }
    }
}
