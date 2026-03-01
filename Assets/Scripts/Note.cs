using UnityEngine;

public class Note : MonoBehaviour
{
    public float speed = 5f;
    public float spawnTime;
    public int damageAmount = 1; // Damage dealt to player when note is missed
    public bool isParriable = false; // Whether this note can be parried
    private Player _player;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    void Start()
    {
        // Find the player in the scene
        _player = FindObjectOfType<Player>();
        
        // Get sprite renderer and set color if parriable
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        if (_spriteRenderer != null && isParriable)
        {
            // Make parriable notes yellow/gold so player can distinguish them
            _animator.Play("parry_worm");
            _spriteRenderer.color = Color.red;
        }
        else
        {
            _animator.Play("normal_worm");
        }
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