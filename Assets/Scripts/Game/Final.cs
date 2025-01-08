using QFramework.Gungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Final : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>())
        {   
            if(Global.NextLevel())
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                GameUI.Default.gamePass.SetActive(true);
                Global.UIOpened = true;
                Time.timeScale = 0;
            }
        }
    }
}
