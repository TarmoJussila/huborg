using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource effectAudioSource;

    public void PlaySoundEffect(AudioClip audioClip)
    {
        effectAudioSource.PlayOneShot(audioClip, Random.Range(0.9f, 1.1f));
    }
}