using UnityEngine;
using UnityEngine.UI;

public class CartHealth : MonoBehaviour
{
    public static CartHealth instance;

    public int maxHealth = 100; // �ִ� ü��
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
        // ������ �ı��Ǿ��� ���� ���� (�ִϸ��̼�, ��ƼŬ ȿ�� ��)
        Destroy(gameObject);
    }

    public void SetMaxHealth(float health)          // UI �ִ� ü�� ����
    {
        hpSlider.maxValue = health;
        hpSlider.value = health;
    }

    public void SetHealth(float health)
    {
        hpSlider.value = health;
    }
}
