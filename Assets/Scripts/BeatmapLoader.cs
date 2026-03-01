using UnityEngine;

public class BeatmapLoader : MonoBehaviour
{
    public Beatmap beatmap;
    private string[] beatmapFiles = new string[7] {"insomnia", "stillness", "drift", "adrenaline", "tachysensia", "untitled", "euphoria"};
    public int currentBeatmapIndex = 0;
    void Awake()
    {
        TextAsset json = Resources.Load<TextAsset>(beatmapFiles[currentBeatmapIndex]);
        beatmap = JsonUtility.FromJson<Beatmap>(json.text);
    }
}