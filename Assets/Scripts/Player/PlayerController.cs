using UnityEngine;

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
    [SerializeField] bool isCrouch;
    [SerializeField] bool canMove = true;

    [SerializeField] GameObject player;
    [SerializeField] Animator playerAnimator;

    float horizontalMovement;
    float verticalMovement;
    Vector3 gravityV3;

    CharacterController charCont;
    GameManager gameManager;

    private void Start()
    {
        charCont = GetComponent<CharacterController>();
        gameManager = FindAnyObjectByType<GameManager>();
        playerAnimator = player.GetComponent<Animator>();
        ChangeMovePermit(true);
    }

    private void Update()
    {
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

        //Crouch ----------------------------------------------------------------------------------------------------------- //
        if (Input.GetKeyDown(KeyCode.C) && isGrounded)
        {
            isCrouch = !isCrouch;

            if (isCrouch)
            {
                charCont.height = crouchHeight;
                moveSpeed = 2f;
            }
            else
            {
                charCont.height = 1.6f;
            }
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
            playerAnimator.SetTrigger("Dead");
            gameManager.DeathPanel();
        }
    }

    public void ChangeMovePermit(bool isCanMove)
    {
        canMove = isCanMove;
    }
}
