using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelController : MonoBehaviour
{

    public TileBase groundTile;

    public Tilemap groundTileMap;

    public Player player;

    public Enemy enemy;


    /*
        1代表地块
        @代表玩家
        e代表敌人
    */
    public List<string> InitRoom { get; set; } = new List<string>()
    {
        "1111111111",
        "1        1",
        "1    @   1",
        "1        1",
        "1        1",
        "1    e   1",
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
        for(int i = 0;i < InitRoom.Count; i++)
        {
            var rowCode = InitRoom[i];
            for(int j = 0;j < rowCode.Length; j++)
            {
                var code = rowCode[j];

                int x = j;
                int y = InitRoom.Count - i;

                if(code == '1')
                {
                    groundTileMap.SetTile(new Vector3Int(x, y, 0), groundTile);
                }
                else if(code == '@')
                {
                    var newPlayer = Instantiate(player);
                    newPlayer.transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
                    newPlayer.gameObject.SetActive(true);
                    Global.player = newPlayer;
                }
                else if(code == 'e')
                {
                    var newEnemy = Instantiate(enemy);
                    newEnemy.transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
                    newEnemy.gameObject.SetActive(true);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
