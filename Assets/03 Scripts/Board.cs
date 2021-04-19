using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    //private Tile[,] m_TileArray = new Tile[7,7];
    private Dictionary<string, Tile> m_TileDictonary = new Dictionary<string, Tile>();
    public int m_indexX = 7;
    public int m_indexY = 7;
    private GameObject m_tilePrefab0;
    private GameObject m_tilePrefab1;
    private GameObject m_tilePrefab2;
    //public Tile[,] TileArray { get => m_TileArray; set => m_TileArray = value; }

    void Start()
    {
        m_tilePrefab0 = Resources.Load("Prefabs/Black_Cat") as GameObject;
        m_tilePrefab1 = Resources.Load("Prefabs/UnityChan") as GameObject;
        m_tilePrefab2 = Resources.Load("Prefabs/Misaki") as GameObject;

        //Tile tile_0 = Instantiate<Tile>(m_tilePrefab1.transform.GetComponent<Tile>());
        //Tile tile_0 = Instantiate<Tile>(m_black_Cat_Prefab.transform.GetComponent<Tile>(), new Vector3(1f,1f,1f), Quaternion.identity);
        //tile_0.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        //tile_0.transform.position = new Vector3(1,1,1);

        CreatTile();
    }

    private void CreatTile()
    {
        for (int y = 0; y < m_indexY; ++y)
        {
            for (int x = 0; x < m_indexX; ++x)
            {

                string key = x.ToString() + "," + y.ToString();
                int randomTile = Random.Range(0, 1);
                Tile tile_0;
                if (randomTile == 0)
                {
                    tile_0 = Instantiate<Tile>(m_tilePrefab1.transform.GetComponent<Tile>());
                }
                else 
                {
                    tile_0 = Instantiate<Tile>(m_tilePrefab2.transform.GetComponent<Tile>());
                }
                //tile_0.transform.position = Vector3.zero;
                //tile_0.transform.SetParent(this.transform);
                m_TileDictonary.Add(key, tile_0);
                tile_0.transform.localScale = new Vector3(3f, 3f, 3f);
                tile_0.transform.position = new Vector3(x * 2, y * 2);
            }
        }
    }
}
