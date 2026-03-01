using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public BeatmapLoader loader;
    public GameObject notePrefab;
    public float noteDistance;
    public AudioSource audioSource; // Assign in Inspector
    public float audioOffset; // Adjust this to sync notes with music (in seconds)
    
    [SerializeField] private SongSelection songSelection;
    private string[] _songFiles = { "Music/Insomnia", "Music/Stillness", "Music/Drift", "Music/Adrenaline", "Music/Tachysensia", "Music/Nostalgia", "Music/Euphoria" };
    
    public float[] laneY = { -2.5f, -1.5f, -0.5f, 0.5f, 1.5f };
    public float spawnX = 7f;

    void Start()
    {
        
        
        // Load the correct audio clip based on selected song
        if (audioSource != null)
        {
            AudioClip clip = Resources.Load<AudioClip>(_songFiles[songSelection.selectedSong]);
            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.PlayDelayed(audioOffset);
                Debug.Log($"Playing song: {_songFiles[songSelection.selectedSong]}");
            }
            else
            {
                Debug.LogError($"NoteSpawner: Audio clip not found: {_songFiles[songSelection.selectedSong]}");
            }
        }

        SpawnNotes(loader.beatmap.buttonOne, 0);
        SpawnNotes(loader.beatmap.buttonTwo, 1);
        SpawnNotes(loader.beatmap.buttonThree, 2);
        SpawnNotes(loader.beatmap.buttonFour, 3);
        SpawnNotes(loader.beatmap.buttonFive, 4);
    }

    void SpawnNotes(float[] timestamps, int laneIndex)
    {
        foreach (float t in timestamps)
        {
            float x = spawnX + (t * noteDistance);  // <-- use t here
            Vector3 pos = new Vector3(x, laneY[laneIndex], 0f);

            GameObject note = Instantiate(notePrefab, pos, Quaternion.identity);
            Note noteScript = note.GetComponent<Note>();
            noteScript.spawnTime = t;
            
            // 1/10 chance (10%) for note to be parriable
            noteScript.isParriable = (Random.value <= 0.1f);
        }
    }

    void Update()
    {
        // Debug: Display current audio time and offset info
        if (audioSource != null && audioSource.isPlaying)
        {
            Debug.Log($"Audio Time: {audioSource.time:F2}s | Offset: {audioOffset:F2}s");
        }
    }
}