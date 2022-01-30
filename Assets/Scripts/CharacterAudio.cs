using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    [SerializeField] private AudioSource effectAudioSource;
    [SerializeField] private AudioClip upgradeAudioClip;

    private void PlaySoundEffect(AudioClip audioClip)
    {
        effectAudioSource.PlayOneShot(audioClip, Random.Range(0.9f, 1.1f));
    }

    public void PlayUpgradeAudioClip()
    {
        PlaySoundEffect(upgradeAudioClip);
    }
}