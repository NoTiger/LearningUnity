using UnityEngine;

namespace Leaning.GameObject
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _horizontalSpeed = 7f;
        [SerializeField] private float _verticalSpeed = 7f;
        [SerializeField] private float _speedMultiplier = 1.3f;

        [SerializeField] private BoxCollider2D _groundCheckBox;
        [SerializeField] private LayerMask groundLayer;

        private Rigidbody2D _rigidbody;
        private SpriteRenderer _sprite;
        private float _horizontalMovementDir = 0;

        private Animator _animator;
        private enum MovementState { idle, running, jumping, falling };

        private bool _grounded = false;
        private bool _jumpRequested = false;
        private bool _running = false;

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
            HandleMovementAnimation();
        }

        private void HandleInput()
        {
            HandleHorizontalInput();
            HandleJumpInput();
        }

        private void HandleHorizontalInput()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _running = true;
            }
            else
            {
                _running = false;
            }

            if (_grounded)
            {
                _horizontalMovementDir = Input.GetAxis("Horizontal");
            }
            else
            {
                _horizontalMovementDir = _rigidbody.velocity.x / _horizontalSpeed;
            }
        }

        private void HandleJumpInput()
        {

            if (Input.GetButtonDown("Jump"))
            {
                if (_grounded)
                {
                    _jumpRequested = true;
                }
            }
        }

        private void HandleMovementAnimation()
        {
            MovementState state;
            float velocityY = _rigidbody.velocity.y;
            float velocityX = _rigidbody.velocity.x;


            if (velocityX != 0)
            {
                state = MovementState.running;

                bool shouldBeRunning = _running & _grounded & (velocityX > .1f | velocityX < -.1f);
                _animator.SetBool("running", shouldBeRunning);

                if (velocityX > .1f)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else if (velocityX < -.1f)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
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
            float multiplier = _running & _grounded? _speedMultiplier : 1;
            _rigidbody.velocity = new Vector2(_horizontalMovementDir * _horizontalSpeed * multiplier, _rigidbody.velocity.y);
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