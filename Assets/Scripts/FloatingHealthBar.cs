using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    public void updateHealthBar(float Currenthealth, float maxHealth)
    {
        slider.value = Currenthealth/maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
        transform.position = target.position + offset;
    }
}
