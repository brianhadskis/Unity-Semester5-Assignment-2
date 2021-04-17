using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    private PlayerControls playerInput;
    private CharacterController controller;
    private Animator playerAnimator;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private bool isRunning;
    private float playerSpeed;

    private Transform cameraMain;

    public CinemachineFreeLook TPCam;
    public CinemachineVirtualCamera OverheadCam;
    public CinemachineVirtualCamera SceneCam;

    [SerializeField]
    private float playerWalkSpeed = 2.0f;
    [SerializeField]
    private float playerRunSpeed = 8.0f;
    [SerializeField]
    private float gravity = -9.81f;
    [SerializeField]
    private float rotationSpeed = 4f;

    public void MakeDead()
    {
        playerInput.Disable();
        playerAnimator.SetTrigger("deadTrigger");
    }

    public void Winner()
    {
        playerInput.Disable();
    }

    private void Awake()
    {
        playerInput = new PlayerControls();
        controller = GetComponent<CharacterController>();
        playerAnimator = gameObject.GetComponentInChildren<Animator>();
        playerInput.Player.Run.performed += ctx => isRunning = ctx.ReadValueAsButton();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraMain = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if (isGrounded && playerInput.Player.Jump.triggered)
        {
            playerAnimator.SetTrigger("jumpTrigger");
        }

        if (playerInput.Player.TPCam.triggered)
        {
            SceneCam.enabled = false;
            OverheadCam.enabled = false;
            TPCam.enabled = true;
            playerInput.Player.Move.Enable();
        }
        if (playerInput.Player.OverheadCam.triggered)
        {
            SceneCam.enabled = false;
            OverheadCam.enabled = true;
            TPCam.enabled = false;
            playerInput.Player.Move.Enable();
        }
        if (playerInput.Player.SceneCam.triggered)
        {
            playerInput.Player.Move.Disable();
            SceneCam.enabled = true;
            OverheadCam.enabled = false;
            TPCam.enabled = false;

        }

        Vector2 movementInput = playerInput.Player.Move.ReadValue<Vector2>();
        Vector3 move = (transform.forward * movementInput.y + transform.right * movementInput.x);

        if (isRunning)
        {
            playerSpeed = playerRunSpeed;
        }
        else
        {
            playerSpeed = playerWalkSpeed;
        }

        controller.Move(move * Time.deltaTime * playerSpeed);

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (movementInput != Vector2.zero)
        {
            playerAnimator.SetBool("isWalking", true);
            if (isRunning)
            {
                playerAnimator.SetBool("isRunning", true);
            }
            else
            {
                playerAnimator.SetBool("isRunning", false);
            }
            Quaternion rotation = Quaternion.Euler(new Vector3(controller.transform.localEulerAngles.x, cameraMain.localEulerAngles.y, controller.transform.localEulerAngles.z));
            controller.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, rotation, Time.deltaTime * rotationSpeed);

            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }

        } else
        {
            playerAnimator.SetBool("isWalking", false);
            playerAnimator.SetBool("isRunning", false);

            GetComponent<AudioSource>().Stop();
        }
    }
}
