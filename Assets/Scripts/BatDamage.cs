using UnityEngine;

public class BatDamage : MonoBehaviour
{
    public int damage = 99999;
    private StaffSound sound;

    private void Start()
    {
        sound = GetComponent<StaffSound>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            // 예시로 만든 EnemyHealth 스크립트를 가져옴
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                sound.PlaySound();
                enemyHealth.TakeDamage(damage);
            }
        }
    }
}
