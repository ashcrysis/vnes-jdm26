using Data;
using Tiles;
using UnityEngine;
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
        private TileBase playerTile;
        private Rigidbody2D rb;
        private PlayerInput playerInput;

        private static readonly Color[] playerColors = new Color[]
        {
            Color.red, Color.blue, Color.green, Color.yellow
        };

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            playerInput = GetComponent<PlayerInput>();

            playerInput.actions["Move"].performed += OnMove;
            playerInput.actions["Move"].canceled += OnMove;

            int index = Mathf.Clamp(playerInput.playerIndex, 0, playerColors.Length - 1);
            playerColor = playerColors[index];

            if (floorTilemap == null)
                floorTilemap = GameObject.Find("FloorTilemap")?.GetComponent<Tilemap>();

            if (floorTilemap == null)
            {
                Debug.LogError("FloorTilemap não encontrado na cena!");
                return;
            }

            playerTile = ScriptableTile.CreateTile(playerColor, floorTilemap);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }

        void FixedUpdate()
        {
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
            Vector2 normalized = moveInput;
            if (normalized.magnitude > 1f)
                normalized.Normalize();

            // Passa para o Animator
            animator.SetFloat("x", normalized.x);
            animator.SetFloat("y", normalized.y);
            animator.SetBool("isMoving", moveInput.sqrMagnitude > 0.01f);
        }
    }
}