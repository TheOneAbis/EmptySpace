using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioSource audioInitial;
    private AudioSource audioNext;
    // Start is called before the first frame update
    void Start()
    {
        audioInitial = GetComponents<AudioSource>()[0];
        audioNext = GetComponents<AudioSource>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchAudio(AudioClip newClip, float transitionSeconds, bool loop)
    {
        // Swap audio references
        AudioSource temp = audioNext;
        audioNext = audioInitial;
        audioInitial = temp;

        audioNext.clip = newClip;
        StartCoroutine(FadeInMain(transitionSeconds, loop));
    }

    public void StopPlaying()
    {
        if (audioInitial.isPlaying) audioInitial.Stop();
        if (audioNext.isPlaying) audioNext.Stop();
    }

    IEnumerator FadeInMain(float fadeTimeSeconds, bool loop)
    {
        audioNext.loop = loop;
        audioNext.Play();
        
        for (float i = 0; i <=1; i += Time.deltaTime * (1 / fadeTimeSeconds))
        {
            audioInitial.volume = 1 - i;
            audioNext.volume = i;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        if (audioInitial.isPlaying)
            audioInitial.Stop();
    }
}
