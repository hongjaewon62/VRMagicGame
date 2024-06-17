using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int health; // �� ����
    public bool isDead = false;

    private EnemyController enemyController;

    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
        health = maxHealth;
    }
    

    public void TakeDamage(int damage) // ������ �ޱ�
    {
        if (!isDead)
        {
            health -= damage;
            enemyController.EnemyDamage();
            if (health <= 0)
            {
                enemyController.EnemyDie();
            }
        }
    }
}
