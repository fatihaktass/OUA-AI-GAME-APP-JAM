using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] bool isTheBigProjectile;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (!isTheBigProjectile)
            {
                collision.collider.GetComponent<PlayerController>().PlayerTakenDamage(15);
                Destroy(gameObject);
            }
            else
            {
                collision.collider.GetComponent<PlayerController>().PlayerTakenDamage(25);
                Destroy(gameObject);
            }
            
            Destroy(gameObject, 5f);
        }
    }
}
