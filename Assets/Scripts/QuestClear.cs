using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestClear : MonoBehaviour
{
    // �浹 �� ������ �������� �����Ϳ��� ������ �� �ֵ��� public���� ����
    public GameObject prefabToInstantiate;

    // �浹 ���� �޼���
    void OnCollisionEnter(Collision collision)
    {
        // �浹�� ������Ʈ�� �±װ� "Animal"�� ���
        if (collision.gameObject.CompareTag("Animal"))
        {
            // �浹 ������ ù ��° ���� ���� ��ġ ��������
            Vector3 collisionPoint = collision.contacts[0].point;

            // �ش� ��ġ�� ������ ����
            Instantiate(prefabToInstantiate, collisionPoint, Quaternion.identity);

            // �浹�� ������Ʈ ��Ȱ��ȭ
            collision.gameObject.SetActive(false);
        }
    }
}