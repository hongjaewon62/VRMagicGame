using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarrierDirector : MonoBehaviour
{
    public List<Transform> PatrolPath = new List<Transform>(); // Path ����Ʈ
    private NavMeshAgent NMA; // Nav Mesh Agent
    private int currentPath = 0; // ���� ���� ��ǥ (PatrolPath)
    public GameObject reward;
    public Transform parent;

    private void Start()
    {
        NMA = GetComponent<NavMeshAgent>(); // NavMeshAgent ������Ʈ �ʱ�ȭ
        if (NMA == null)
        {
            Debug.LogError("NavMeshAgent component not found on this game object.");
        }
        else if (PatrolPath.Count > 0)
        {
            NMA.SetDestination(PatrolPath[currentPath].position); // ó�� ��η� �̵� ����
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("c_path"))
        { // Path �� ������ ���
            if (other.gameObject.Equals(PatrolPath[currentPath].gameObject))
            {
                StartCoroutine(SetCurrentPath());
            }
        }

        else if (other.CompareTag("q_clear"))
        {
            // �浹�� ������Ʈ�� ��Ȱ��ȭ
            this.gameObject.SetActive(false);

            // �浹�� ������Ʈ�� ��ġ�� ȸ���� ������
            Vector3 spawnPosition = other.transform.position;
            Quaternion spawnRotation = other.transform.rotation;

            // �������� �ش� ��ġ�� ����
            Instantiate(reward, spawnPosition, spawnRotation, parent);
        }
    }

    private bool is_SetPath = false; // �ߺ� ī��Ʈ ����

    IEnumerator SetCurrentPath()
    {
        if (!is_SetPath)
        {
            is_SetPath = true;

            currentPath += 1; // ���� ��η� �̵�
            if (currentPath >= PatrolPath.Count)
            {
                currentPath = 0; // ������ �ε����� �����ϸ� �ٽ� ó������
            }

            yield return new WaitForSeconds(0f); // �Ͻ� ���

            if (NMA != null)
            {
                NMA.SetDestination(PatrolPath[currentPath].position); // ������ ��η� �̵�
            }
            else
            {
                Debug.LogError("NavMeshAgent is not assigned.");
            }

            is_SetPath = false;
        }
    }
}
