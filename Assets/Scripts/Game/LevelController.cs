using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.Tilemaps;

public class LevelController : MonoBehaviour
{

    public TileBase Wall0;
    public TileBase Wall1;
    public TileBase Wall2;
    public TileBase Wall3;
    public TileBase Floor0;
    public TileBase Floor1;
    public TileBase Floor2;
    public TileBase Floor3;

    public TileBase Wall
    {
        get
        {
            var wallIndex = Random.Range(0, 3 + 1);

            if (wallIndex == 0)
                return Wall0;
            if (wallIndex == 1)
                return Wall1;
            if (wallIndex == 2)
                return Wall2;
            if (wallIndex == 3)
                return Wall3;

            return Wall0;
        }
    }

    public TileBase Floor
    {
        get
        {
            var floorIndex = Random.Range(0, 3 + 1);

            if (floorIndex == 0)
                return Floor0;
            if (floorIndex == 1)
                return Floor1;
            if (floorIndex == 2)
                return Floor2;
            if (floorIndex == 3)
                return Floor3;

            return Floor0;
        }
    }

    public Tilemap wallMap;

    public Tilemap floorMap;

    public Player player;

    public Enemy enemy;

    public Final final;

    private int currentX = 0;


    /*
        1代表地块
        @代表玩家
        e代表敌人
        #代表出口
    */
    public List<string> initRoom { get; set; } = new List<string>()
    {
        "1111111111",
        "1        1",
        "1        1",
        "1        1",
        "1    @    ",
        "1         ",
        "1        1",
        "1        1",
        "1        1",
        "1111111111"
    };

    public List<string> normalRoom { get; set; } = new List<string>()
    {
        "1111111111",
        "1        1",
        "1    e   1",
        "1        1",
        "          ",
        "     e    ",
        "1        1",
        "1        1",
        "1        1",
        "1111111111"
    };

    public List<string> finalRoom { get; set; } = new List<string>()
    {
        "1111111111",
        "1        1",
        "1        1",
        "1        1",
        "     #   1",
        "         1",
        "1        1",
        "1        1",
        "1        1",
        "1111111111"
    };
    private void Awake()
    {
        player.gameObject.SetActive(false);
        enemy.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateRoom(currentX, initRoom);
        currentX += initRoom.First().Length;
        GenerateRoom(currentX + 2, normalRoom);
        currentX += normalRoom.First().Length;
        GenerateRoom(currentX + 4, finalRoom);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenerateRoom(int currentX,List<string> RoomCode)
    {
        for (int i = 0; i < RoomCode.Count; i++)
        {
            var rowCode = RoomCode[i];
            for (int j = 0; j < rowCode.Length; j++)
            {
                var code = rowCode[j];

                int x = j + currentX;
                int y = RoomCode.Count - i;

                floorMap.SetTile(new Vector3Int(x, y, 0), Floor);

                if (code == '1')
                {
                    wallMap.SetTile(new Vector3Int(x, y, 0), Wall);
                }
                else if (code == '@')
                {
                    var newPlayer = Instantiate(player);
                    newPlayer.transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
                    newPlayer.gameObject.SetActive(true);
                    Global.player = newPlayer;
                }
                else if (code == 'e')
                {
                    var newEnemy = Instantiate(enemy);
                    newEnemy.transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
                    newEnemy.gameObject.SetActive(true);
                }
                else if (code == '#')
                {
                    var newFinal = Instantiate(final);
                    newFinal.transform.position = new Vector3(x, y, 0);
                    newFinal.gameObject.SetActive(true);
                }
            }
        }
    }

}
