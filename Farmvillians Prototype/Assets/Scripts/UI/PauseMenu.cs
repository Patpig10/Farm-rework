using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeInHierarchy)
                pauseMenu.SetActive(false);
            else
                pauseMenu.SetActive(true);
        }
        
    }

    public void Reload()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());

    }

    public void ExitGame()
    {

        Application.Quit();

    }
}
