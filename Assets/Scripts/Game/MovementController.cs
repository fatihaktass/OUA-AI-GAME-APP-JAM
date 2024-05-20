using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementController : MonoBehaviour
{

    private void OnEnable()
    {
        FindAnyObjectByType<PlayerController>().ChangeMovePermit(false);
    }
    private void OnDisable()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
