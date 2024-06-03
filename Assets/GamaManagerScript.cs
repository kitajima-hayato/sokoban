using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;



public class GameManagerScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject goalPrefab;
    public GameObject particlePrefab;
    public GameObject OzenPrefab;

    int[,] map;
    GameObject[,] field;
    public GameObject clearText;

    // Start is called before the first frame update
    //int[] map;

    private Vector2Int GetplayerIndex()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                if (field[y, x] == null)
                {
                    continue;
                }
                if (field[y, x].tag == "Player")
                {
                    return new Vector2Int(x, y);
                }

            }

        }
        return new Vector2Int(-1, -1);
    }

    bool MoveNumber(Vector2Int moveFrom, Vector2Int moveTo)
    {
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }




        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            Vector2Int velocity = moveTo - moveFrom;
            bool success = MoveNumber(moveTo, moveTo + velocity);
            if (!success) { return false; }

        }

        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        Vector3 moveToPosition = new Vector3(
            moveTo.x, map.GetLength(0) - moveTo.y, 0);


        field[moveFrom.y, moveFrom.x].GetComponent<Move>().MoveTo(moveToPosition);
        field[moveFrom.y, moveFrom.x] = null;

        for (int i = 0; i < 5; ++i)
        {
            field[moveFrom.y, moveFrom.x] = Instantiate(particlePrefab,
           new Vector3(moveFrom.x,  moveFrom.y, 0.0f), Quaternion.identity);
        }
       

        return true;
    }

    bool IsCleard()
    {
        //Vector2IntŒ^‚Ì‰Â•Ï’·”z—ñ‚Ìì¬
        List<Vector2Int> goals = new List<Vector2Int>();
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 3)
                {
                    //Ši”[êŠ‚ÌƒCƒ“ƒfƒbƒNƒX‚ðT‚¦‚Ä‚¨‚­
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }

        for (int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if (f == null || f.tag != "Box")
            {
                return false;
            }
        }
        return true;
    }

    void Start()
    {
        Screen.SetResolution(1280,720,false);


        map = new int[,] {
        { 4, 0, 3, 0, 0, },
        { 4, 0, 2, 0, 0, },
        { 4, 0, 1, 2, 3, },
        { 4, 0, 2, 0, 0, },
        { 4, 0, 3, 0, 0, },
        };
        field = new GameObject[map.GetLength(0), map.GetLength(1)];

        string debugText = "";
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {

                if (map[y, x] == 1)
                {
                    //GameObject insetance = 
                    field[y, x] = Instantiate(playerPrefab,
                        new Vector3(x, map.GetLength(0) - y, 0.0f),
                        Quaternion.identity);
                }
                if (map[y, x] == 2)
                {
                    //GameObject insetance = 
                    field[y, x] = Instantiate(boxPrefab,
                        new Vector3(x, map.GetLength(0) - y, 0.0f),
                        Quaternion.identity);
                }
                if (map[y, x] == 3)
                {
                    field[y, x] = Instantiate(goalPrefab, new Vector3(x, map.GetLength(0) - y, 0.0f),
                        Quaternion.identity);
                }
                if (map[y, x] == 4)
                {
                    field[y, x] = Instantiate(OzenPrefab, new Vector3(x, map.GetLength(0) - y, 0.0f),
                        Quaternion.identity);
                }
                debugText += map[y, x].ToString() + ",";
            }
            debugText += "\n";
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            Vector2Int playerIndex = GetplayerIndex();
            MoveNumber(playerIndex, playerIndex + new Vector2Int(1, 0));
            if (IsCleard())
            {
                Debug.Log("clear");
                clearText.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            Vector2Int playerIndex = GetplayerIndex();
            MoveNumber(playerIndex, playerIndex - new Vector2Int(1, 0));
            if (IsCleard())
            {
                Debug.Log("clear");
                clearText.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            Vector2Int playerIndex = GetplayerIndex();
            MoveNumber(playerIndex, playerIndex - new Vector2Int(0, 1));
            if (IsCleard())
            {
                Debug.Log("clear");
                clearText.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            Vector2Int playerIndex = GetplayerIndex();
            MoveNumber(playerIndex, playerIndex + new Vector2Int(0, 1));
            if (IsCleard())
            {
                Debug.Log("clear");
                clearText.SetActive(true);
            }
        }


    }
}


