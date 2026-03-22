using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tiles
{
    public class ScriptableTile : Tile
    {
        public static TileBase CreateTile(Color color, Tilemap tilemap)
        {
            Vector3 cellSize = tilemap.layoutGrid.cellSize;

            int texWidth = Mathf.CeilToInt(cellSize.x * 100); 
            int texHeight = Mathf.CeilToInt(cellSize.y * 100);

            Tile tile = ScriptableObject.CreateInstance<Tile>();

            Texture2D tex = new Texture2D(texWidth, texHeight);
            Color[] pixels = new Color[texWidth * texHeight];

            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = color;

            tex.SetPixels(pixels);
            tex.Apply();

            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, texWidth, texHeight), new Vector2(0.5f, 0.5f), 100f);
            tile.sprite = sprite;

            return tile;
        }
    }
}