using UnityEngine;

public class KnightSword : MonoBehaviour
{
    PlayerController controller;

    private void Start()
    {
        controller = FindAnyObjectByType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            controller.PlayerTakenDamage(20);
        }
    }
}
