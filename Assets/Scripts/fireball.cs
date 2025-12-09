using UnityEngine;

public class fireball : MonoBehaviour
{
    public float lifetime = 0.25f;
    public int damage = 50;
    public GameObject explosionPrefab;
    void Start(){
        Destroy(gameObject, lifetime);
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {     

        if (other.CompareTag("Enemy")){

            CharacterHealth health = other.GetComponentInParent<CharacterHealth>();

            Debug.Log("Fireball detected enemy: " + other.name);

            if (health != null){
                health.TakeDamage(damage);
            }
            Vector3 explosionPos = new Vector3(this.transform.position.x, this.transform.position.y, 0);
            Instantiate(explosionPrefab, explosionPos, Quaternion.identity);
            Destroy(gameObject);   
        }
    }

    //debug function
    void OnDestroy()
    {
        Debug.Log("Fireball destroyed");
    }
}
