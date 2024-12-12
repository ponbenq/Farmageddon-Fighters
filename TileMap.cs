using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanaNita.MonoGameTnt;
using Microsoft.Xna.Framework;

namespace GameProject
{
    public class TileMap : Actor
    {
        public delegate Actor CreateTileDelegate(int tileCode);
        int[,] tileArray;
        CreateTileDelegate createTile;

        public bool ShowGrid { get => showGrid; set { showGrid = value; UpdateTileArray(); } }
        bool showGrid = false;

        public Color GridColor { get => gridColor; set { gridColor = value; UpdateTileArray(); } }
        Color gridColor = Color.Blue;

        public Color BgColor { get => bgColor; set { bgColor = value; UpdateTileArray(); } }
        Color bgColor = Color.Transparent;

        public Vector2 TileSize { get; private set; }
        public Vector2i Count2d
        {
            get
            {
                return new Vector2i(tileArray.GetLength(1), tileArray.GetLength(0));
            }
        }
        public TileMap(Vector2 tileSize, int[,] tileArray, CreateTileDelegate createTile)
            : this(tileSize, tileArray, createTile, false)
        {

        }
        public TileMap(Vector2 size, int[,] tileArray, CreateTileDelegate createTile, bool showGrid)
        {
            TileSize = size;
            this.tileArray = tileArray;
            this.createTile = createTile;
            this.showGrid = showGrid;
            CreateTileMap();
        }
        public void CreateTileMap()
        {
            AddBackground();
            for (int y = 0; y < tileArray.GetLength(0); ++y)
                for (int x = 0; x < tileArray.GetLength(1); ++x)
                {
                    var tile = createTile(tileArray[y, x]);
                    tile.Position = new Vector2(x * TileSize.X, y * TileSize.Y) + TileSize / 2;
                    this.Add(tile);
                }

            if (showGrid)
                AddGrid();
        }

        private void AddBackground()
        {
            var rect = new RectangleActor(BgColor, Count2d * TileSize);
            Add(rect);
        }

        private void AddGrid()
        {
            Add(CreateGrid(gridColor));
        }

        public int[,] GetAllTiles()
        {
            return tileArray;
        }

        public void SetAllTiles(int[,] newArray)
        {
            for (int y = 0; y < tileArray.GetLength(0); ++y)
                for (int x = 0; x < tileArray.GetLength(1); ++x)
                    tileArray[y, x] = newArray[y, x];
            UpdateTileArray();
        }

        public int GetTile(Vector2i index)
        {
            return tileArray[index.Y, index.X];
        }
        public void SetTile(Vector2i index, int tileCode)
        {
            tileArray[index.Y, index.X] = tileCode;
            UpdateTileArray();
        }

        // Preset Tile จะยังไม่เรียก UpdateTileArray() ให้ เผื่อผู้ใช้งานต้องการ preset หลายตำแหน่ง
        public void PresetTile(Vector2i index, int tileCode)
        {
            tileArray[index.Y, index.X] = tileCode;
        }

        public void UpdateTileArray()
        {
            this.Clear();
            CreateTileMap();
        }

        public bool IsInside(Vector2i index)
        {
            return !IsOutside(index);
        }

        public bool IsOutside(Vector2i index)
        {
            return (index.X < 0 || index.Y < 0 ||
                                index.X >= tileArray.GetLength(1) ||
                                index.Y >= tileArray.GetLength(0));
        }

        public Vector2i CalcIndex(Vector2 position, Vector2 direction)
        {
            return CalcIndex(position) + (Vector2i)direction;
        }
        public Vector2i CalcIndex(Vector2 position)
        {
            Vector2 index = (position - TileSize / 2) / TileSize;
            return new Vector2i((int)MathF.Round(index.X), (int)MathF.Round(index.Y));
        }
        public Vector2 TileCenter(Vector2 position, Vector2 direction)
        {
            return TileCenter(CalcIndex(position, direction));
        }

        public int GetTileCode(Vector2i index)
        {
            if (IsOutside(index))
                return -1;
            return tileArray[index.Y, index.X];
        }

        public void ReplaceAllTiles(int originalTileCode, int newTileCode)
        {
            for (int y = 0; y < Count2d.Y; y++)
                for (int x = 0; x < Count2d.X; x++)
                {
                    var index = new Vector2i(x, y);
                    if (this.GetTile(index) == originalTileCode)
                        this.PresetTile(index, newTileCode);
                }
        }

        private Grid CreateGrid(Color color)
        {
            return new Grid(TileSize, Count2d.X, Count2d.Y, color);
        }

        public Vector2 TileCenter(Vector2i index)
        {
            return index * TileSize + TileSize / 2;
        }

        public Vector2 RightBottomCorner(Vector2i index)
        {
            return index * TileSize + TileSize;
        }
    }
}
