using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PAUT
{
    using UnityEngine;
    using System.Collections;

    public class RandomSound : MonoBehaviour
    {

     public AudioSource randomSound;

        public AudioClip[] audioSources;
 
        public bool isPlaying()
        {
            return randomSound.isPlaying;
        }

        public void setVolume(float val)
        {
            randomSound.volume = val;
        }

        public void PlayRandom()
        {
            int index = Random.Range(0, audioSources.Length);
            AudioClip shootClip = audioSources[index];

            randomSound.clip = shootClip;
            randomSound.Play();

        }
    }

}

