using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floor_spell_damage : MonoBehaviour
{

    public float DestroyExplosion = 4.0f;
    public float DestroyChildren = 2.0f;
    public float damageInterval = 1.0f;  // 데미지 주는 간격 (초)

    private HashSet<Collider> collidersInRange = new HashSet<Collider>();

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("enemy"))
        {
            collidersInRange.Add(col);
            StartCoroutine(DamageEnemy(col));
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("enemy"))
        {
            collidersInRange.Remove(col);
        }
    }

    private IEnumerator DamageEnemy(Collider col)
    {
        while (collidersInRange.Contains(col))
        {
            EnemyHealth enemyHealth = col.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(5);
            }
            yield return new WaitForSeconds(damageInterval);
        }
    }

    private void OnDestroy()
    {
        collidersInRange.Clear();
    }
}
