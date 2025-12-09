//using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.Windows;

public class Pause : MonoBehaviour
{
    [Header("What menus to show")]
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject TutorialMenu;
    [Header("Should the tutorial be shown?")]
    [SerializeField] bool showTutorial;

    private bool tutorialOpen = false;

    public void Start()
    {
        if (showTutorial)
        {
            tutorialOpen = true;
            TutorialMenu.SetActive(true);
        }
    }

    public void closeTutorial()
    {
        tutorialOpen = false;
        TutorialMenu.SetActive(false);
    }

    public void Update()
    {
        if (tutorialOpen && Input.GetKeyDown(KeyCode.Escape) )
        {
            tutorialOpen = false;
            TutorialMenu.SetActive(false);
            Time.timeScale = 1f;
        }

        else if (Input.GetKeyDown(KeyCode.Escape) && (Time.timeScale != 0))
        {
            PauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }

        else if((Input.GetKeyDown(KeyCode.Escape)) && (Time.timeScale == 0))
        {
            PauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void QuitFromPause()
    {
        Time.timeScale = 1f;
        SceneController.instance.menuScene();
    }
}
