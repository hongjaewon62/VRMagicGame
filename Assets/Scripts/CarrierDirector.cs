using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarrierDirector : MonoBehaviour
{
    public List<Transform> PatrolPath = new List<Transform>(); // Path 리스트
    private NavMeshAgent NMA; // Nav Mesh Agent
    private int currentPath = 0; // 현재 순찰 좌표 (PatrolPath)
    public GameObject reward;
    public Transform parent;

    private void Start()
    {
        NMA = GetComponent<NavMeshAgent>(); // NavMeshAgent 컴포넌트 초기화
        if (NMA == null)
        {
            Debug.LogError("NavMeshAgent component not found on this game object.");
        }
        else if (PatrolPath.Count > 0)
        {
            NMA.SetDestination(PatrolPath[currentPath].position); // 처음 경로로 이동 시작
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("c_path"))
        { // Path 에 도달할 경우
            if (other.gameObject.Equals(PatrolPath[currentPath].gameObject))
            {
                StartCoroutine(SetCurrentPath());
            }
        }

        else if (other.CompareTag("q_clear"))
        {
            // 충돌한 오브젝트를 비활성화
            this.gameObject.SetActive(false);

            // 충돌한 오브젝트의 위치와 회전을 가져옴
            Vector3 spawnPosition = other.transform.position;
            Quaternion spawnRotation = other.transform.rotation;

            // 프리팹을 해당 위치에 생성
            Instantiate(reward, spawnPosition, spawnRotation, parent);
        }
    }

    private bool is_SetPath = false; // 중복 카운트 방지

    IEnumerator SetCurrentPath()
    {
        if (!is_SetPath)
        {
            is_SetPath = true;

            currentPath += 1; // 다음 경로로 이동
            if (currentPath >= PatrolPath.Count)
            {
                currentPath = 0; // 마지막 인덱스에 도달하면 다시 처음으로
            }

            yield return new WaitForSeconds(0f); // 일시 대기

            if (NMA != null)
            {
                NMA.SetDestination(PatrolPath[currentPath].position); // 지정된 경로로 이동
            }
            else
            {
                Debug.LogError("NavMeshAgent is not assigned.");
            }

            is_SetPath = false;
        }
    }
}
