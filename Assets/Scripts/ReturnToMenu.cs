using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnToMenu : MonoBehaviour
{
    [SerializeField] public string ChangeTo;

    public void SwapScene()
    {
        SceneManager.LoadSceneAsync(ChangeTo);
    }

    public void SwapScene(string sceneTarget)
    {
        SceneManager.LoadSceneAsync(sceneTarget);
    }

}
