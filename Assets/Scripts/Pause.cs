//using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.Windows;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (Time.timeScale != 0))
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
}
