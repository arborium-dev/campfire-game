using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    
    public float[] laneY = { -2f, -1f, 0f, 1f, 2f }; // defining lanes
    public TextMeshProUGUI healthDisplay; // Assign in Inspector
    public GameObject gameOver;
    public TextMeshProUGUI gameOverText; // Assign the text component for game over message
    public TextMeshProUGUI streakDisplay;
    
    // Sound Effects
    // private AudioSource _audioSource;
    // private AudioClip _attackSfx;
    // private AudioClip _parrySfx;
    [SerializeField] private SongSelection songSelection;
    
    private InputAction _moveUpAction;
    private InputAction _moveDownAction;
    private InputAction _attackAction;
    private InputAction _parryAction;
    private InputAction _hiddenAction;
    private float _laneIndex = 2; // start in the middle lane
    private int _health = 15; // Starting health
    private List<Note> _overlappingNotes = new List<Note>(); // Track notes in player's lane
    private Animator _animator;
    private bool _dead;
    private int _streak = 0; // Track consecutive successful hits
    private float _shakeIntensity = 0f;
    private float _shakeTimer = 0f;
    private Vector3 _originalCameraPos;
    private Camera _mainCamera;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveUpAction = InputSystem.actions.FindAction("MoveUp");
        _moveDownAction = InputSystem.actions.FindAction("MoveDown");
        _attackAction = InputSystem.actions.FindAction("Attack");
        _parryAction = InputSystem.actions.FindAction("Parry");
        _hiddenAction = InputSystem.actions.FindAction("HiddenLevel");
        _animator = GetComponent<Animator>();
        _mainCamera = Camera.main;
        
        if (_mainCamera != null)
        {
            _originalCameraPos = _mainCamera.transform.position;
        }
        
        // Initialize AudioSource
        // _audioSource = GetComponent<AudioSource>();
        // if (_audioSource == null)
        // {
        //     _audioSource = gameObject.AddComponent<AudioSource>();
        // }
        //
        // // Load sound effects from Resources
        // _attackSfx = Resources.Load<AudioClip>("SFX/normalHit");
        // _parrySfx = Resources.Load<AudioClip>("SFX/parryHit");
        //
        // if (_attackSfx == null)
        // {
        //     Debug.LogWarning("Attack sound effect not found at Resources/SFX/normalHit");
        // }
        // if (_parrySfx == null)
        // {
        //     Debug.LogWarning("Parry sound effect not found at Resources/SFX/parryHit");
        // }
        //
        // Debug: Check if animator is found
        if (_animator == null)
        {
            Debug.LogError("Animator not found on Player object!");
        }
        else
        {
            Debug.Log("Animator found successfully");
        }
        
        // Enable input actions
        _moveUpAction.Enable();
        _moveDownAction.Enable();
        _attackAction.Enable();
        _parryAction.Enable();
        
        // Initialize health display
        UpdateHealthDisplay();
        gameOver.SetActive(false);
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Update screenshake
        // UpdateScreenShake(); // no screenshake. no. dont.
        
        // Update player position based on lane index
        transform.position = new Vector2(-3, laneY[(int)_laneIndex]);

        if (_hiddenAction.IsPressed())
        {
            songSelection.selectedSong = 7; // Set to hidden level index
            SceneManager.LoadScene("Gameplay");
        }
    
        if (_moveUpAction.IsPressed() && _laneIndex < 4 && _dead == false)
        {
            _laneIndex=3;
        }

        if (_moveDownAction.IsPressed() && _laneIndex > 0 && _dead == false)
        {
            _laneIndex=1;
        }
        
        if (!_moveUpAction.IsPressed() && ! _moveDownAction.IsPressed())
        {
            _laneIndex=2;
        }
        
        if (_attackAction.WasPressedThisFrame() && _dead == false)
        {
            Debug.Log("Attack key pressed!");
            PlayAttackAnimation();
            // Destroy only the leftmost note
            DestroyLeftmostNote();
        }
        
        if (_parryAction.WasPressedThisFrame() && _dead == false) 
        {
            Debug.Log("Parry key pressed!");
            PlayParryAnimation();
            // Destroy only the leftmost note with parry logic
            DestroyLeftmostNoteWithParry();
        }
        //when health is under 1 you fail
        if(_health <= 0)
        {
            Time.timeScale = 0f;
            _dead = true;
            gameOver.SetActive(true);
            // Show defeat message
            if (gameOverText != null)
            {
                gameOverText.text = "Game Over!";
            }
        }
        
        // Check for victory condition - all notes destroyed
        if (!_dead && GameObject.FindGameObjectsWithTag("Note").Length == 0)
        {
            
            Time.timeScale = 0f;
            _dead = true;
            gameOver.SetActive(true);
            // Show victory message
            if (gameOverText != null)
            {
                gameOverText.text = "You Won!";
            }
            
        }
    }
    
    // Update the health display UI
    private void UpdateHealthDisplay()
    {
        if (healthDisplay != null)
        {
            healthDisplay.text = "Food left: " + _health;
        }
    }
    
    // Update the streak display UI
    private void UpdateStreakDisplay()
    {
        if (streakDisplay != null)
        {
            streakDisplay.text = _streak.ToString();
        }
    }

    // Method to take damage
    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health < 0) _health = 0;
        UpdateHealthDisplay();
        
        // Reset streak on taking damage
        _streak = 0;
        UpdateStreakDisplay();
    }
    
    public void GainHealth(int amount)
    {
        _health += amount;
        if (_health > 25) _health = 25; // Max health cap
        UpdateHealthDisplay();
    }
    
    // Detect when a note overlaps with the player
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            Note note = collision.GetComponent<Note>();
            if (note != null && !_overlappingNotes.Contains(note))
            {
                _overlappingNotes.Add(note);
            }
        }
    }
    
    // Remove note from list when it leaves the player's collision area
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            Note note = collision.GetComponent<Note>();
            if (note != null && _overlappingNotes.Contains(note))
            {
                _overlappingNotes.Remove(note);
                
                // Only reset streak if the note still exists (wasn't destroyed by us)
                if (note.gameObject != null)
                {
                    // Streak broken if note was missed
                    _streak = 0;
                    UpdateStreakDisplay();
                }
            }
        }
    }
    
    // Animation Functions
    
    private void PlayAttackAnimation()
    {
        if (_animator != null)
        {
            _animator.Play("Attack");
            StartCoroutine(ReturnToIdleAfterAnimation("Attack"));
        }
    }
    
    private void PlayParryAnimation()
    {
        if (_animator != null)
        {
            _animator.Play("Parry");
            StartCoroutine(ReturnToIdleAfterAnimation("Parry"));
        }
    }
    
    private System.Collections.IEnumerator ReturnToIdleAfterAnimation(string animationName)
    {
        // Get animation length
        AnimationClip clip = GetAnimationClip(animationName);
        if (clip != null)
        {
            yield return new WaitForSeconds(clip.length);
            _animator.Play("Idle");
        }
    }
    
    private AnimationClip GetAnimationClip(string clipName)
    {
        if (_animator == null || _animator.runtimeAnimatorController == null)
            return null;
        
        AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == clipName)
                return clip;
        }
        return null;
    }
    
    // Destroy only the leftmost note
    private void DestroyLeftmostNote()
    {
        if (_overlappingNotes.Count == 0)
            return;
        
        // Find the note with the smallest X position (leftmost)
        Note leftmostNote = _overlappingNotes[0];
        foreach (Note note in _overlappingNotes)
        {
            if (note != null && note.transform.position.x < leftmostNote.transform.position.x)
            {
                leftmostNote = note;
            }
        }
        
        if (leftmostNote != null)
        {
            Destroy(leftmostNote.gameObject);
            _overlappingNotes.Remove(leftmostNote);
            
            // Increment streak and trigger screenshake
            _streak++;
            TriggerScreenShake(0.1f + (_streak * 0.02f)); // Intensity scales with streak
            UpdateStreakDisplay();
            
            // // Play attack sound effect
            // if (_audioSource != null && _attackSfx != null)
            // {
            //     _audioSource.PlayOneShot(_attackSfx);
            // }
        }
    }
    
    // Destroy only the leftmost note with parry logic
    private void DestroyLeftmostNoteWithParry()
    {
        if (_overlappingNotes.Count == 0)
            return;
        
        // Find the note with the smallest X position (leftmost)
        Note leftmostNote = _overlappingNotes[0];
        foreach (Note note in _overlappingNotes)
        {
            if (note != null && note.transform.position.x < leftmostNote.transform.position.x)
            {
                leftmostNote = note;
            }
        }
        
        if (leftmostNote != null)
        {
            if (leftmostNote.isParriable)
            {
                // Successfully parried! Gain health
                Destroy(leftmostNote.gameObject);
                GainHealth(3); // Gain 3 health for parrying
                
                // Increment streak and trigger stronger screenshake
                _streak++;
                TriggerScreenShake(0.15f + (_streak * 0.02f)); // Parry has stronger shake
                UpdateStreakDisplay();
                
                // Play parry sound effect
                // if (_audioSource != null && _parrySfx != null)
                // {
                //     _audioSource.PlayOneShot(_parrySfx);
                // }
            }
            else
            {
                // Can't parry this note! Take damage
                TakeDamage(1);
                _streak = 0; // Reset streak on failed parry
                UpdateStreakDisplay();
                // Play attack sound for failed parry (optional)
                // if (_audioSource != null && _attackSfx != null)
                // {
                //     _audioSource.PlayOneShot(_attackSfx);
                // }
            }
            _overlappingNotes.Remove(leftmostNote);
        }
    }
    
    // Trigger a screenshake effect
    private void TriggerScreenShake(float intensity)
    {
        _shakeIntensity = intensity;
        _shakeTimer = 0.1f; // Duration of screenshake
    }
    
    // Update screenshake position
    private void UpdateScreenShake()
    {
        if (_mainCamera == null || _shakeIntensity <= 0f)
            return;
        
        if (_shakeTimer > 0f)
        {
            _shakeTimer -= Time.deltaTime;
            
            // Random offset within shake intensity
            float xOffset = Random.Range(-_shakeIntensity, _shakeIntensity);
            float yOffset = Random.Range(-_shakeIntensity, _shakeIntensity);
            
            Vector3 newPos = _originalCameraPos + new Vector3(xOffset, yOffset, 0);
            _mainCamera.transform.position = newPos;
        }
        else
        {
            // Return to original position
            _shakeIntensity = 0f;
            _mainCamera.transform.position = _originalCameraPos;
        }
    }
}
