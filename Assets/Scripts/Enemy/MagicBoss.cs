using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MagicBoss : MonoBehaviour
{
    [SerializeField] float attackType;
    [SerializeField] float bossHealth = 800f;
    [SerializeField] int healthTimer = 10;

    [SerializeField] bool attackDelay;
    [SerializeField] bool isAttacking;
    [SerializeField] bool sightRange;
    [SerializeField] bool bossIsDead;
    
    [SerializeField] Transform player;
    public Slider healthSlider;
    [SerializeField] GameObject projectileRed, projectileBlue;
    [SerializeField] ParticleSystem healPart, attack1, attack2, attack2_1, shield;
    Animator magicAnim;

    private void Start()
    {
        magicAnim = GetComponent<Animator>();
    }

    void Update()
    {
        sightRange = Physics.CheckSphere(transform.position, 100f, LayerMask.GetMask("Player"));
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        healthSlider.value = bossHealth;

        if (sightRange && !attackDelay && !bossIsDead)
        {
            if (!isAttacking)
            {
                shield.Stop();
                isAttacking = true; attackDelay = true;
                attackType = Random.Range(0, 3);

                magicAnim.SetBool("isAttacking", true);
                magicAnim.SetFloat("attackType", Mathf.RoundToInt(attackType));

                switch (attackType)
                {
                    case 0: // Normal attack;
                        attack1.Play();
                        Invoke(nameof(AttackResetter), 2.3f);
                        break;
                    case 1: // Big Attack
                        attack2.Play();
                        attack2_1.Play();
                        Invoke(nameof(AttackResetter), 2.69f);
                        break;
                    case 2: // Heal
                        healthTimer = 10;
                        healPart.Play();
                        StartCoroutine(HealthBuff());
                        Invoke(nameof(AttackResetter), 2.67f);
                        break;
                    default:
                        isAttacking = false;
                        break;
                }
            }
            
        }
    }

    IEnumerator HealthBuff()
    {
        while (true)
        {
            if (bossHealth < 800)
            {
                healthTimer--;
                bossHealth += 10f;
                yield return new WaitForSeconds(.25f);
            }
            else
            {
                break;
            }

            if (healthTimer == 0)
            {
                break;
            }
        }
        
    }

    void AttackResetter()
    {
        magicAnim.SetBool("isAttacking", false);
        isAttacking = false;
        healPart.Stop();
        shield.Play();
        Invoke(nameof(AttackDelay), 8f);
    }

    void AttackDelay()
    {
        attackDelay = false;
    }

    public void Attack1Vs()
    {
        attack1.Stop();
        Instantiate(projectileBlue, transform.position, Quaternion.identity).GetComponent<Rigidbody>().AddForce(transform.forward * 50f, ForceMode.Impulse);
    }

    public void Attack2Vs()
    {
        attack2.Stop();
        attack2_1.Stop();
        Instantiate(projectileRed, transform.position, Quaternion.identity).GetComponent<Rigidbody>().AddForce(transform.forward * 50f, ForceMode.Impulse);
    }

    public void BossTakenDamage(float amountOfDamage)
    {
        if (isAttacking)
        {
            bossHealth -= (amountOfDamage * 3);
        }
        else
        {
            bossHealth -= amountOfDamage;
        }

        if (bossHealth <= 0)
        {
            bossHealth = 0f;
            bossIsDead = true;

            magicAnim.SetTrigger("Dead");
            healthSlider.gameObject.SetActive(false);

            attack1.Stop();
            attack2.Stop();
            attack2_1.Stop();
            shield.Stop();
            healPart.Stop();
        }

    }
}
