using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Music manager, a singleton
    private SoundManager() { }

    private static SoundManager instance = null;

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
                DontDestroyOnLoad(instance.transform.root);
            }
            return instance;
        }

        private set { instance = value; }
    }

    AudioSource audioSource;

    // List of bgm
    [SerializeField]
    List<AudioClip> sound;

    public enum TrackID
    {
        BUTTON,
        WIN,
        LOSE,
        LASER
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Instance.JustToCreateInstance();
    }

    public void PlaySound(TrackID id)
    {
        audioSource.clip = sound[(int)id];
        audioSource.Play();
    }

    private void JustToCreateInstance()
    {

    }
}
