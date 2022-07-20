using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

   //public List<AudioClip> soundlist = new List<AudioClip>();

   
    public AudioSource sfx;
    public AudioSource ambience;
    public AudioSource music;

    public bool isLooping = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(gameObject);
        }  
    }
    
    public void PlayAmbience(AudioClip clip)
    {
        ambience.clip = clip;
        ambience.Play();
    }

    public void StopAmbience(AudioClip clip)
    {
        ambience.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfx.clip = clip;
        if (sfx.isPlaying == false)
        {
            sfx.Play();
        }
    }

    public void PlaySFXOnce(AudioClip clip)
    {
        sfx.clip = clip;
        if (sfx.isPlaying == false)
        {
            sfx.PlayOneShot(clip);
        }
    }

    public void LoopSFX(AudioClip clip)
    {
        sfx.clip = clip;
        if(sfx.isPlaying == false)
        {
            sfx.Play();
            isLooping = true;
            sfx.loop = isLooping;
        }
    }

    public void StopSFX(AudioClip clip)
    {
        sfx.clip = clip;
        sfx.Stop();
    }
    public void PlayMusic(AudioClip clip)
    {
        music.clip = clip;
        music.Play();
    }
    public void StopMusic()
    {
        music.Stop();
    }
    
}
