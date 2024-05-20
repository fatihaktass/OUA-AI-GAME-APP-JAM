using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject deathPanel, bossSlider;
    [SerializeField] Transform player;
    [SerializeField] Camera mainCamera;
    [SerializeField] PlayableDirector _playableDirector;
    [SerializeField] Transform attackPoint;
    [SerializeField] Material scene1mat, scene2mat;
    [SerializeField] GameObject scene1obj, scene2obj ,finalSceneObj;
    public static bool scene2;
    public static bool finalScene;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void DeathPanel()
    {
        _playableDirector.Play();
    }

    public void ShootingRay()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("Knight")))
        {
            hit.collider.GetComponentInParent<NpcController>().NpcTakenDamage(50);
        }
        if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("Boss")))
        {
            hit.collider.GetComponentInParent<MagicBoss>().BossTakenDamage(10);
        }
    }

    public void Scene1Completed()
    {
        scene2 = true; 
        finalScene = false;
        scene2obj.SetActive(true);
        scene1obj.SetActive(false);
        RenderSettings.skybox = scene2mat;
    }

    public void Scene2Completed()
    {
        scene2 = false;
        finalScene = true;
        RenderSettings.skybox = scene1mat;
        finalSceneObj.SetActive(true);
        scene2obj.SetActive(false);
        FindAnyObjectByType<MagicBoss>().healthSlider.gameObject.SetActive(true);
    }

    public void ResetGame()
    {
        if (!scene2 && !finalScene)
        {
            scene1obj.SetActive(true);
            scene2obj.SetActive(false);
            RenderSettings.skybox = scene1mat;
        }
        if (scene2 && !finalScene)
        {
            scene2obj.SetActive(true);
            scene1obj.SetActive(false);
            RenderSettings.skybox = scene2mat; 
        }
        if (finalScene)
        {
            scene2obj.SetActive(false);
            finalSceneObj.SetActive(true);
            FindAnyObjectByType<MagicBoss>().healthSlider.gameObject.SetActive(true);
            RenderSettings.skybox = scene1mat;
        }
    }
}
