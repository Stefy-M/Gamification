﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conqueror {
    public class SoundManager : MonoBehaviour
    {
        public AudioSource efxSource;
        public AudioSource musicSource;
        public static SoundManager instance = null;

        public float lowPitchRange = .95f;
        public float highPitchRange = 1.05f;

        public minigame curGame;

        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            // Something that kills this when switching to a different game.
            if (player.Incre.currentGame != curGame)
                Destroy(gameObject);
        }

        public void PlaySingle(AudioClip clip)
        {
            efxSource.clip = clip;
            efxSource.Play();
        }

        public void PlayMusic(AudioClip mus)
        {
            musicSource.clip = mus;
            musicSource.loop = true;
            musicSource.Play();
        }

        public void RandomizeSfx(params AudioClip[] clips)
        {
            int randomIndex = Random.Range(0, clips.Length);
            float randomPitch = Random.Range(lowPitchRange, highPitchRange);

            efxSource.pitch = randomPitch;
            efxSource.clip = clips[randomIndex];
            efxSource.PlayOneShot(efxSource.clip);
        }
    }
}