using Unity.Mathematics;
using UnityEngine;


public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    public Tile[,] grid;
    [HideInInspector]public float tileSize = 1.0f;
    public LevelData levelData;
    protected void Awake()
    {
        Instance = this;
          /*Camera.main.orthographicSize = Mathf.Max(levelData.sizeGridX, levelData.sizeGridY);
                Vector3 cameraPos = Camera.main.transform.position;
                Camera.main.transform.position = cameraPos;*/
        
    }
    
    public void InitializeGrid(LevelData level)
    {
        DestroyBo();
        levelData = level;
        grid = new Tile[levelData.sizeGridX, levelData.sizeGridY];
        for (int x = 0; x < levelData.sizeGridX; x++)
        {
            for (int y = 0; y < levelData.sizeGridY; y++)
            {
                int tileIndex = levelData.Datas[x * levelData.sizeGridY + y] % 10;
                Vector3 pos = new Vector3(x * tileSize, y * tileSize, 0);
                Tile tile = PoolingManager.Spawn(levelData.prefabTiles[tileIndex], pos, quaternion.identity);
                tile.transform.name = $"{x},{y}";
                tile.transform.parent = transform;
                tile.InitTile(levelData.Datas[x * levelData.sizeGridY + y]);
                grid[x, y] = tile;
            }
        }

        UIGame.Instance.txtLive.text = levelData.live.ToString();
        UpdateAllTileNeighbors();
    }
    private void UpdateAllTileNeighbors()
    {
        var tilesCopy = grid.Clone() as Tile[,]; // Cloning the grid to avoid modifying during iteration.
        if (tilesCopy != null)
            foreach (var tile in tilesCopy)
            {
                tile.UpdateAllNeighbors();
            }
    }

    private void DestroyBo()
    {
        if (grid != null)
        {
            foreach (var obj in grid)
            {
                PoolingManager.Despawn(obj.gameObject);
            }   
        }
    }


    public Tile GetTileInGrid(int x,int y)
    {
        if (x < levelData.sizeGridX && x > -1 && y > -1 && y < levelData.sizeGridY)
        {
            return grid[x, y];
        }

        return null;
    }
    
    
}