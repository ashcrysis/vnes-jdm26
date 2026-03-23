using System.Collections;
using Data;
using Tiles;
using UI;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Animator)), RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        public float moveSpeed = 5f;
        
        // Personal
        private bool _initialized;
        private int _playerIndex;
        private TileBase _playerTile;
        
        // Components
        private Animator _animator;
        private Rigidbody2D _rb;
        private PlayerInput _playerInput;

        // Movement
        private Vector2 _moveInput;
        private Vector2 _lastFacedDirection = Vector2.down;
        
        
        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _playerInput = GetComponent<PlayerInput>();
            _animator = GetComponent<Animator>();

            _playerInput.actions["Move"].performed += OnMove;
            _playerInput.actions["Move"].canceled += OnMove;

            int index = Mathf.Clamp(_playerInput.playerIndex, 0, GameSettings.Instance.GetMaxPlayers - 1);
            
            _playerIndex = index;
            
            transform.position = GameSettings.Instance.GetSpawnPoint(index).position;
            Color color = GameSettings.Instance.GetTeamColor(index);

            _playerTile = ScriptableTile.CreateTile(color, TilePaintManager.Instance.PaintedTilemap);
            
            ScoreUI.Instance.OnPlayerJoined();
            
            StartCoroutine(MarkAsInitialized());
        }

        private IEnumerator MarkAsInitialized()
        {
            // gambiarra que impede pintar chão antes de teleportar pro spawnpoint
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            
            _initialized = true;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
            if (_moveInput.sqrMagnitude > 0.01f)
            {
                _lastFacedDirection = _moveInput;
            }
        }

        void FixedUpdate()
        {
            if (!_initialized)
            {
                return;
            }
            
            // Movimento
            Vector2 newPos = _rb.position + _moveInput * (moveSpeed * Time.fixedDeltaTime);
            _rb.MovePosition(newPos);

            // Marca tile
            TilePaintManager.Instance.TryPaint(_rb.position, _playerTile, _playerIndex);
        }

        void Update()
        {
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            if (_animator == null) return;

            // Clamp entre -1 e 1 (normalizado)
            Vector2 normalized = _lastFacedDirection;
            if (normalized.magnitude > 1f)
                normalized.Normalize();

            // Passa para o Animator
            _animator.SetFloat("x", normalized.x);
            _animator.SetFloat("y", normalized.y);
            _animator.SetBool("isMoving", _moveInput.sqrMagnitude > 0.01f);
            // animator.SetBool("isMoving", true);
        }

        public void Die()
        {
            transform.position = GameSettings.Instance.GetSpawnPoint(_playerIndex).position;
        }
    }
}