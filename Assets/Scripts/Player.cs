using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    
    public float[] laneY = { -2f, -1f, 0f, 1f, 2f }; // defining lanes
    public TextMeshProUGUI healthDisplay; // Assign in Inspector
    public GameObject gameOver;

    private InputAction _moveUpAction;
    private InputAction _moveDownAction;
    private InputAction _attackAction;
    private InputAction _parryAction;
    private float _laneIndex = 2; // start in the middle lane
    private int _health = 15; // Starting health
    private List<Note> _overlappingNotes = new List<Note>(); // Track notes in player's lane
    private Animator _animator;
    private bool dead = false;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveUpAction = InputSystem.actions.FindAction("MoveUp");
        _moveDownAction = InputSystem.actions.FindAction("MoveDown");
        _attackAction = InputSystem.actions.FindAction("Attack");
        _parryAction = InputSystem.actions.FindAction("Parry");
        _animator = GetComponent<Animator>();
        
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
        // Update player position based on lane index
        transform.position = new Vector2(-3, laneY[(int)_laneIndex]);
        
    
        if (_moveUpAction.IsPressed() && _laneIndex < 4 && dead == false)
        {
            _laneIndex=3;
        }

        if (_moveDownAction.IsPressed() && _laneIndex > 0 && dead == false)
        {
            _laneIndex=1;
        }
        
        if (!_moveUpAction.IsPressed() && ! _moveDownAction.IsPressed())
        {
            _laneIndex=2;
        }
        
        if (_attackAction.WasPressedThisFrame() && dead == false)
        {
            Debug.Log("Attack key pressed!");
            PlayAttackAnimation();
            // Destroy only the leftmost note
            DestroyLeftmostNote();
        }
        
        if (_parryAction.WasPressedThisFrame() && dead == false) 
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
            dead = true;
            gameOver.SetActive(true);
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
    
    // Method to take damage
    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health < 0) _health = 0;
        UpdateHealthDisplay();
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
            if (note != null)
            {
                _overlappingNotes.Remove(note);
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
            }
            else
            {
                // Can't parry this note! Take damage
                TakeDamage(1);
            }
            _overlappingNotes.Remove(leftmostNote);
        }
    }
}
