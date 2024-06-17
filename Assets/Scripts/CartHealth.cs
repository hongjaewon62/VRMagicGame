using UnityEngine;
using UnityEngine.UI;

public class CartHealth : MonoBehaviour
{
    public static CartHealth instance;

    public int maxHealth = 100; // 최대 체력
    public int currentHealth;

    public BgmChange bgm;

    public Slider hpSlider;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        SetHealth(currentHealth);
        Debug.Log("Cart Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Cart Destroyed!");
        bgm.MainSound();
        // 마차가 파괴되었을 때의 동작 (애니메이션, 파티클 효과 등)
        Destroy(gameObject);
    }

    public void SetMaxHealth(float health)          // UI 최대 체력 설정
    {
        hpSlider.maxValue = health;
        hpSlider.value = health;
    }

    public void SetHealth(float health)
    {
        hpSlider.value = health;
    }
}
