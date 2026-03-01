using UnityEngine;

public class BeatmapLoader : MonoBehaviour
{
    public Beatmap beatmap;

    private string[] _beatmapFiles =
        { "insomnia", "stillness", "drift", "adrenaline", "tachysensia", "nostalgia", "euphoria", "unbeatable" };

    [SerializeField] private SongSelection songSelection;

    void Awake()
    {
        // Load SongSelection from Resources if not assigned in Inspector
        if (songSelection == null)
        {
            songSelection = Resources.Load<SongSelection>("SongSelection");
        }

        if (songSelection == null)
        {
            Debug.LogError(
                "BeatmapLoader: SongSelection asset not found! Make sure it exists at Assets/Resources/SongSelection.asset");
            return;
        }

        Debug.Log("Loading beatmap: " + _beatmapFiles[songSelection.selectedSong]);
        TextAsset json = Resources.Load<TextAsset>(_beatmapFiles[songSelection.selectedSong]);

        if (json == null)
        {
            Debug.LogError("BeatmapLoader: Beatmap file not found: " + _beatmapFiles[songSelection.selectedSong]);
            return;
        }

        beatmap = JsonUtility.FromJson<Beatmap>(json.text);
    }

}