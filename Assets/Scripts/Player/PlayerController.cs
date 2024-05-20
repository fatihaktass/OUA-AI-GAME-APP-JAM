using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Player Features")]
    [SerializeField] float moveSpeed;
    [SerializeField] float crouchHeight;
    [SerializeField] float jumpForce;
    [SerializeField] float playerHealth = 100f;
    [SerializeField] float gravityForce;
    [SerializeField] bool isGrounded;
    [SerializeField] Transform groundChecker;
    [SerializeField] Slider healthSlider;
    [SerializeField] bool isCrouch;
    [SerializeField] bool isAttacking;
    [SerializeField] bool canMove = true;

    [SerializeField] GameObject player;
    [SerializeField] Animator playerAnimator;

    float horizontalMovement;
    float verticalMovement;
    static bool firstTime;
    Vector3 gravityV3;

    CharacterController charCont;
    GameManager gameManager;

    private void Start()
    {
        charCont = GetComponent<CharacterController>();
        gameManager = FindAnyObjectByType<GameManager>();
        playerAnimator = player.GetComponent<Animator>();
        ChangeMovePermit(true);
        if (!firstTime)
        {
            gameManager.ResetGame();
        }
    }

    private void Update()
    {
        healthSlider.value = playerHealth;
        if (canMove)
        {
            PlayerMovements();
        }
        Gravity();

        if (Input.GetKeyDown(KeyCode.X))
        {
            PlayerTakenDamage(50);
        }
    }

    void PlayerMovements()
    {
        //Movements WASD --------------------------------------------------------------------------------------------------- //
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");

        Vector3 V3movement = (horizontalMovement * transform.right + verticalMovement * transform.forward).normalized;
        charCont.Move(moveSpeed * Time.deltaTime * V3movement);

        playerAnimator.SetFloat("Horizontal", horizontalMovement);
        playerAnimator.SetFloat("Vertical", verticalMovement);

        //Jump ------------------------------------------------------------------------------------------------------------- //
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            gravityV3.y = Mathf.Sqrt(jumpForce * -2f * gravityForce);
            playerAnimator.ResetTrigger("FinishedJump");
            playerAnimator.SetTrigger("Jumping");
        }

        //Speed ------------------------------------------------------------------------------------------------------------- //
        if (isGrounded && !isCrouch)
        {
            if (horizontalMovement != 0 || verticalMovement != 0)
            {
                moveSpeed = 3f;
                playerAnimator.SetFloat("IsRunning", 0);
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    moveSpeed = 5f;
                    playerAnimator.SetFloat("IsRunning", 1);
                }
            }
            else
            {
                moveSpeed = 0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking)
        {
            isAttacking = true;
            playerAnimator.SetBool("isAttacking", true);
            gameManager.ShootingRay();
            Invoke(nameof(AttackResetter), 0.51f);
        }
    }

    void AttackResetter()
    {
        playerAnimator.SetBool("isAttacking", false);
        isAttacking = false;
    }

    void Gravity()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, 0.35f, LayerMask.GetMask("Ground"));

        gravityV3.y += gravityForce * Time.deltaTime;
        charCont.Move(gravityV3 * Time.deltaTime);

        if (gravityV3.y < 0 && isGrounded)
        {
            gravityV3.y = -3f;
            playerAnimator.SetTrigger("FinishedJump");
        }
    }

    public void PlayerTakenDamage(float amountOfDamage)
    {
        playerHealth -= amountOfDamage;

        if (playerHealth <= 0f)
        {
            playerHealth = 0f;
            gameManager.DeathPanel();
            charCont.height = 0f;
            healthSlider.gameObject.SetActive(false);
        }
    }

    public void ChangeMovePermit(bool isCanMove)
    {
        canMove = isCanMove;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StationCapsule"))
        {
            gameManager.Scene1Completed();
            firstTime = true;
        }
        if (other.CompareTag("Portal"))
        {
            gameManager.Scene2Completed();
        }
    }

}
