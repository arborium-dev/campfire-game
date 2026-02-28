using UnityEngine;

public class Note : MonoBehaviour
{
    public float speed = 5f;
    public float spawnTime;

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Remove"))
            Destroy(gameObject);
    }
}