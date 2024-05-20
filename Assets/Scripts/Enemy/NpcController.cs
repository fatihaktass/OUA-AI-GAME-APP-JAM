using UnityEngine;
using UnityEngine.AI;

public class NpcController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] bool sightArea;
    [SerializeField] bool attackArea;
    [SerializeField] bool npcAttacking;
    public bool npcIsDead;
    float knightHealth = 100f;

    Animator animator;
    NavMeshAgent agent;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!npcIsDead)
        {
            sightArea = Physics.CheckSphere(transform.position, 20f, LayerMask.GetMask("Player"));
            attackArea = Physics.CheckSphere(transform.position, 1f, LayerMask.GetMask("Player"));

            if (sightArea)
            {
                Vector3 playerPos = new Vector3(player.position.x, transform.position.y, player.position.z);
                transform.LookAt(playerPos);
                animator.SetBool("Run", true);
                agent.SetDestination(player.position);
            }

            if (attackArea && !npcAttacking)
            {
                npcAttacking = true;
                animator.SetBool("isAttacking", true);
                Invoke(nameof(AttackReset), 1.55f);
            }
        }
        
    }

    void AttackReset()
    {
        animator.SetBool("isAttacking", false);
        npcAttacking = false;
    }

    public void NpcTakenDamage(float amountOfDamage)
    {
        knightHealth -= amountOfDamage;

        if (knightHealth <= 0)
        {
            npcIsDead = true;
            agent.SetDestination(transform.position);
            animator.SetTrigger("Dead");
            Destroy(gameObject, 10f);
        }
    }
}
