/*******************************************************************
Aurthor: Christopher Kennedy
Last Modified - 10/04/2020

Desc: This code is meant to be used in Tandem with SoundAssetManager.cs
This code plays sounds based on the information stored inside SoundAssetManager.cs

How to Use: Call the PlaySoundFunction in any script that you want a sound to play.
Params include the sound in the sound array stored in SoundAssetManager for 2D sound
For 3D sound you simply pass in the transform of the object after the sound.

*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    // Add to this Enum when you wish to add a sound to the AssetSoundManager
    // Also add to the switch statement in the CanBePlayed function 
    public enum Sound
    {
       BackgroundMusic,
       Boop
    } // END Sound enum

    // Add to this when you want the sound to have a timer before it can play again. EX. Footsteps / music
    private static Dictionary<Sound, float> soundTimerDictionary;

    // Fuction must be called in game startup
    public static void Initialize()
    {
        soundTimerDictionary = new Dictionary<Sound, float>();

        // Populate the dictionary with the sound plus their timer delay
        // To add this you will follow a formula
        // soundTimerDictionary[Sound.ENUM_TO_BE_ADDED] = -1.0f * SoundAssetManager.i.soundAudioClipArray[POSITION_IN_AUDIO_CLIP_LIBRARY_ARRAY].timer;

        //soundTimerDictionary[Sound.BackgroundMusic] = -1.0f * SoundAssetManager.i.soundAudioClipArray[1].timer;
        soundTimerDictionary[Sound.Boop] = -1.0f * SoundAssetManager.i.soundAudioClipArray[0].timer;

    } // END Initialize Function

     // Used to play Mono Sound (2D Sound)
    public static void PlaySound(Sound _sound)
    {
        if (CanPlaySound(_sound))
        {
            GameObject oneShotGameObject = new GameObject("2D Sound");
            AudioSource oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();

            foreach (SoundAssetManager.SoundAudioClip soundAudioClip in SoundAssetManager.i.soundAudioClipArray)
            {
                if (soundAudioClip.sound == _sound)
                {
                    oneShotAudioSource.clip = soundAudioClip.audioClip;
                    oneShotAudioSource.outputAudioMixerGroup = soundAudioClip.audioMixerGroup;
                }
            }

            oneShotAudioSource.PlayOneShot(oneShotAudioSource.clip);
            Object.Destroy(oneShotGameObject, oneShotAudioSource.clip.length);
        }
    } // END PlaySound Function

    // Used to play a sound from a position (3D Sound)
    public static void PlaySound(Sound _sound, Transform _transform)
    {
        if (CanPlaySound(_sound))
        {
            // Creates the object and assigns it the the object that made the sound
            GameObject soundGameObject = new GameObject("3D Sound");
            soundGameObject.transform.position = _transform.position;
            soundGameObject.transform.SetParent(_transform);
            //Add the tag "Sound" for enemy AI;
            soundGameObject.AddComponent<BoxCollider>();
            soundGameObject.transform.tag = "Sound";

            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

            // Assigns the data from the SoundAssetManager to the sound
            foreach (SoundAssetManager.SoundAudioClip soundAudioClip in SoundAssetManager.i.soundAudioClipArray)
            {
                if (soundAudioClip.sound == _sound)
                {
                    audioSource.clip = soundAudioClip.audioClip;
                    audioSource.outputAudioMixerGroup = soundAudioClip.audioMixerGroup;
                    audioSource.minDistance = soundAudioClip.minDistance;
                    audioSource.maxDistance = soundAudioClip.maxDistance;
                    audioSource.volume = soundAudioClip.zeroToOneVolume;
                    audioSource.spatialBlend = soundAudioClip.spatialBlend;
                    audioSource.rolloffMode = soundAudioClip.rolloffMode;
                }
            }

            audioSource.Play();

            // Destroys the object after it plays to save memory
            Object.Destroy(soundGameObject, audioSource.clip.length);
        }
    } // END PlaySound Function

    // Checks the dictionary timer to see if a sound can be played.
    // Add to the switch statement when you add to the enum above
    private static bool CanPlaySound(Sound _sound)
    {
        switch (_sound)
        {
            default:
                return true;

            /* Example Statement to add at the bottom

            case Sound.ENUM_TO_BE_ADDED:
                if (soundTimerDictionary.ContainsKey(_sound))
                {
                    if (soundTimerDictionary[_sound] + SoundAssetManager.i.soundAudioClipArray[POSITION_IN_AUDIO_CLIP_LIBRARY_ARRAY].timer < Time.time)
                    {
                        soundTimerDictionary[_sound] = Time.time;
                        return true;
                    }
                    else
                        return false;
                }
                break;         

             */
            case Sound.BackgroundMusic:
                if (soundTimerDictionary.ContainsKey(_sound))
                {
                    if (soundTimerDictionary[_sound] + SoundAssetManager.i.soundAudioClipArray[1].timer < Time.time)
                    {
                        soundTimerDictionary[_sound] = Time.time;
                        return true;
                    }
                    else
                        return false;
                }
                break;

            case Sound.Boop:
                if (soundTimerDictionary.ContainsKey(_sound))
                {
                    if (soundTimerDictionary[_sound] + SoundAssetManager.i.soundAudioClipArray[0].timer < Time.time)
                    {
                        soundTimerDictionary[_sound] = Time.time;
                        return true;
                    }
                    else
                        return false;
                }
                break;

        }
        return true;
    } // END CanPlaySound Function

    // The functions below are from previous versions and are not currenly called.
    // Debating on reusing them to add safety to the code
    /*
    private static AudioClip GetAudioClip(Sound _sound)
    {
        foreach (SoundAssetManager.SoundAudioClip soundAudioClip in SoundAssetManager.i.soundAudioClipArray)
        {
            if(soundAudioClip.sound == _sound)
                return soundAudioClip.audioClip;          
        }

        Debug.LogError("Sound " + _sound + " not found!");
        return null;
    } // END GetAudioClip Function

    private static AudioSource GetSoundAudioClipMemberVariables(Sound _sound)
    {
        AudioSource returnThis = new AudioSource();
        //returnThis.clip = GetAudioClip(_sound);

        foreach (SoundAssetManager.SoundAudioClip soundAudioClip in SoundAssetManager.i.soundAudioClipArray)
        {
            if (soundAudioClip.sound == _sound)
            {
                Debug.Log(_sound + " == " + soundAudioClip.sound);
                returnThis.clip = soundAudioClip.audioClip;
                returnThis.minDistance = soundAudioClip.minDistance;
                returnThis.maxDistance = soundAudioClip.maxDistance;
                returnThis.volume = soundAudioClip.zeroToOneVolume;
                returnThis.rolloffMode = soundAudioClip.rolloffMode;
            }
        }

        return returnThis;
    }
    */
} // END SoundManager Class
