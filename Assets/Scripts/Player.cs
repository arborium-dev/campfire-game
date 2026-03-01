using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    
    public float[] laneY = { -2f, -1f, 0f, 1f, 2f }; // defining lanes
    public TextMeshProUGUI healthDisplay; // Assign in Inspector

    private InputAction _moveUpAction;
    private InputAction _moveDownAction;
    private InputAction _attackAction;
    private InputAction _parryAction;
    private float _laneIndex = 2; // start in the middle lane
    private int _health = 3; // Starting health
    private List<Note> _overlappingNotes = new List<Note>(); // Track notes in player's lane
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveUpAction = InputSystem.actions.FindAction("MoveUp");
        _moveDownAction = InputSystem.actions.FindAction("MoveDown");
        _attackAction = InputSystem.actions.FindAction("Attack");
        _parryAction = InputSystem.actions.FindAction("Parry");
        
        // Enable input actions
        _moveUpAction.Enable();
        _moveDownAction.Enable();
        _attackAction.Enable();
        _parryAction.Enable();
        
        // Initialize health display
        UpdateHealthDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        // Update player position based on lane index
        transform.position = new Vector2(-3, laneY[(int)_laneIndex]);
        
    
        if (_moveUpAction.WasPressedThisFrame() && _laneIndex < 4)
        {
            _laneIndex++;
        }

        if (_moveDownAction.WasPressedThisFrame() && _laneIndex > 0)
        {
            _laneIndex--;
        }
        
        if (_attackAction.WasPressedThisFrame())
        {
            // Create a copy of the list to avoid modification during iteration
            List<Note> notesToDestroy = new List<Note>(_overlappingNotes);
            foreach (Note note in notesToDestroy)
            {
                if (note != null)
                {
                    Destroy(note.gameObject);
                }
            }
            _overlappingNotes.Clear();
        }
        
        if (_parryAction.WasPressedThisFrame()) 
        {
            // Create a copy of the list to avoid modification during iteration
            List<Note> notesToDestroy = new List<Note>(_overlappingNotes);
            foreach (Note note in notesToDestroy)
            {
                if (note != null)
                {
                    if (note.isParriable)
                    {
                        // Successfully parried! Gain health
                        Destroy(note.gameObject);
                        GainHealth(1);
                    }
                    else
                    {
                        // Can't parry this note! Take damage
                        TakeDamage(1);
                    }
                }
            }
            _overlappingNotes.Clear();
        }
    }
    
    // Update the health display UI
    private void UpdateHealthDisplay()
    {
        if (healthDisplay != null)
        {
            healthDisplay.text = "Health: " + _health;
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
        if (_health > 5) _health = 5; // Max health cap
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
}
