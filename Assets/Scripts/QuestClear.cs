using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestClear : MonoBehaviour
{
    // 충돌 시 생성할 프리팹을 에디터에서 설정할 수 있도록 public으로 선언
    public GameObject prefabToInstantiate;

    // 충돌 감지 메서드
    void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트의 태그가 "Animal"인 경우
        if (collision.gameObject.CompareTag("Animal"))
        {
            // 충돌 지점의 첫 번째 접촉 지점 위치 가져오기
            Vector3 collisionPoint = collision.contacts[0].point;

            // 해당 위치에 프리팹 생성
            Instantiate(prefabToInstantiate, collisionPoint, Quaternion.identity);

            // 충돌한 오브젝트 비활성화
            collision.gameObject.SetActive(false);
        }
    }
}