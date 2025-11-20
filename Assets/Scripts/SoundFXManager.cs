using UnityEngine;

public class SoundFXManager : MonoBehaviour
{

    public static SoundFXManager Instance;

    [SerializeField] private AudioSource soundFXobject;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void playSoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXobject, spawnTransform.position, Quaternion.identity) ;

        audioSource.clip = audioClip ;

        audioSource.volume = volume ;

        audioSource.Play();

        float clipLength = audioSource.clip.length ;

        Destroy(audioSource.gameObject, clipLength ) ;
    }

}
