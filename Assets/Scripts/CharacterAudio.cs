using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    [SerializeField] private AudioSource effectAudioSource;
    [SerializeField] private AudioClip upgradeAudioClip;
    [SerializeField] private AudioClip relativeAudioClip;
    [SerializeField] private AudioClip pickupAudioClip;

    private void PlaySoundEffect(AudioClip audioClip)
    {
        effectAudioSource.PlayOneShot(audioClip, Random.Range(0.9f, 1.1f));
    }

    public void PlayUpgradeAudioClip()
    {
        PlaySoundEffect(upgradeAudioClip);
    }

    public void PlayRelativeSacrificeAudioClip()
    {
        PlaySoundEffect(relativeAudioClip);
    }

    public void PlayPickupAudioClip()
    {

    }
}