using System;
using UnityEngine;

namespace Leaning.GameObject
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _horizontalSpeed = 7f;
        [SerializeField] private float _verticalSpeed = 7f;

        [SerializeField] private BoxCollider2D _groundCheckBox;
        [SerializeField] private LayerMask groundLayer;

        private Rigidbody2D _rigidbody;
        private SpriteRenderer _sprite;
        private float _horizontalMovementDir = 0;

        private Animator _animator;
        private enum MovementState { idle, running, jumping, falling };

        private bool _grounded = false;
        private bool _jumpRequested = false;

        // Use this for initialization
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _sprite = GetComponent<SpriteRenderer>();
        }

        #region Update event handler

        private void Update()
        {
            HandleInput();
            HandleAnimation();
        }

        private void HandleInput()
        {
            HandleHorizontalInput();
            HandleJumpInput();
        }

        public void HandleHorizontalInput()
        {
            if (_grounded)
            {
                _horizontalMovementDir = Input.GetAxis("Horizontal");
            }
            else
            {
                _horizontalMovementDir = _rigidbody.velocity.x / _horizontalSpeed;
            }
        }

        public void HandleJumpInput()
        {

            if (Input.GetButtonDown("Jump"))
            {
                if (_grounded)
                {
                    _jumpRequested = true;
                }
            }
        }

        private void HandleAnimation()
        {
            MovementState state;
            float velocityY = _rigidbody.velocity.y;
            float velocityX = _rigidbody.velocity.x;


            if (velocityX != 0)
            {
                state = MovementState.running;
                if (velocityX > 0)
                {
                    _sprite.flipX = false;
                }
                else
                {
                    _sprite.flipX = true;
                }
            }
            else
            {
                state = MovementState.idle;
            }

            if (velocityY > .1f)
            {
                state = MovementState.jumping;
            }
            else if (velocityY < -.1f)
            {
                state = MovementState.falling;
            }


            _animator.SetInteger("state", (int)state);
        }

        #endregion

        #region FixedUpdate event handler

        private void FixedUpdate()
        {
            UpdateRigidBodyVector();
            UpdateRigidBodyForce();
        }

        private void UpdateRigidBodyVector()
        {
            _rigidbody.velocity = new Vector2(_horizontalMovementDir * _horizontalSpeed, _rigidbody.velocity.y);
        }

        private void UpdateRigidBodyForce()
        {
            if (_jumpRequested)
            {
                _rigidbody.AddForce(Vector2.up * _verticalSpeed, ForceMode2D.Impulse);
                _jumpRequested = false;
            }
        }

        #endregion

        #region Collision event handler

        private void OnCollisionEnter2D(Collision2D collision)
        {
            IsGrounded();
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            IsGrounded();
        }

        private void IsGrounded()
        {
            _grounded = Physics2D.BoxCast(_groundCheckBox.bounds.center, _groundCheckBox.bounds.size, 0f, Vector2.down, .1f, groundLayer);
        }

        #endregion
    }
}