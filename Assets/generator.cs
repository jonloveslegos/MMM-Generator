using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class generator : MonoBehaviour
{
    public List<TileEntry> tileIds;
    public List<TileDat> tilesPossible;
    public List<SpikeDat> spikesPossible;
    public List<LadderDat> ladderPossible;
    public List<ObjDat> objsPossible;
    public BoundsInt tilemapBounds;
    public Tilemap mainTiles;
    public List<ScreenOption> screenOptions;
    public int screenCount = 10;
    bool Generated = false;
    public float timer = 0.2f;
    public Vector2Int screenSize = new Vector2Int(16, 14);
    public List<Vector2Int> possibleScreens = new List<Vector2Int>();
    public int menuScene;
    [TextArea]
    public string BaseEnd = "2a0,0=\"1.000000\"\n1s=\"3152.000000\"\n1r=\"0.000000\"\n1q=\"3344.000000\"\n1p=\"0.000000\"\n1m=\"8.000000\"\n1l=\"12.000000\"\n1k8=\"33.000000\"\n1k7=\"30.000000\"\n1k6=\"13.000000\"\n1k5=\"17.000000\"\n1k4=\"32.000000\"\n1k3=\"31.000000\"\n1k2=\"82.000000\"\n1k1=\"14.000000\"\n1k0=\"0.000000\"\n1bc=\"0.000000\"\n1f=\"-1.000000\"\n1e=\"29.000000\"\n1d=\"6.000000\"\n1cc=\"1.000000\"\n1cb=\"1.000000\"\n1bb=\"0.000000\"\n1ca=\"0.000000\"\n1ba=\"0.000000\"\n1c=\"1.000000\"\n1b=\"1.000000\"\n4b=\"3.000000\"\n4a=\"Generator\"\n1a=\"Generated\"\n0v=\"1.7.5\"\n0a=\"327493.000000\"";
    // Start is called before the first frame update
    public void GenerateFile()
    {
        mainTiles.RefreshAllTiles();
        tileIds.Clear();
        mainTiles.CompressBounds();
        List<Vector2Int> maps = new List<Vector2Int>();
        for (int x = 0; x <= tilemapBounds.size.x; x++)
        {
            for (int y = 0; y <= tilemapBounds.size.y; y++)
            {
                if (mainTiles.cellBounds.Contains(new Vector3Int((Mathf.RoundToInt((x) / (float)screenSize.x)) * screenSize.x + tilemapBounds.position.x, Mathf.RoundToInt((-y) / (float)screenSize.y) * screenSize.y - tilemapBounds.position.y, 0)))
                {
                    if (mainTiles.GetTile<TileBase>(new Vector3Int(x + tilemapBounds.position.x, -y + -tilemapBounds.position.y, 0)) != null)
                    {
                        if (Mathf.Round((float)x / (float)screenSize.x) == (float)x / (float)screenSize.x && Mathf.Round(((float)y) / (float)screenSize.y) == ((float)y / (float)screenSize.y))
                        {
                            maps.Add(new Vector2Int(x * 16, y * 16));
                        }
                    }
                }
                if (mainTiles.GetTile<TileBase>(new Vector3Int(x + tilemapBounds.position.x, -y + -tilemapBounds.position.y, 0)) != null)
                { 
                    Sprite tileRule = null;
                    TileBase tile = mainTiles.GetTile<TileBase>(new Vector3Int(x + tilemapBounds.position.x, -y + -tilemapBounds.position.y, 0));
                    if (tile != null)
                    {
                        if (mainTiles.GetTile(new Vector3Int(x + tilemapBounds.position.x, -y + -tilemapBounds.position.y, 0)) is RuleTile)
                        {
                            tileRule = mainTiles.GetSprite(new Vector3Int(x + tilemapBounds.position.x, -y + -tilemapBounds.position.y, 0));
                        }
                        else
                        {
                            tileRule = mainTiles.GetTile<Tile>(new Vector3Int(x + tilemapBounds.position.x, -y + -tilemapBounds.position.y, 0)).sprite;
                        }
                    }
                    foreach (var item in tilesPossible)
                    {
                        if (tileRule != null)
                        {
                            if (item.tileBase == tileRule)
                            {
                                TileEntry temp = new TileEntry();
                                temp.posX = x * 16;
                                temp.posY = y * 16;
                                temp.tileId = tilesPossible.IndexOf(item);
                                temp.tileType = TypeOfTile.tile;
                                tileIds.Add(temp);
                            }
                        }
                    }
                    foreach (var item in objsPossible)
                    {
                        if (tileRule != null)
                        {
                            if (item.tileBase == tileRule)
                            {
                                TileEntry temp = new TileEntry();
                                temp.posX = x * 16;
                                temp.posY = y * 16;
                                temp.tileId = objsPossible.IndexOf(item);
                                temp.tileType = TypeOfTile.obj;
                                tileIds.Add(temp);
                            }
                        }
                    }
                    foreach (var item in spikesPossible)
                    {
                        if (tileRule != null)
                        {
                            if (item.tileBase == tileRule)
                            {
                                TileEntry temp = new TileEntry();
                                temp.posX = x * 16;
                                temp.posY = y * 16;
                                temp.tileId = spikesPossible.IndexOf(item);
                                temp.tileType = TypeOfTile.spike;
                                tileIds.Add(temp);
                            }
                        }
                    }
                    foreach (var item in ladderPossible)
                    {
                        if (tileRule != null)
                        {
                            if (item.tileBase == tileRule)
                            {
                                TileEntry temp = new TileEntry();
                                temp.posX = x * 16;
                                temp.posY = y * 16;
                                temp.tileId = ladderPossible.IndexOf(item);
                                temp.tileType = TypeOfTile.ladder;
                                tileIds.Add(temp);
                            }
                        }
                    }

                }
            } 
        }
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.dataPath + "/Generated.mmlv";
        string tempSave = "";

        tempSave = tempSave+"[Level]\n";
        for (int i = tileIds.Count-1; i >= 0 ; i--)
        {
            var item = tileIds[i];
            if (item.tileType == TypeOfTile.tile)
            {
                tempSave = tempSave + tilesPossible[item.tileId].ReturnString(item.posX, item.posY) + "\n";
            }
            else if (item.tileType == TypeOfTile.obj)
            {
                tempSave = tempSave + objsPossible[item.tileId].ReturnString(item.posX, item.posY) + "\n";
            }
            else if (item.tileType == TypeOfTile.spike)
            {
                tempSave = tempSave + spikesPossible[item.tileId].ReturnString(item.posX, item.posY) + "\n";
            }
            else if (item.tileType == TypeOfTile.ladder)
            {
                tempSave = tempSave + ladderPossible[item.tileId].ReturnString(item.posX, item.posY) + "\n";
            }
        }
        tempSave = tempSave + "2b0, 4256 =\"0.000000\"\n2b0,4032=\"0.000000\"\n2b0,3808=\"0.000000\"\n2b0,3584=\"0.000000\"\n2b0,3360=\"0.000000\"\n2b0,3136=\"0.000000\"\n2b0,2912=\"0.000000\"\n2b0,2688=\"0.000000\"\n2b0,2464=\"0.000000\"\n2b0,2240=\"0.000000\"\n2b0,2016=\"0.000000\"\n2b0,1792=\"0.000000\"\n2b0,1568=\"0.000000\"\n2b0,1344=\"0.000000\"\n2b0,1120=\"0.000000\"\n2b0,896=\"0.000000\"\n2b0,672=\"0.000000\"\n2b0,448=\"0.000000\"\n2b0,224=\"0.000000\"\n2b0,0=\"0.000000\"\n";
        foreach (var item in maps)
        {
            tempSave = tempSave + "2a" + item.x + "," + item.y + "=\"1.000000\"" + "\n";
        }
        tempSave = tempSave + BaseEnd;
        if (WebGLFileSaver.IsSavingSupported())
        {
            WebGLFileSaver.SaveFile(tempSave, "Generated.mmlv");
        }
        else
        {
            File.WriteAllText(path, tempSave);
        }
        SceneManager.LoadScene(menuScene);
    }
    public bool spawnScreen(Tilemap mainTilemap, ScreenOption toCopyFrom,Vector2Int offset, ref int placeInd, List<Vector2Int> posScreen, Vector2Int dirFrom)
    {
        GameObject temp = GameObject.Instantiate(toCopyFrom.tilemapPrefab);
        if (toCopyFrom.up && dirFrom.y >= posScreen[placeInd].y)
        {
            if (posScreen.Contains(new Vector2Int(posScreen[placeInd].x, posScreen[placeInd].y- screenSize.y)) == false)
            {
                GameObject.Destroy(temp);
                return false;
            }
        }
        if (toCopyFrom.down && dirFrom.y <= posScreen[placeInd].y)
        {
            if (posScreen.Contains(new Vector2Int(posScreen[placeInd].x, posScreen[placeInd].y + screenSize.y)) == false)
            {
                GameObject.Destroy(temp);
                return false;
            }
        }
        if (toCopyFrom.right && dirFrom.x <= posScreen[placeInd].x)
        {
            if (posScreen.Contains(new Vector2Int(posScreen[placeInd].x+ screenSize.x, posScreen[placeInd].y)) == false)
            {
                GameObject.Destroy(temp);
                return false;
            }
        }
        if (toCopyFrom.left && dirFrom.x >= posScreen[placeInd].x)
        {
            if (posScreen.Contains(new Vector2Int(posScreen[placeInd].x - screenSize.x, posScreen[placeInd].y)) == false)
            {
                GameObject.Destroy(temp);
                return false;
            }
        }
        temp.GetComponentInChildren<Tilemap>().CompressBounds();
        temp.GetComponentInChildren<Tilemap>().size = new Vector3Int(16, 14, 0);
        for (int x = 0; x < temp.GetComponentInChildren<Tilemap>().cellBounds.size.x; x++)
        {
            for (int y = 0; y < temp.GetComponentInChildren<Tilemap>().cellBounds.size.y; y++)
            {
                mainTilemap.SetTile(new Vector3Int(x+offset.x, y- temp.GetComponentInChildren<Tilemap>().cellBounds.size.y+1 - offset.y, 0), temp.GetComponentInChildren<Tilemap>().GetTile(new Vector3Int(x, y- temp.GetComponentInChildren<Tilemap>().cellBounds.size.y+1, 0)));
            }
        }
        GameObject.Destroy(temp);
        return true;
    }
    private void Update()
    {
        if (timer <= 0)
        {
            possibleScreens = new List<Vector2Int>();
            for (int x = 0; x < tilemapBounds.size.x / screenSize.x; x++)
            {
                for (int y = 0; y < tilemapBounds.size.y / screenSize.y; y++)
                {
                    possibleScreens.Add(new Vector2Int(x * screenSize.x + tilemapBounds.x, y * screenSize.y + tilemapBounds.y));
                }
            }
            List<ScreenOption> mains = new List<ScreenOption>();
            List<ScreenOption> bosses = new List<ScreenOption>();
            List<ScreenOption> checkpoints = new List<ScreenOption>();
            List<ScreenOption> starts = new List<ScreenOption>();
            foreach (var item in screenOptions)
            {
                if (item.start == true)
                {
                    starts.Add(item);
                }
                else if (item.checkpoint == true)
                {
                    checkpoints.Add(item);
                }
                else if (item.boss == true)
                {
                    bosses.Add(item);
                }
                else if (item.boss == false && item.checkpoint == false && item.start == false)
                {
                    mains.Add(item);
                }
            }
            mainTiles.ClearAllTiles();
            int toPlace = possibleScreens.IndexOf(new Vector2Int((tilemapBounds.size.x / screenSize.x) / 4 * screenSize.x + tilemapBounds.x, (tilemapBounds.size.y / screenSize.y) / 4 * screenSize.y + tilemapBounds.y));
            Vector2Int pastSpot = new Vector2Int();
            ScreenOption chosen = starts[Random.Range(0, starts.Count)];
            spawnScreen(mainTiles, chosen, possibleScreens[toPlace], ref toPlace, possibleScreens, pastSpot);
            pastSpot = possibleScreens[toPlace];
            Debug.Log(possibleScreens[toPlace]);
            Vector2Int pastVect = possibleScreens[toPlace];
            possibleScreens[toPlace] = new Vector2Int(-100, -100);
            if (chosen.up && pastSpot.y <= pastVect.y)
            {
                toPlace = possibleScreens.IndexOf(new Vector2Int(pastVect.x, pastVect.y - (screenSize.y)));
            }
            else if (chosen.down && pastSpot.y >= pastVect.y)
            {
                toPlace = possibleScreens.IndexOf(new Vector2Int(pastVect.x, pastVect.y + (screenSize.y)));
            }
            else if (chosen.right && pastSpot.x <= pastVect.x)
            {
                toPlace = possibleScreens.IndexOf(new Vector2Int(pastVect.x + (screenSize.x), pastVect.y));
            }
            else if (chosen.left && pastSpot.x >= pastVect.x)
            {
                toPlace = possibleScreens.IndexOf(new Vector2Int(pastVect.x - (screenSize.x), pastVect.y));
            }
            for (int i = 0; i <= ((screenCount)); i++)
            {
                bool spawned = false;
                int repeated = 0;
                while (spawned == false)
                {
                    List<ScreenOption> tempList = new List<ScreenOption>();
                    Debug.Log(toPlace);
                    if (i == screenCount / 2)
                    {
                        foreach (var item in checkpoints)
                        {
                            if (item.up && pastSpot.y < possibleScreens[toPlace].y)
                            {
                                tempList.Add(item);
                            }
                            else if (item.down && pastSpot.y > possibleScreens[toPlace].y)
                            {
                                tempList.Add(item);
                            }
                            else if (item.right && pastSpot.x > possibleScreens[toPlace].x)
                            {
                                tempList.Add(item);
                            }
                            else if (item.left && pastSpot.x < possibleScreens[toPlace].x)
                            {
                                tempList.Add(item);
                            }
                        }
                    }
                    else if (i == screenCount)
                    {
                        foreach (var item in bosses)
                        {
                            if (item.up && pastSpot.y < possibleScreens[toPlace].y)
                            {
                                tempList.Add(item);
                            }
                            else if (item.down && pastSpot.y > possibleScreens[toPlace].y)
                            {
                                tempList.Add(item);
                            }
                            else if (item.right && pastSpot.x > possibleScreens[toPlace].x)
                            {
                                tempList.Add(item);
                            }
                            else if (item.left && pastSpot.x < possibleScreens[toPlace].x)
                            {
                                tempList.Add(item);
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in mains)
                        {
                            if (item.up && pastSpot.y < possibleScreens[toPlace].y)
                            {
                                tempList.Add(item);
                            }
                            else if (item.down && pastSpot.y > possibleScreens[toPlace].y)
                            {
                                tempList.Add(item);
                            }
                            else if (item.right && pastSpot.x > possibleScreens[toPlace].x)
                            {
                                tempList.Add(item);
                            }
                            else if (item.left && pastSpot.x < possibleScreens[toPlace].x)
                            {
                                tempList.Add(item);
                            }
                        }
                    }
                    if (tempList.Count > 0)
                    {
                        chosen = tempList[Random.Range(0, tempList.Count)];
                        spawned = spawnScreen(mainTiles, chosen, possibleScreens[toPlace], ref toPlace, possibleScreens, pastSpot);
                    }
                    else
                    {
                        spawned = false;
                    }
                    repeated++;
                    if (repeated > 30)
                    {
                        break;
                    }
                }
                if (repeated <= 30)
                {
                    pastVect = possibleScreens[toPlace];
                    possibleScreens[toPlace] = new Vector2Int(-100, -100);
                    if (chosen.down && pastSpot.y > pastVect.y)
                    {
                        if (chosen.up)
                        {
                            toPlace = possibleScreens.IndexOf(new Vector2Int(pastVect.x, pastVect.y - (screenSize.y)));
                        }
                        else if (chosen.right)
                        {
                            toPlace = possibleScreens.IndexOf(new Vector2Int(pastVect.x + (screenSize.x), pastVect.y));
                        }
                        else if (chosen.left)
                        {
                            toPlace = possibleScreens.IndexOf(new Vector2Int(pastVect.x - (screenSize.x), pastVect.y));
                        }
                    }
                    else if (chosen.up && pastSpot.y < pastVect.y)
                    {
                        if (chosen.right)
                        {
                            toPlace = possibleScreens.IndexOf(new Vector2Int(pastVect.x + (screenSize.x), pastVect.y));
                        }
                        else if (chosen.left)
                        {
                            toPlace = possibleScreens.IndexOf(new Vector2Int(pastVect.x - (screenSize.x), pastVect.y));
                        }
                        else if (chosen.down)
                        {
                            toPlace = possibleScreens.IndexOf(new Vector2Int(pastVect.x, pastVect.y + (screenSize.y)));
                        }
                    }
                    else if (chosen.left && pastSpot.x < pastVect.x)
                    {
                        if (chosen.up)
                        {
                            toPlace = possibleScreens.IndexOf(new Vector2Int(pastVect.x, pastVect.y - (screenSize.y)));
                        }
                        else if (chosen.right)
                        {
                            toPlace = possibleScreens.IndexOf(new Vector2Int(pastVect.x + (screenSize.x), pastVect.y));
                        }
                        else if (chosen.down)
                        {
                            toPlace = possibleScreens.IndexOf(new Vector2Int(pastVect.x, pastVect.y + (screenSize.y)));
                        }
                    }
                    else if (chosen.right && pastSpot.x > pastVect.x)
                    {
                        if (chosen.up)
                        {
                            toPlace = possibleScreens.IndexOf(new Vector2Int(pastVect.x, pastVect.y - (screenSize.y)));
                        }
                        else if (chosen.left)
                        {
                            toPlace = possibleScreens.IndexOf(new Vector2Int(pastVect.x - (screenSize.x), pastVect.y));
                        }
                        else if (chosen.down)
                        {
                            toPlace = possibleScreens.IndexOf(new Vector2Int(pastVect.x, pastVect.y + (screenSize.y)));
                        }
                    }
                    pastSpot = pastVect;
                    Debug.Log(pastVect);
                }
                else
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
            mainTiles.CompressBounds();
        }
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (timer <= 0)
        {
            if (Generated == false)
            {
                Generated = true;
                GenerateFile();
            }
        }
        timer = 0;
    }
}
[System.Serializable]
public struct ScreenOption
{
    public GameObject tilemapPrefab;
    public bool up;
    public bool down;
    public bool left;
    public bool right;
    public bool boss;
    public bool checkpoint;
    public bool start;
}

[System.Serializable]
public struct TileDat
{
    public Sprite tileBase;
    public int tileId;
    public int tileOffset;
    public string ReturnString(int x,int y)
    {
        string posString = x + "," + y;
        return "k"+ posString+"=\"" + tileId + ".000000\"\nj" + posString + "=\""+tileOffset+".000000\"\ni" + posString + "=\"1.000000\"\ne" + posString + "=\"3.000000\"\na" + posString + "=\"1.000000\"";
    }
}
[System.Serializable]
public struct LadderDat
{
    public Sprite tileBase;
    public int tileId;
    public int tileOffset;
    public string ReturnString(int x, int y)
    {
        string posString = x + "," + y;
        return "i" + posString + "=\"" + tileId + ".000000\"\ne" + posString + "=\"" + tileOffset + ".000000\"\na" + posString + "=\"1.000000\"";
    }
}
[System.Serializable]
public struct ObjDat
{
    public Sprite tileBase;
    [Range(4,8)]
    public int objId;
    public int enemyId;
    [Range(-1,1)]
    public int direction;
    [Range(0,3)]
    public int moveDir;
    [Range(0,1)]
    public int isPlayer;
    public string ReturnString(int x, int y)
    {
        string posString = x + "," + y;
        return "o" + posString + " = \"9999.000000\"\nd" + posString + " = \""+ objId + ".000000\"\ne" + posString + " = \"" + enemyId + ".000000\"\ng" + posString + " = \"" + (moveDir*90) + ".000000\"\nb" + posString + " = \""+ direction + ".000000\"\na" + posString + " = \"1.000000\"";
    }
}
[System.Serializable]
public struct SpikeDat
{
    public Sprite tileBase;
    public int objId;
    public int dir;
    public string ReturnString(int x, int y)
    {
        string posString = x + "," + y;
        return "l" + posString + " = \""+ dir+".000000\"\ni" + posString + " = \"2.000000\"\ne" + posString + " = \""+objId+".000000\"\na" + posString + " = \"1.000000\"";
    }
}
[System.Serializable]
public enum TypeOfTile
{
    tile,
    obj,
    spike,
    ladder
}
[System.Serializable]
public struct TileEntry
{
    public int posX;
    public int posY;
    public int tileId;
    public TypeOfTile tileType;
}