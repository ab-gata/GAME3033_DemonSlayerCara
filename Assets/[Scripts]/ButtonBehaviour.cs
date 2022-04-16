using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ButtonBehaviour : MonoBehaviour
{
    MusicManager musicManager = null;
    SoundManager soundManager = null;

    // Start is called before the first frame update
    void Start()
    {
        musicManager = FindObjectOfType<MusicManager>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButton()
    {
        // Play Sound
        soundManager.PlaySound(SoundManager.TrackID.BUTTON);

        // Switch music
        musicManager.PlayMusic(MusicManager.TrackID.ATTACK);

        // Load Scene
        SceneManager.LoadScene("Game");
    }

    public void QuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
