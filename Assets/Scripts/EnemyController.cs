using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum EnemyType { meleeAttack, rangedAttack }

    public EnemyType enemyType;                 // 공격 타입
    private EnemyHealth enemyHealth;
    public int attackDamage;                    // 데미지
    public float attackCooldown;                // 데미지 쿨타임
    private bool isAttack = false;

    public Vector3 attackRange;                 // 공격 범위
    public float attackDistance;                // 공격 거리
    public Vector3 sightRange;                  // 시야 범위
    public float sightDistance;                 // 시야 거리

    public LayerMask playerLayer;               // 플레이어 레이어
    public LayerMask horseLayer;

    private Animator anim;
    private BoxCollider boxCollider;

    public NavMeshAgent agent;
    public float moveRange;

    public Transform centrePoint;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        enemyHealth = GetComponent<EnemyHealth>();

        if(centrePoint == null && gameObject.transform.parent != null)
        {
            centrePoint = gameObject.transform.parent.gameObject.transform;
        }

        anim.SetBool("Walk", true);
    }

    private void Update()
    {
        EnemyAttack();
        RandomMove();
    }

    private void RandomMove()                                                       // 랜덤한 위치로 이동
    {
        if(agent == null)
        {
            return;
        }

        if(!PlayerInSight() && !HorseInSight())
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                Vector3 point;
                if (RandomPoint(centrePoint.position, moveRange, out point))
                {
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                    agent.SetDestination(point);
                }
            }
        }
        else if (HorseInSight())
        {
            agent.SetDestination(CartHealth.instance.gameObject.transform.position);
        }
        else if(PlayerInSight())
        {
            agent.SetDestination(PlayerController.instance.gameObject.transform.position);
        }
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)           // 랜덤한 위치 지정
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private void EnemyAttack()
    {
        StartCoroutine(Attack());
    }

    public void EnemyDamage()              // 적 데미지
    {
        anim.Play("Idle", 0, 0);
        anim.StopPlayback();
        anim.SetTrigger("Damage");
    }

    public void EnemyDie()                 // 적 사망
    {
        enemyHealth.isDead = true;
        StartCoroutine(Die());
    }

    private IEnumerator Attack()
    {
        if(isAttack)
        {
            yield break;
        }

        isAttack = true;

        if (PlayerInAttackSight())
        {
            anim.SetTrigger("Attack");
            PlayerController.instance.TakeDamage(attackDamage);
            yield return new WaitForSeconds(attackCooldown);
        }
        if (HorseInAttackSight())
        {
            anim.SetTrigger("Attack");

            // 마차에 데미지 주기
            CartHealth cartHealth = FindObjectOfType<CartHealth>();
            if (cartHealth != null)
            {
                cartHealth.TakeDamage(attackDamage);
            }

            yield return new WaitForSeconds(attackCooldown);
        }
        else
        {
            agent.isStopped = false;
            anim.SetBool("Walk", true);
        }

        isAttack = false;

        yield return null;
    }

    private IEnumerator Die()
    {
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    private bool PlayerInAttackSight()
    {
        bool isHit = Physics.CheckBox(transform.position, attackRange, transform.rotation, playerLayer);
        if (isHit)
        {
            agent.isStopped = true;
            anim.SetBool("Walk", false);
            //playerHealth = hit.transform.GetComponent<Health>();
            Debug.Log("발견");
        }

        return isHit;
    }
    private bool HorseInAttackSight()
    {
        bool isHit = Physics.CheckBox(transform.position, attackRange, transform.rotation, horseLayer);

        if (isHit)
        {
            agent.isStopped = true;
            anim.SetBool("Walk", false);
            Debug.Log("발견");
        }

        return isHit;
    }

    private bool PlayerInSight()
    {
        Vector3 sightSize = new Vector3(boxCollider.bounds.size.x * sightRange.x, boxCollider.bounds.size.y * sightRange.y, boxCollider.bounds.size.z * sightRange.z);
        bool isHit = Physics.CheckBox(transform.position + transform.forward * sightDistance, sightSize / 2f, transform.rotation, playerLayer);

        return isHit;
    }

    private bool HorseInSight()
    {
        Vector3 sightSize = new Vector3(boxCollider.bounds.size.x * sightRange.x, boxCollider.bounds.size.y * sightRange.y, boxCollider.bounds.size.z * sightRange.z);
        bool isHit = Physics.CheckBox(transform.position + transform.forward * sightDistance, sightSize / 2f, transform.rotation, horseLayer);

        return isHit;
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


        // 시야 범위
        Vector3 sightSize = new Vector3(boxCollider.bounds.size.x * sightRange.x, boxCollider.bounds.size.y * sightRange.y, boxCollider.bounds.size.z * sightRange.z);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.forward * sightRange.x * transform.localScale.x * sightDistance, sightSize);

    }
}
