using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    private static List<AudioSource> loopingSources = new List<AudioSource>();

    public enum Sound
    {
        Explosion,
        CoinPickUp,
        PlaneSound,
        ButtonClick,
        Scroll,
        PowerUp,
        Archivement
        // Add more as needed
    }

    public static void PlaySound(Sound sound)
    {
        if (PlayerPrefs.GetInt("VolumeMuted", 0) == 0)
        {
            GameObject soundGameObject = new GameObject("Sound");
            soundGameObject.tag = "Sound";
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            AudioClip clip = GetAudioClip(sound);
            if (clip != null)
            {
                audioSource.PlayOneShot(clip);
                soundGameObject.AddComponent<SoundDestroyer>().Init(clip.length);
            }
        }
    }

    //  New function: plays and also returns the AudioClip
    public static AudioClip PlaySoundAndReturn(Sound sound)
    {
        if (PlayerPrefs.GetInt("VolumeMuted", 0) == 0)
        {
            GameObject soundGameObject = new GameObject("Sound");
            soundGameObject.tag = "Sound";
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            AudioClip clip = GetAudioClip(sound);
            if (clip != null)
            {
                audioSource.PlayOneShot(clip);
                soundGameObject.AddComponent<SoundDestroyer>().Init(clip.length);
            }
            return clip;
        }
        return null;
    }

    public static AudioSource PlayLoopingSound(Sound sound, float volume = 1f)
    {
        if (PlayerPrefs.GetInt("VolumeMuted", 0) == 1)
            return null;

        GameObject soundGameObject = new GameObject("LoopingSound_" + sound);
        soundGameObject.tag = "Sound";
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        AudioClip clip = GetAudioClip(sound);

        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.volume = volume;
            audioSource.Play();
            loopingSources.Add(audioSource); // 👈 Track it
            return audioSource;
        }
        return null;
    }

    public static void StopSound(AudioSource audioSource)
    {
        if (audioSource != null)
        {
            loopingSources.Remove(audioSource); // 👈 Remove if stopped
            GameObject.Destroy(audioSource.gameObject);
        }
    }

    private class SoundDestroyer : MonoBehaviour
    {
        public void Init(float duration)
        {
            Destroy(gameObject, duration);
        }
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (GameAsserts.SoundAudioClip soundAudioClip in GameAsserts.i.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
                return soundAudioClip.audioClip;
        }
        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }

    public static void PauseAllLoopingSounds()
    {
        foreach (var src in loopingSources)
        {
            if (src != null && src.isPlaying)
                src.Pause();
        }
    }

    public static void ResumeAllLoopingSounds()
    {
        foreach (var src in loopingSources)
        {
            if (src != null && !src.isPlaying)
                src.UnPause();
        }
    }
}
