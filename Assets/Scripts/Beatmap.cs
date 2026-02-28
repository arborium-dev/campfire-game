using UnityEngine;

[System.Serializable]
public class Beatmap
{
    public Metadata metadata;
    public float[] buttonOne;
    public float[] buttonTwo;
    public float[] buttonThree;
    public float[] buttonFour;
    public float[] buttonFive;
}

[System.Serializable]
public class Metadata
{
    public string songName;
    public string artist;
    public float bpm;
    public float songLength;
}

