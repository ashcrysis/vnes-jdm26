namespace Data
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    public class TilePaintManager : MonoBehaviour
    {
        public static TilePaintManager Instance;

        private Dictionary<Vector3Int, int> tileOwners = new();
        private Dictionary<int, int> playerScores = new();

        void Awake()
        {
            Instance = this;
        }

        public void PaintTile(Vector3Int pos, int playerId)
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
            if (!playerScores.ContainsKey(playerId))
                playerScores[playerId] = 0;

            playerScores[playerId]++;
        }

        public int GetScore(int playerId)
        {
            return playerScores.TryGetValue(playerId, out int score) ? score : 0;
        }
    }
}