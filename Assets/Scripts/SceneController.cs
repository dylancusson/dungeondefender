using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    [SerializeField] Animator transitionAnimation;
    private string changeTo;

    // Add a flag to prevent double-loading
    private bool isTransitioning = false;

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
        TryLoadScene("MenuScene");
    }

    public void NextScene(string nextScene)
    {
        TryLoadScene(nextScene);
    }

    // Centralized function to check the lock
    private void TryLoadScene(string sceneName)
    {
        // If we are already going somewhere, ignore new requests!
        if (isTransitioning) return;

        changeTo = sceneName;
        StartCoroutine(loadNextScene());
    }

    IEnumerator loadNextScene()
    {
        isTransitioning = true; // Lock to prevent transition errors

        // Check if animator is assigned to prevent crashes
        if (transitionAnimation != null)
            transitionAnimation.SetTrigger("outOf");

        yield return new WaitForSeconds(1);

        SceneManager.LoadSceneAsync(changeTo);

        // Wait for the scene to actually finish loading before unlocking? 
        // usually the new scene initializes its own state, so we just trigger the animation.

        if (transitionAnimation != null)
            transitionAnimation.SetTrigger("inTo");

        // Unlock after a small buffer to allow the new scene to settle
        yield return new WaitForSeconds(1f);
        isTransitioning = false;
    }
}