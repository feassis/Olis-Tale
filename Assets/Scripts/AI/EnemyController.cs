using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float LookRadious = 10f;

    [SerializeField] private Animator animator;
    [SerializeField] private float damage;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackDelay = 0.4f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private DamagebleCharacter character;

    Transform target;
    NavMeshAgent agent;

    private bool isAttacking = false;

    private Vector3 previousPosition;
    private Vector3 wanderPosition;
    private float curSpeed;

    private readonly string attack = "Attack";
    private readonly string idle = "IdleNormal";

    private void Start()
    {
        target = PlayerManager.instance.Player.transform;
        agent = GetComponent<NavMeshAgent>();
        previousPosition = transform.position;
        StartCoroutine(Wander());
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if(distance <= LookRadious && !character.IsDead)
        {
            agent.SetDestination(target.position);
            if(distance <= agent.stoppingDistance)
            {
                if (!isAttacking)
                {
                    StartCoroutine(AttackPlayer());
                }
                FaceTarget();
            }
        }
        else if(!character.IsDead)
        {
            agent.SetDestination(wanderPosition);
        }

        Vector3 curMove = transform.position - previousPosition;
        curSpeed = curMove.magnitude / Time.deltaTime;
        previousPosition = transform.position;

        animator.SetFloat("velocity", curSpeed);
    }

    private Collider[] GetPlayersInRange(Transform pointOfOrigin, float range)
    {
        return Physics.OverlapSphere(pointOfOrigin.position, range, playerLayer);
    }

    private void DamagePlayer(Collider[] playersInRange, float damage)
    {
        Debug.Log("attack player");
        foreach (Collider player in playersInRange)
        {
            player.GetComponent<PlayerCharacter>().TakeDamage(damage);
        }
    }

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;
        animator.Play(attack);

        yield return WaitAnimationToEnd(attack);

        var players = GetPlayersInRange(attackPoint, attackRange);
        DamagePlayer(players, damage);

        animator.Play(idle);
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }

    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private IEnumerator WaitAnimationToEnd(string animName)
    {
        AnimatorStateInfo state;
        do
        {
            yield return null;
            state = animator.GetCurrentAnimatorStateInfo(0);
        } while (!state.IsName(animName) || state.normalizedTime < 1f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, LookRadious);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, agent.stoppingDistance);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private IEnumerator Wander()
    {
        while (true)
        {
            wanderPosition = new Vector3(transform.position.x + Random.Range(-15f, 15f), 0, transform.position.z + Random.Range(-15f, 15f)) ;
            yield return new WaitForSeconds(2f);
        }
    }
}
