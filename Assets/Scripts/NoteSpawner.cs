using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public BeatmapLoader loader;
    public GameObject notePrefab;
    public float noteDistance;
        
    public float[] laneY = new float[5] { -2f, -1f, 0f, 1f, 2f };
    public float spawnX = 7f;

    void Start()
    {
        SpawnNotes(loader.beatmap.buttonOne, 0);
        //SpawnNotes(loader.beatmap.buttonTwo, 1);
        //SpawnNotes(loader.beatmap.buttonThree, 2);
        //SpawnNotes(loader.beatmap.buttonFour, 3);
        //SpawnNotes(loader.beatmap.buttonFive, 4);
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

}