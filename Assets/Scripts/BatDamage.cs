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
            // ���÷� ���� EnemyHealth ��ũ��Ʈ�� ������
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                sound.PlaySound();
                enemyHealth.TakeDamage(damage);
            }
        }
    }
}
