using UnityEngine;
using UnityEngine.Audio;

public class Radio : MonoBehaviour
{
    [SerializeField] AudioClip[] songs;
    [SerializeField] AudioSource musicPlayer;
    private void Start()
    {
        musicPlayer.clip = songs[Random.Range(0, songs.Length)];
        musicPlayer.Play();

    }

    private void Update()
    {
        if (!musicPlayer.isPlaying)
        {
            musicPlayer.clip = songs[Random.Range(0, songs.Length)];
            musicPlayer.Play();
        }
    }


}
