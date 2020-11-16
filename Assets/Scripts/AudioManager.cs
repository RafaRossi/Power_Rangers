using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Manager<AudioManager>
{
    public AudioSource source;
    public AudioClip punch;

    public void PlayPunch()
    {
        source.PlayOneShot(punch);
    }
}
