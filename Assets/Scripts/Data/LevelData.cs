using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/ DataLevel",fileName = "DataLevel")]
public class LevelData : ScriptableObject
{
    public int sizeGridX;
    public int sizeGridY;
    public int live;
    public List<Tile> prefabTiles;
    public Vector2 startPoint;
    public Vector2 endPoint;
    public int[] Datas;
    public List<Vector2> posEnemy;
}
