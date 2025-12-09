using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    [SerializeField] Animator transitionAnimation;
    private string changeTo;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void menuScene()
    {
        changeTo = "MenuScene";
        StartCoroutine(loadNextScene());
    }

    public void NextScene(string nextScene)
    {
        changeTo = nextScene;
        StartCoroutine(loadNextScene());
    }

    IEnumerator loadNextScene()
    {
        transitionAnimation.SetTrigger("outOf");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(changeTo);
        transitionAnimation.SetTrigger("inTo");
    }
}
