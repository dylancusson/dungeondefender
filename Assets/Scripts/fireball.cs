using UnityEngine;

public class fireball : MonoBehaviour
{
    public float lifetime = 0.25f;
    public GameObject explosionPrefab;
    void Start(){
        Destroy(gameObject, lifetime);

    }

    void OnTriggerEnter2D(Collider2D other)
    {     

        if (other.CompareTag("Enemy")){

            CharacterHealth health = other.GetComponent<CharacterHealth>();

            Debug.Log("Fireball detected enemy: " + other.name);
            if (health != null){
                health.currentHealth -= 30;
                Debug.Log("Enemy damaged. new health: " + health.currentHealth);
            }     

            Destroy(gameObject);   
        }
    }

    //debug function
    void OnDestroy()
    {
        Debug.Log("Fireball destroyed");
    }
}
