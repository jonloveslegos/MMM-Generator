using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelEditorScript : MonoBehaviour
{
    public DownloadLevelScript downloadScr;
    public Tilemap mainTiles;
    public List<TileBase> tilesRef = new List<TileBase>();
    public Vector2Int screenSize = new Vector2Int(16, 14);
    public GameObject tilePlacer;
    public RuleTile currTile;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tilePlacer.GetComponent<SpriteRenderer>().sprite = currTile.m_DefaultSprite;
        tilePlacer.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tilePlacer.transform.position = mainTiles.CellToWorld(mainTiles.WorldToCell(tilePlacer.transform.position));
        tilePlacer.transform.position += new Vector3(0.08f, 0.08f, 0);
        if (Input.GetMouseButton(0))
        {
            if (mainTiles.WorldToCell(tilePlacer.transform.position).x >= 0 && mainTiles.WorldToCell(tilePlacer.transform.position).x < screenSize.x)
            {
                if (mainTiles.WorldToCell(tilePlacer.transform.position).y <= 0 && mainTiles.WorldToCell(tilePlacer.transform.position).y > -screenSize.y)
                {
                    mainTiles.SetTile(mainTiles.WorldToCell(tilePlacer.transform.position), currTile);
                }
            }
        }
    }
    public void SaveLevel(string name)
    {
        string tempStr = "";
        for (int y = 0; y <= -screenSize.y; y++)
        {
            for (int x = 0; x <= screenSize.x; x++)
            {
                tempStr = tempStr+(tilesRef.IndexOf(mainTiles.GetTile(new Vector3Int(x, -y, 0)))+10);
            }
        }
        downloadScr.ExportItem(name,tempStr);
    }
}

public struct TileRef
{
    public TileBase tile;
    [Range(0,89)]
    public int id;
}