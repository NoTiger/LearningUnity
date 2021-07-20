using UnityEngine;
using Learning.Interface;

namespace Leaning.GameObject
{
    public class PlayerController : MonoBehaviour, IBasicMovementDetector
    {
        public float horizontalSpeed = 7f;
        public float verticalSpeed = 7f;

        private Rigidbody2D _rigidbody;
        private Vector2 _playerVelocity = new Vector2(0, 0);

        private bool _hasDoubledJump = false;
        private bool _touchingSurface = false;

        // Use this for initialization
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D()
        {
            _touchingSurface = true;
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
        }

        private Vector2 GetMovementVelocityFromInput()
        {
            float dirX = GetHorizontalMovement();
            float velocityY = GetVerticalMovement();

            return new Vector2(dirX * horizontalSpeed, velocityY);
        }

        public float GetHorizontalMovement()
        {
            float horizontalInput = Input.GetAxis("Horizontal");

            if (_touchingSurface)
            {
                return horizontalInput;
            }

            return _rigidbody.velocity.x / horizontalSpeed;
        }

        public float GetVerticalMovement()
        {
            bool allowJump = _touchingSurface || !_hasDoubledJump;

            if (Input.GetButtonDown("Jump") && allowJump)
            {
                _hasDoubledJump = true;
                return verticalSpeed;
            }
            if (_touchingSurface)
            {
                // reset
                _hasDoubledJump = false;
            }

            return _rigidbody.velocity.y;
        }
    }
}