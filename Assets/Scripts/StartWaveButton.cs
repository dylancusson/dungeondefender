using UnityEngine;

public class StartWaveButton : MonoBehaviour
{
    [SerializeField] private WaveManager waveManager;
    public void OnButtonPressed()
    {
        waveManager.StartWave();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
