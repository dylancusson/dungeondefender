using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnToMenu : MonoBehaviour
{
    public string ChangeTo;

    public void SwapScene()
    {
        SceneManager.LoadSceneAsync(ChangeTo);
    }

}
