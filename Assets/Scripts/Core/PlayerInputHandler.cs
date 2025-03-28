using UnityEngine;
using GameJamPlatformer.Core.Interfaces;

namespace GameJamPlatformer.Core
{
    /// <summary>
    /// Handles player input and controls character behavior
    /// </summary>
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private ICharacter character;
        private Camera _mainCamera;

        private void Start()
        {
            if (character == null)
            {
                character = GetComponent<ICharacter>();
            }
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            HandleMovement();
            HandleJump();
            HandleAiming();
            HandleShooting();
            HandleWeaponSwitch();
        }

        private void HandleMovement()
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            character.Move(horizontalInput);
        }

        private void HandleJump()
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (character.IsGrounded())
                {
                    character.Jump();
                }
                else
                {
                    character.TryDoubleJump();
                }
            }
        }

        private void HandleAiming()
        {
            Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 characterPosition = character.GetPosition();
            Vector2 aimDirection = mousePosition - characterPosition;
            character.UpdateAim(aimDirection);
        }

        private void HandleShooting()
        {
            if (Input.GetMouseButton(0)) // Left click
            {
                character.FireWeapon();
            }
            else if (Input.GetMouseButton(1)) // Right click
            {
                // Alternative fire (e.g., detonate bombs)
                character.FireWeapon();
            }
        }

        private void HandleWeaponSwitch()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                character.SwitchWeapon();
            }
        }
    }
} 