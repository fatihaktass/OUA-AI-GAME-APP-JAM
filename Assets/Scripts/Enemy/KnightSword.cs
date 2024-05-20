using UnityEngine;

public class KnightSword : MonoBehaviour
{
    PlayerController controller;
    NpcController npcController;

    private void Start()
    {
        controller = FindAnyObjectByType<PlayerController>();
        npcController = GetComponentInParent<NpcController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !npcController.npcIsDead)
        {
            controller.PlayerTakenDamage(20);
        }
    }
}
