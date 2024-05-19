using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject deathPanel;
    [SerializeField] PlayableDirector _playableDirector;
    public void DeathPanel()
    {
        _playableDirector.Play();
    }
}
