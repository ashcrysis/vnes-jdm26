using System;

namespace Data
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    public class TilePaintManager : MonoBehaviour
    {
        public static TilePaintManager Instance;

        private readonly Dictionary<Vector3Int, bool> _isPaintable = new();
        [SerializeField] private Tilemap paintableTilemap; // define which tiles can be painted or not;
        [SerializeField] private Tilemap paintedTilemap; // which tilemap will actually be painted;

        public Tilemap PaintedTilemap => paintedTilemap;
        
        private Dictionary<Vector3Int, int> tileOwners = new();
        private Dictionary<int, int> playerScores = new();
        
        [SerializeField] private  BoundsInt bounds;

        void Awake()
        {

            paintableTilemap.color = new Color(0, 0, 0, 0);
            Instance = this;
            
            // get all paintable tiles
            Vector3Int start = new Vector3Int(bounds.xMin, bounds.yMin, Int32.MinValue);
            Vector3Int end = new Vector3Int(bounds.xMax, bounds.yMax, Int32.MaxValue);

            int count = paintableTilemap.GetTilesRangeCount(start, end);
            Vector3Int[] positions = new Vector3Int[count];
            TileBase[] tiles = new TileBase[count];
            count = paintableTilemap.GetTilesRangeNonAlloc(start, end, positions, tiles);
            
            for (int i = 0; i < count; i++)
            {
                _isPaintable.TryAdd(positions[i], true);
            }
        }
        
        public void TryPaint(Vector2 pos, TileBase tile, int playerId)
        {
            Vector3Int cellPos = PaintedTilemap.WorldToCell(pos);

            if (!IsPaintable(cellPos))
            {
                Debug.Log("Not a paintable cell");
                return;
            }
            
            PaintedTilemap.SetTile(cellPos, tile);
            OnPaintTile(cellPos, playerId);
        }

        private void OnPaintTile(Vector3Int pos, int playerId)
        {
            // Já foi pintado?
            if (tileOwners.TryGetValue(pos, out int previousOwner))
            {
                if (previousOwner == playerId)
                    return; // nada muda

                // remove ponto do antigo dono
                playerScores[previousOwner]--;
            }

            // define novo dono
            tileOwners[pos] = playerId;

            // adiciona ponto pro jogador atual
            playerScores.TryAdd(playerId, 0);

            playerScores[playerId]++;
        }

        public int GetScore(int playerId)
        {
            return playerScores.GetValueOrDefault(playerId, 0);
        }

        private bool IsPaintable(Vector3Int pos)
        {
            return _isPaintable.GetValueOrDefault(pos, false);
        }
    }
}