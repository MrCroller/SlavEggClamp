using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionMenu;

    public bool OptionOpen
    {
        set
        {
            mainMenu.SetActive(!value);
            optionMenu.SetActive(value);
        }
    }

    private void Start()
    {
        mainMenu.SetActive(true);
        optionMenu.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
