using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float speed;

    //public float speed = 1f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) && (transform.position.x >= -12.5) && (Input.GetKey(KeyCode.W) && (transform.position.y <= 6.5)))
        {
            transform.position += ((Vector3.left / 3 + Vector3.up / 2) * Time.deltaTime * speed) ;
        }

        else if (Input.GetKey(KeyCode.D) && (transform.position.x <= 12.5) && (Input.GetKey(KeyCode.W) && (transform.position.y <= 6.5)))
        {
            transform.position += ((Vector3.right / 3 + Vector3.up / 2) * Time.deltaTime * speed);
        }

        else if (Input.GetKey(KeyCode.A) && (transform.position.x >= -12.5) && (Input.GetKey(KeyCode.S) && (transform.position.y >= -9)))
        {
            transform.position += ((Vector3.left / 3 + Vector3.down / 2) * Time.deltaTime * speed);
        }

        else if (Input.GetKey(KeyCode.D) && (transform.position.x <= 12.5) && (Input.GetKey(KeyCode.S) && (transform.position.y >= -9)))
        {
            transform.position += ((Vector3.right / 3 + Vector3.down / 2) * Time.deltaTime * speed);
        }

        if (Input.GetKey(KeyCode.A) && (transform.position.x >= -12.5))
        {
            transform.position += Vector3.left * Time.deltaTime * speed;
        }

        else if (Input.GetKey(KeyCode.D) && (transform.position.x <= 12.5))
        {
            transform.position += Vector3.right * Time.deltaTime * speed;
        }

        else if (Input.GetKey(KeyCode.W) && (transform.position.y <= 6.5))
        {
            transform.position += Vector3.up * Time.deltaTime * speed;
        }

        else if (Input.GetKey(KeyCode.S) && (transform.position.y >= -9))
        {
            transform.position += Vector3.down * Time.deltaTime * speed;
        }
    }
}
