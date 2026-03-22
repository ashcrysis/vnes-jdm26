using System;
using System.Collections;
using Data;
using Tiles;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        public float moveSpeed = 5f;

        [Header("Tilemap")]
        public Tilemap floorTilemap;

        [Header("Player")]
        public Color playerColor;

        [Header("Animator")]
        public Animator animator;

        private Vector2 moveInput;
        private Vector2 lastFacedDirection = Vector2.down;
        private TileBase playerTile;
        private Rigidbody2D rb;
        private PlayerInput playerInput;

        private int playerIndex;
        
        private bool _initialized;
        
        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            playerInput = GetComponent<PlayerInput>();

            playerInput.actions["Move"].performed += OnMove;
            playerInput.actions["Move"].canceled += OnMove;

            int index = Mathf.Clamp(playerInput.playerIndex, 0, GameSettings.Instance.GetMaxPlayers - 1);
            
            playerIndex = index;
            
            transform.position = GameSettings.Instance.GetSpawnPoint(index).position;
            playerColor = GameSettings.Instance.GetTeamColor(index);
            
            if (!floorTilemap)
                floorTilemap = GameObject.Find("FloorTilemap")?.GetComponent<Tilemap>();

            if (!floorTilemap)
            {
                Debug.LogError("FloorTilemap não encontrado na cena!");
                return;
            }

            playerTile = ScriptableTile.CreateTile(playerColor, floorTilemap);
            
            ScoreUI.Instance.OnPlayerJoined();
            animator = GetComponent<Animator>();
            
            StartCoroutine(MarkAsInitialized());
        }

        public IEnumerator MarkAsInitialized()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            
            _initialized = true;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
            if (moveInput.sqrMagnitude > 0.01f)
            {
                lastFacedDirection = moveInput;
            }
        }

        void FixedUpdate()
        {
            if (!_initialized)
            {
                return;
            }
            
            // Movimento
            Vector2 newPos = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);

            // Marca tile
            Vector3Int cellPos = floorTilemap.WorldToCell(rb.position);
            floorTilemap.SetTile(cellPos, playerTile);
            TilePaintManager.Instance.PaintTile(cellPos, playerInput.playerIndex);
        }

        void Update()
        {
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            if (animator == null) return;

            // Clamp entre -1 e 1 (normalizado)
            Vector2 normalized = lastFacedDirection;
            if (normalized.magnitude > 1f)
                normalized.Normalize();

            // Passa para o Animator
            animator.SetFloat("x", normalized.x);
            animator.SetFloat("y", normalized.y);
            animator.SetBool("isMoving", moveInput.sqrMagnitude > 0.01f);
            // animator.SetBool("isMoving", true);
        }

        public void Die()
        {
            transform.position = GameSettings.Instance.GetSpawnPoint(playerIndex).position;
        }
    }
}