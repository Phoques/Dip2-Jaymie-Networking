using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

//Bug found, if player (On client or host / server) hold a direction while jumping, the reciever (If you're moving host, its client and vice versa) see that the other players avatar
//Does not ground itself and floats up in the air.
public class PlayerNetworkMovementTest : NetworkBehaviour
{
    //How fast we move
    [SerializeField] private float _playerSpeed = 5;
    //Player position
    [SerializeField] private Transform _playerTransform; // No use?
    //Player Controller - component that moves character. Has an inbuilt ground check but not gravity.
    [SerializeField] private CharacterController _characterController;
    //Generated (new) input system
    [SerializeField] private PlayerInput _playerInput;
    //Calculate players movement
    [SerializeField] private Vector3 _calculateMovement;
    //Gravity
    [SerializeField] private float _gravity = -20f;
    //Jumping
    [SerializeField] private float _jumpSpeed = 8f;
    //Crouching
    [SerializeField] private float _crouchSpeed = 2.5f;
    //Sprinting
    [SerializeField] private float _sprintSpeed = 10f;

    public bool endJump;


    private void Start()
    {
        _playerTransform = GetComponent<Transform>();   
        _characterController = GetComponent<CharacterController>();
        _playerInput = new PlayerInput();
        _playerInput.Enable();
    }


    private void Move(Vector2 input, float jump, float sprint, float crouch)
    {
        if (_characterController.isGrounded)
        {
            endJump = true;
            _calculateMovement = new Vector3(input.x, 0, input.y);
            _calculateMovement = transform.TransformDirection(_calculateMovement);
            _calculateMovement *= _playerSpeed;

            if (jump > 0)
            {
                endJump = false;
                _calculateMovement.y = _jumpSpeed;
                if (endJump)
                {
                    //_playerTransform = new Vector3(0, 0, 0); See about changing the transform to 0,0,0 or playerpos, 0, playerpos
                }

                Debug.Log("Jump");
            }
            if (sprint > 0)
            {
                _calculateMovement = new Vector3(input.x, 0, input.y);
                _calculateMovement = transform.TransformDirection(_calculateMovement);
                _calculateMovement *= _sprintSpeed;
                Debug.Log("Sprint");
            }

            if (crouch > 0)
            {
                _calculateMovement = new Vector3(input.x, 0, input.y);
                _calculateMovement = transform.TransformDirection(_calculateMovement);
                _calculateMovement *= _crouchSpeed;
                Debug.Log("Crouch");
            }
        }

        _calculateMovement.y -= _gravity * Time.deltaTime;
        _characterController.Move(_calculateMovement * Time.deltaTime);



    }

    // Requesting permission to move from the server based on behaviour.
    [ServerRpc]
    private void MoveServerRpc(Vector2 input, float jump, float sprint, float crouch)
    {
        Move(input, jump, sprint, crouch);

    }


    private void Update()
    {
        //read movement values. Reference (_playerInput), action map (Player), Action (Movement) (Referring to the new input system)
        Vector2 moveInput = _playerInput.Player.Movement.ReadValue<Vector2>();
        float jumpInput = _playerInput.Player.Jump.ReadValue<float>();
        float sprintInput = _playerInput.Player.Sprint.ReadValue<float>();
        float crouchInput = _playerInput.Player.Crouch.ReadValue<float>();

        //Check are we the server
        if (IsServer && IsLocalPlayer)
        {
            //If so move
            Move(moveInput, jumpInput, sprintInput, crouchInput);
        }
        else if (IsClient && IsLocalPlayer)
        {
            MoveServerRpc(moveInput, jumpInput, sprintInput, crouchInput);
        }
    }

}
