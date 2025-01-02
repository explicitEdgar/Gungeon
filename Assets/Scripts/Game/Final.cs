using QFramework.Gungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Final : Enemy
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>())
        {
            GameUI.Default.gamePass.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
