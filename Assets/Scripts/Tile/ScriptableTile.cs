namespace Tile
{
    using UnityEngine;
    using UnityEngine.Tilemaps;

    public class ScriptableTile : Tile
    {
        public static TileBase CreateTile(Color color)
        {
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            Texture2D tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, color);
            tex.Apply();
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), Vector2.zero);
            tile.sprite = sprite;
            return tile;
        }
    }
}