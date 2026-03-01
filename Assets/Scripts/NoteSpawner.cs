using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public BeatmapLoader loader;
    public GameObject notePrefab;
    public float noteDistance;
    public AudioSource audioSource; // Assign in Inspector
    public float audioOffset = 0f; // Adjust this to sync notes with music (in seconds)
        
    public float[] laneY = new float[5] { -2f, -1f, 0f, 1f, 2f };
    public float spawnX = 7f;

    void Start()
    {
        // Play audio and spawn notes
        if (audioSource != null)
        {
            audioSource.PlayDelayed(audioOffset);
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
            note.GetComponent<Note>().spawnTime = t;
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