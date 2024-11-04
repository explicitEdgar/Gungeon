using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI Default;

    public GameObject gamePass;

    public GameObject gameOver;

    private void Awake()
    {
        Default = this;
    }

    private void OnDestroy()
    {
        Default = null;
    }
    // Start is called before the first frame update
    void Start()
    {
        gamePass.transform.Find("RestartBtn").GetComponent<Button>()
            .onClick.AddListener(() =>
            {
                SceneManager.LoadScene("SampleScene");
                Time.timeScale = 1;
            });

        gameOver.transform.Find("RestartBtn").GetComponent<Button>()
            .onClick.AddListener(() =>
            {
                SceneManager.LoadScene("SampleScene");
                Time.timeScale = 1;
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
