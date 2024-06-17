using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int health; // 피 설정
    public bool isDead = false;

    private EnemyController enemyController;

    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
        health = maxHealth;
    }
    

    public void TakeDamage(int damage) // 데미지 받기
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
