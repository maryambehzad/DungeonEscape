using UnityEngine;

public class SoundManager
{
    public static void Play(string name, float volume = 1f)
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/" + name);
        if (clip == null) return;
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
    }
}