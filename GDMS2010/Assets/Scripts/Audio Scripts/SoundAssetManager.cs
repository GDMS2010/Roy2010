/*******************************************************************
Aurthor: Christopher Kennedy
Last Modified - 10/04/2020

Desc: This code is meant to be used in Tandem with SoundManager.cs
This code plays holds sound data for the SOundManager to use

How to Use: Fill out the data in editor for the sound that you want
Direct Manipulation of this file should not be needed.

*******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAssetManager : MonoBehaviour
{
    private static SoundAssetManager _i;

    public static SoundAssetManager i
    {
        get
        {
            if (_i == null)
            {
                _i = (Instantiate(Resources.Load("SoundAssetManager")) as GameObject).GetComponent<SoundAssetManager>();
            }

            return _i;
        }

    }  // END SoundAssetManager Getter

    public SoundAudioClip[] soundAudioClipArray; 

    [System.Serializable]
    public class SoundAudioClip
    {
        public string audioName;
        public SoundManager.Sound sound;
        public AudioClip audioClip;
        public float timer;
        public UnityEngine.Audio.AudioMixerGroup audioMixerGroup;
        public float minDistance;
        public float maxDistance;
        public float zeroToOneVolume;
        public float spatialBlend;
        public AudioRolloffMode rolloffMode;

    } // END SoundAudioClip Class

} // END SoundAssetManager Class
