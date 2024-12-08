using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] musicTracks; // Array to hold all music tracks
    private AudioSource audioSource;

    private bool dontStart = true;


    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        
        dontStart = true;

    }

    void Update()
    {
        // Check if the music has stopped playing
        if (!dontStart)
        {
            if (!audioSource.isPlaying)
            {
                PlayRandomTrack();
            }
        }

    }

    public void PlayRandomTrack()
    {
        
        if (musicTracks.Length == 0) return; // Avoid errors if no tracks are set
        int randomIndex = Random.Range(0, musicTracks.Length);
        audioSource.clip = musicTracks[randomIndex];

        // Play the selected track
        audioSource.Play();
        dontStart = false;
    }
}
