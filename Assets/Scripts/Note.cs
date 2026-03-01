using UnityEngine;

public class Note : MonoBehaviour
{
    public float speed = 5f;
    public float spawnTime;
    public int damageAmount = 1; // Damage dealt to player when note is missed
    private Player _player;

    void Start()
    {
        // Find the player in the scene
        _player = FindObjectOfType<Player>();
    }

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Remove"))
        {
            // Damage the player
            if (_player != null)
            {
                _player.TakeDamage(damageAmount);
            }
            Destroy(gameObject);
        }
    }
}