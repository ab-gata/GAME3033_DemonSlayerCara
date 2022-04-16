using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private float volume = 0.3f;
    // Music manager, a singleton
    private MusicManager() { }

    private static MusicManager instance = null;

    public static MusicManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MusicManager>();
                DontDestroyOnLoad(instance.transform.root);
            }
            return instance;
        }

        private set { instance = value; }
    }

    AudioSource audioSource;

    // List of bgm
    [SerializeField]
    List<AudioClip> music;

    public enum TrackID
    {
        MAINMENU,
        ATTACK
    }

    // Start is called before the first frame update
    void Start()
    {
        // Each time start is loaded, there is another Music Manager
        MusicManager[] imposters = FindObjectsOfType<MusicManager>();

        if (imposters.Length > 1)
        {
            foreach (MusicManager imposter in imposters)
            {
                if (imposter == this)
                {
                    Destroy(imposter.gameObject);
                    break;
                }
            }
        }

        audioSource = GetComponent<AudioSource>();
        Instance.PlayMusic(TrackID.MAINMENU);
    }

    public void PlayMusic(TrackID id)
    {
        audioSource.clip = music[(int)id];
        StartCoroutine(FadeInMusicOverDuration(3.0f));
    }

    public void StopMusic()
    {
        StartCoroutine(FadeOutMusicOverDuration(3.0f));
    }

    IEnumerator FadeInMusicOverDuration(float duration)
    {
        audioSource.Play();
        float timer = 0.0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float fadeValue = timer / duration;
            audioSource.volume = Mathf.SmoothStep(0.0f, volume, fadeValue); // S-curved fade in
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator FadeOutMusicOverDuration(float duration)
    {
        float timer = 0.0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float fadeValue = timer / duration;
            audioSource.volume = Mathf.SmoothStep(volume, 0.0f, fadeValue); // S-curved fade in
            yield return new WaitForEndOfFrame();
        }
        audioSource.Stop();
    }
}
