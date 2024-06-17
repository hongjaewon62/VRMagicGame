using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDirector : MonoBehaviour
{
    public Vector3 attackRange;                 // ���� ����
    public float attackDistance;                // ���� �Ÿ�
    public Vector3 sightRange;                  // �þ� ����
    public float sightDistance;                 // �þ� �Ÿ�
    public LayerMask playerLayer;               // �÷��̾� ���̾�

    private BoxCollider boxCollider;

    public List<Transform> PatrolPath = new List<Transform>(); // Path ����Ʈ
    private NavMeshAgent NMA; // Nav Mesh Agent
    private Animator animator; // Animator ������Ʈ
    private int currentPath = 0; // ���� ���� ��ǥ (PatrolPath)

    private void Start()
    {
        NMA = GetComponent<NavMeshAgent>(); // NavMeshAgent ������Ʈ �ʱ�ȭ
        animator = GetComponent<Animator>(); // Animator ������Ʈ �ʱ�ȭ
        if (NMA == null)
        {
            Debug.LogError("NavMeshAgent component not found on this game object.");
        }
        else if (PatrolPath.Count > 0)
        {
            NMA.SetDestination(PatrolPath[currentPath].position); // ó�� ��η� �̵� ����
            SetMovingAnimation(true); // �̵� �ִϸ��̼� ����
        }

        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("path"))
        { // Path �� ������ ���
            if (other.gameObject.Equals(PatrolPath[currentPath].gameObject))
            {
                StartCoroutine(SetCurrentPath());
            }
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

            SetMovingAnimation(false); // Idle �ִϸ��̼����� ��ȯ
            yield return new WaitForSeconds(2f); // �Ͻ� ���

            if (NMA != null)
            {
                NMA.SetDestination(PatrolPath[currentPath].position); // ������ ��η� �̵�
                SetMovingAnimation(true); // �̵� �ִϸ��̼� ����
            }
            else
            {
                Debug.LogError("NavMeshAgent is not assigned.");
            }

            is_SetPath = false;
        }
    }

    public void SetMovingAnimation(bool isMoving)
    {
        if (animator != null)
        {
            animator.SetBool("Walk", isMoving);
        }
        else
        {
            Debug.LogError("Animator is not assigned.");
        }
    }

    public bool PlayerInSight()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, sightRange / 2, Quaternion.identity, playerLayer);

        foreach (Collider collider in hitColliders)
        {
            Vector3 direction = collider.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            if (angle < sightDistance)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, direction.normalized, out hit, sightDistance))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private void Update()
    {
        if (PlayerInSight())
        {
            NMA.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        if (boxCollider == null)
        {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, attackRange * 2);

        Gizmos.matrix = Matrix4x4.identity;
        // �þ� ����
        Vector3 sightSize = new Vector3(boxCollider.bounds.size.x * sightRange.x, boxCollider.bounds.size.y * sightRange.y, boxCollider.bounds.size.z * sightRange.z);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.forward * sightRange.x * transform.localScale.x * sightDistance, sightSize);
    }
}
