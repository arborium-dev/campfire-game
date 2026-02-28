using UnityEngine;

public class BeatmapLoader : MonoBehaviour
{
    public Beatmap beatmap;

    void Awake()
    {
        TextAsset json = Resources.Load<TextAsset>("beatmap");
        beatmap = JsonUtility.FromJson<Beatmap>(json.text);
    }
}