using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject deathPanel;
    [SerializeField] PlayableDirector _playableDirector;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void DeathPanel()
    {
        _playableDirector.Play();
    }
}
