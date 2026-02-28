using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public BeatmapLoader loader;
    public GameObject notePrefab;

    public float[] laneX = new float[5] { -4f, -2f, 0f, 2f, 4f };
    public float spawnY = 2f;

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
            Vector3 pos = new Vector3(laneX[laneIndex], spawnY, 0f);
            GameObject note = Instantiate(notePrefab, pos, Quaternion.identity);

            note.GetComponent<Note>().spawnTime = t;
        }
    }
}