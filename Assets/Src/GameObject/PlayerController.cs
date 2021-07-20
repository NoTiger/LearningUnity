using UnityEngine;
using Learning.Interface;

namespace Leaning.GameObject
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _horizontalSpeed = 7f;
        [SerializeField] private float _verticalSpeed = 7f;

        private Rigidbody2D _rigidbody;
        private Vector2 _playerVelocity = new Vector2(0, 0);
        private float _horizontalMovementDir = 0;
        private float _verticalVelocity = 0;

        private Animator _animator;
        private SpriteRenderer _sprite;

        private bool _hasDoubledJump = false;
        private bool _touchingSurface = false;

        // Use this for initialization
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _sprite = GetComponent<SpriteRenderer>();
        }

        private void OnCollisionEnter2D()
        {
            _touchingSurface = true;
            _hasDoubledJump = false;
        }

        private void OnCollisionExit2D()
        {
            _touchingSurface = false;
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = _playerVelocity;
        }

        // Update is called once per frame
        private void Update()
        {
            _playerVelocity = GetMovementVelocityFromInput();
            handleAnimation();
        }

        private Vector2 GetMovementVelocityFromInput()
        {
            setVectorByHorizontalInput();
            setVectorByVerticalInput();

            return new Vector2(_horizontalMovementDir * _horizontalSpeed, _verticalVelocity);
        }

        public void setVectorByHorizontalInput()
        {
            if (_touchingSurface)
            {
                _horizontalMovementDir = Input.GetAxis("Horizontal");
            }
            else
            {
                _horizontalMovementDir = _rigidbody.velocity.x / _horizontalSpeed;
            }
        }

        public void setVectorByVerticalInput()
        {

            if (Input.GetButtonDown("Jump"))
            {
                if (_touchingSurface)
                {
                    _verticalVelocity = _verticalSpeed;
                }
                else if (!_hasDoubledJump)
                {
                    // double jump
                    _hasDoubledJump = true;
                    _horizontalMovementDir = Input.GetAxis("Horizontal");
                    _verticalVelocity = _verticalSpeed;
                }
            }
            else
            {
                _verticalVelocity = _rigidbody.velocity.y;

            }
        }

        private void handleAnimation()
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
    }
}