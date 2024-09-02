using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public float hCost;
    public float gCost;
    public float FCost=>hCost+gCost;
    public Tile parent;
    public List<Tile> Neighbors = new List<Tile>();
    
    
    [SerializeField] private bool[] connections = new bool[4]; // Up, Down, Left, Right
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();
    public int index = 0;
    private SpriteRenderer spriteRenderer;
    public TileType tileType;
    private List<Tile> oldNeighbors;
    private int rotation = 90;

    public void InitTile(int id)
    {
        index = id/10;
        index = Random.Range(0, sprites.Count);
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[index];
        UpdateConnections();
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.isPlay)
        {
            RotateTile();
            UpdateAllNeighbors();
            EventDispatcher.Instance.PostEvent(EventID.UpdateLive);
            MusicManager.Instance.PlaySFX(MusicManager.Instance.musicClickGround);
        }
        EventDispatcher.Instance.PostEvent(EventID.OnUpdatePath);
    }

    private void RotateTile()
    {
        index = (index + 1) % sprites.Count;
        spriteRenderer.sprite = sprites[index];
        UpdateConnections();
    }


    private void UpdateConnections()
    {
        ResetConnections();

        switch (tileType)
        {
            case TileType.FourWay:
                SetAllConnections(true);
                break;

            case TileType.ThreeWay:
                SetThreeWayConnections();
                break;

            case TileType.TwoWay:
                SetTwoWayConnections();
                break;

            case TileType.TwoWayAlt:
                SetTwoWayAltConnections();
                break;

            default:
                Debug.LogWarning("Unrecognized TileType");
                break;
        }
    }

    private void ResetConnections()
    {
        for (int i = 0; i < connections.Length; i++)
        {
            connections[i] = false;
        }
    }

    private void SetAllConnections(bool state)
    {
        for (int i = 0; i < connections.Length; i++)
        {
            connections[i] = state;
        }
    }

    private void SetThreeWayConnections()
    {
        switch (index)
        {
            case 0:
                connections[1] = connections[2] = connections[3] = true;
                break;
            case 1:
                connections[0] = connections[1] = connections[2] = true;
                break;
            case 2:
                connections[0] = connections[2] = connections[3] = true;
                break;
            case 3:
                connections[0] = connections[1] = connections[3] = true;
                break;
            default:
                Debug.LogError("Invalid index for ThreeWay connections");
                break;
        }
    }

    private void SetTwoWayConnections()
    {
        switch (index)
        {
            case 0:
                connections[0] = connections[1] = true;
                break;
            case 1:
                connections[2] = connections[3] = true;
                break;
            default:
                Debug.LogError("Invalid index for TwoWay connections");
                break;
        }
    }

    private void SetTwoWayAltConnections()
    {
        switch (index)
        {
            case 0:
                connections[1] = connections[3] = true;
                break;
            case 1:
                connections[1] = connections[2] = true;
                break;
            case 2:
                connections[0] = connections[2] = true;
                break;
            case 3:
                connections[0] = connections[3] = true;
                break;
            default:
                Debug.LogError("Invalid index for TwoWayAlt connections");
                break;
        }
    }

    private void UpdateNeighbors()
    {
        oldNeighbors = new List<Tile>(Neighbors);
        Neighbors.Clear();
        int x = (int)transform.position.x;
        int y = (int)transform.position.y;

        CheckAndAddNeighbor(x, y + 1, 0, 1); // Up
        CheckAndAddNeighbor(x, y - 1, 1, 0); // Down
        CheckAndAddNeighbor(x - 1, y, 2, 3); // Left
        CheckAndAddNeighbor(x + 1, y, 3, 2); // Right
    }

    private void CheckAndAddNeighbor(int x, int y, int connectionIndex, int oppositeConnectionIndex)
    {
        Tile neighborTile = GridManager.Instance.GetTileInGrid(x, y);
        if (neighborTile != null && connections[connectionIndex] && neighborTile.connections[oppositeConnectionIndex])
        {
            Neighbors.Add(neighborTile);
        }
    }

    public void UpdateAllNeighbors()
    {
        UpdateNeighbors();
        List<Tile> currentNeighbor = new List<Tile>(Neighbors);
        foreach (var neighbor in currentNeighbor)
        {
            neighbor.UpdateNeighbors();
        } 
        foreach (var neighbor in oldNeighbors)
        {
            neighbor.UpdateNeighbors();
        }   
    }

    public enum TileType
    {
        TwoWay, TwoWayAlt, ThreeWay, FourWay, Block,
    }
}
