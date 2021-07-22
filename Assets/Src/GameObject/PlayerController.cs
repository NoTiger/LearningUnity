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
        private Vector2 _playerVelocity = new Vector2(0, 0);
        private float _horizontalMovementDir = 0;
        private float _verticalVelocity = 0;

        private Animator _animator;
        private SpriteRenderer _sprite;

        private bool _doubleJumped = false;
        private bool _grounded = false;

        // Use this for initialization
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _sprite = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            _playerVelocity = GetVelocityFromInput();
            _rigidbody.velocity = _playerVelocity;
            HandleAnimation();
            IsGrounded();
        }

        private Vector2 GetVelocityFromInput()
        {
            SetVectorByHorizontalInput();
            SetVectorByVerticalInput();

            return new Vector2(_horizontalMovementDir * _horizontalSpeed, _verticalVelocity);
        }

        public void SetVectorByHorizontalInput()
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

        public void SetVectorByVerticalInput()
        {

            if (Input.GetButtonDown("Jump"))
            {
                if (_grounded)
                {
                    _verticalVelocity = _verticalSpeed;
                }
                else if (!_doubleJumped)
                {
                    // double jump
                    _doubleJumped = true;
                    _horizontalMovementDir = Input.GetAxis("Horizontal");
                    _verticalVelocity = _verticalSpeed;
                }
            }
            else
            {
                _verticalVelocity = _rigidbody.velocity.y;
            }
        }

        private void HandleAnimation()
        {
            if (_playerVelocity.x != 0)
            {
                _animator.SetBool("isRunning", true);
                if (_playerVelocity.x > 0)
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
                _animator.SetBool("isRunning", false);
            }
        }

        private void IsGrounded()
        {
            _grounded = Physics2D.BoxCast(_groundCheckBox.bounds.center, _groundCheckBox.bounds.size, 0f, Vector2.down, .1f, groundLayer);

            if (_grounded)
            {
                _doubleJumped = false;
            }
        }
    }
}