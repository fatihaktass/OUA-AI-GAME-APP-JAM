using UnityEngine;
using UnityEngine.AI;

public class NpcController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] bool sightArea;
    [SerializeField] bool attackArea;
    [SerializeField] bool npcAttacking;

    Animator animator;
    NavMeshAgent agent;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        sightArea = Physics.CheckSphere(transform.position, 20f, LayerMask.GetMask("Player"));
        attackArea = Physics.CheckSphere(transform.position, 1f, LayerMask.GetMask("Player"));

        if (sightArea)
        {
            Vector3 playerPos = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.LookAt(playerPos);
            agent.SetDestination(playerPos);
        }

        if (attackArea && !npcAttacking)
        {
            npcAttacking = true;
            animator.SetBool("isAttacking", true);
            Invoke(nameof(AttackReset), 1.55f);
        }
    }

    void AttackReset()
    {
        animator.SetBool("isAttacking", false);
        npcAttacking = false;
    }
}
