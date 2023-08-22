using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerNetworkMovementTest : NetworkBehaviour
{
    //How fast we move
    [SerializeField] private float _playerSpeed = 5;
    //Player position
    [SerializeField] private Transform _playerTransform;
    //Player Controller - component that moves character. Has an inbuilt ground check but not gravity.
    [SerializeField] private CharacterController _characterController;
    //Generated (new) input system
    [SerializeField] private PlayerInput _playerInput;
    //Gravity
    [SerializeField] private float _gravity = -9.2f;
    //Jumping
    [SerializeField] private float _jumpSpeed = 20f;


    private void Start()
    {
        _playerTransform = GetComponent<Transform>();
        _characterController = GetComponent<CharacterController>();
        _playerInput = new PlayerInput();
        _playerInput.Enable();
    }


    private void Move(Vector2 input)
    {
        Vector3 calculateMovement = input.x * _playerTransform.right + input.y * _playerTransform.forward;
        _characterController.Move(calculateMovement * _playerSpeed * Time.deltaTime);

        Vector3 applyGravity = new Vector3(0, _gravity, 0);
        _characterController.Move(applyGravity * Time.deltaTime);

    }

    private void Jump(float input)
    {
        Vector2 playerJump = input * _playerTransform.up;
        _characterController.Move(playerJump * _jumpSpeed * Time.deltaTime);
    }


    // Requesting permission to move from the server based on behaviour.
    [ServerRpc]
    private void MoveServerRpc(Vector2 input)
    {
        Move(input);

    }

    [ServerRpc]
    private void JumpServerRpc(float input)
    {
        Jump(input);
    }


    private void Update()
    {
        //read movement values. Reference (_playerInput), action map (Player), Action (Movement) (Referring to the new input system)
        Vector2 moveInput = _playerInput.Player.Movement.ReadValue<Vector2>();
        float jumpInput = _playerInput.Player.Jump.ReadValue<float>();

        //Check are we the server
        if (IsServer && IsLocalPlayer)
        {
            //If so move
            Move(moveInput);
            if (_characterController.isGrounded)
            {
                if (_playerInput.Player.Jump.IsPressed()) 
                {
                    Jump(jumpInput);
                    Debug.Log("I am jumping");
                }

            }
        }

        //else if client
        else if (IsClient && IsLocalPlayer)
        {
            //Please let us move server man
            MoveServerRpc(moveInput);
            if (_playerInput.Player.Jump.IsPressed())
            {
                if (_characterController.isGrounded)
                {

                    JumpServerRpc(jumpInput);
                    Debug.Log("I am client jumping");
                }
            }
        }

    }

}
