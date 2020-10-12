using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public static bool isPaused = false;
    [SerializeField] public GameObject pauseUI;
    private string currScene;
    private Scene thisScene;


    private void Start()
    {
        pauseUI.SetActive(false);
    }
    void Update()
    {
        thisScene = SceneManager.GetActiveScene();
        if (thisScene.name != "Main Menu")
        {
            //uses the esc button to pause and unpause the game
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!isPaused)
                {
                    Pause();
                }
                else
                {
                    Resume();
                }
            }
        }
        else if (thisScene.name == "Main Menu")
            pauseUI.SetActive(false);
    }
    public void Pause()
    {


        Time.timeScale = 0f;
        pauseUI.SetActive(true);


    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseUI.SetActive(false);

    }
   
    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MenuQuit()
    {
        SceneManager.LoadScene("Main Menu");
    }

}

