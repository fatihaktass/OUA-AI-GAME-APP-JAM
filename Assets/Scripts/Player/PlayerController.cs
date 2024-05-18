using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Features")]
    [SerializeField] float moveSpeed;
    [SerializeField] float crouchHeight;
    [SerializeField] float jumpForce;
    [SerializeField] float gravityForce;
    [SerializeField] bool isGrounded;
    [SerializeField] Transform groundChecker;
    [SerializeField] bool isCrouch;

    float horizontalMovement;
    float verticalMovement;
    Vector3 gravityV3;

    CharacterController charCont;

    private void Start()
    {
        charCont = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        PlayerMovements();
        Gravity();
    }

    void PlayerMovements()
    {
        //Movements WASD --------------------------------------------------------------------------------------------------- //
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");

        Vector3 V3movement = (horizontalMovement * transform.right + verticalMovement * transform.forward).normalized;
        charCont.Move(moveSpeed * Time.deltaTime * V3movement);

        //Jump ------------------------------------------------------------------------------------------------------------- //
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            gravityV3.y = Mathf.Sqrt(jumpForce * -2f * gravityForce);
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
                charCont.height = 1.8f;
            }
        }

        //Speed ------------------------------------------------------------------------------------------------------------- //
        if (isGrounded && !isCrouch)
        {
            if (horizontalMovement != 0 || verticalMovement != 0)
            {
                moveSpeed = 5f;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    moveSpeed = 8f;
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
        }
    }
}
