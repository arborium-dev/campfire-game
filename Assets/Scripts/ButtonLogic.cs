using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLogic : MonoBehaviour
{
    public int levelChoice;
    [SerializeField] private SongSelection songSelection;

    private void Start()
    {
        // Validate that songSelection is assigned
        if (songSelection == null)
        {
            Debug.LogError("ButtonLogic: songSelection is not assigned! Drag the SongSelection asset into the Inspector.");
        }
    }
    
    public void level1()
    {
        Debug.Log("level 1");
        songSelection.selectedSong = 0;
        SceneManager.LoadScene("Gameplay");
    }
    public void level2()
    {
        Debug.Log("level 2");
        songSelection.selectedSong = 1;
        SceneManager.LoadScene("Gameplay");
    }
    public void level3()
    {
        Debug.Log("level 3");
        songSelection.selectedSong = 2;
        SceneManager.LoadScene("Gameplay");
    }
    public void level4()
    {
        Debug.Log("level 4");
        songSelection.selectedSong = 3;
        SceneManager.LoadScene("Gameplay");
    }
    public void level5()
    {
        Debug.Log("level 5");
        songSelection.selectedSong = 4;
        SceneManager.LoadScene("Gameplay");
    }
    public void level6()
    {
        Debug.Log("level 6");
        songSelection.selectedSong = 5;
        SceneManager.LoadScene("Gameplay");
    }
    public void level7()
    {
        Debug.Log("level 7");
        songSelection.selectedSong = 6;
        SceneManager.LoadScene("Gameplay");
    }
    public void Reset()
    {
        SceneManager.LoadScene("Gameplay");
    }
    public void Quit()
    {
        songSelection.selectedSong = 0;
        SceneManager.LoadScene("Scenes/Menu");
    }
}
