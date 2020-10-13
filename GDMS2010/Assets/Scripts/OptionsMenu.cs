using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;

    Resolution[] resolutions;

    public Dropdown resolutionDropdown;

    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;
    public Slider dialogueSlider;

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        masterSlider.value = PlayerPrefs.GetFloat("masterVol");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVol");
        musicSlider.value = PlayerPrefs.GetFloat("musicVol");
        dialogueSlider.value = PlayerPrefs.GetFloat("dialougeVol");

    }

    private void Awake()
    {
        audioMixer = Resources.Load<AudioMixer>("MainAudioMixer");
    }
    public void SetMaster(float volume)
    {
        audioMixer.SetFloat("masterVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("masterVol", volume);
        Debug.Log("Master");
    }

    public void SetSFX(float volume)
    {
        audioMixer.SetFloat("sfxVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVol", volume);
        Debug.Log("SFX Volume");
        //soundSource.Play();
    }

    public void SetMusic(float volume)
    {
        audioMixer.SetFloat("musicVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVol", volume);
        Debug.Log("Music Volume");
    }
    public void SetDialouge(float volume)
    {
        audioMixer.SetFloat("dialougeVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("dialougeVol", volume);
        Debug.Log("Dialouge");
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
