using Tile;
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

        private Vector2 moveInput;
        private TileBase playerTile;
        private Rigidbody2D rb;
        private PlayerInput playerInput;

        // Cores fixas para até 4 jogadores
        private static readonly Color[] playerColors = new Color[]
        {
            Color.red, Color.blue, Color.green, Color.yellow
        };

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            playerInput = GetComponent<PlayerInput>();

            if (floorTilemap == null)
                floorTilemap = GameObject.Find("FloorTilemap").GetComponent<Tilemap>();

            // Define cor do jogador pelo playerIndex
            int index = Mathf.Clamp(playerInput.playerIndex, 0, playerColors.Length - 1);
            playerColor = playerColors[index];

            // Cria tile colorido
            playerTile = ScriptableTile.CreateTile(playerColor);
        }

        // Chamado pelo PlayerInput → Move
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
        }
    }
}