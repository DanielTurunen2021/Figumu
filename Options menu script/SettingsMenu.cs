using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] AudioMixer MainMixer;
    [SerializeField] Dropdown ResolutionDropDown;
    [SerializeField] Slider Musicslider;
    [SerializeField] Slider SFXSlider;

    private void Awake()
    {
       
        float musicvolume = PlayerPrefs.GetFloat("MusicVol");
        float sfxvolume = PlayerPrefs.GetFloat("SFXVol");
        MainMixer.SetFloat("MusicVolume", musicvolume);
        MainMixer.SetFloat("SFXVolume", sfxvolume);
        PlayerPrefs.GetInt("Resolution");
        Musicslider.value = musicvolume;
        SFXSlider.value = sfxvolume;
        
        

        //Clears the values in the resolution dropdown.
        ResolutionDropDown.ClearOptions();

        //Creates a list called options
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        //Loops through our resolutions array
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            //Converts our resolutions in the array into strings.
            string option = Screen.resolutions[i].width + "X" + Screen.resolutions[i].height + " : " + Screen.resolutions[i].refreshRate + " Hz";
            //Adds the strings to our options list.
            options.Add(option);

            //Checks the current resolution width & height and sets it equal to the resolution index.
            if(Screen.resolutions[i].width == Screen.currentResolution.width && 
                Screen.resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        //Adds the options we just converted to our dropdown.
        ResolutionDropDown.AddOptions(options);

        //Sets the value of the dropdown to the resolutionindex that we got in the above if statement.
        ResolutionDropDown.value = currentResolutionIndex;

        PlayerPrefs.SetInt("Resolution", currentResolutionIndex);

        //Refreshes the text and image of the currently selected option.
        ResolutionDropDown.RefreshShownValue();
    }

    //Lets you update the resolution.
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = Screen.resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }


    //Changes the music volume
    public void SetMusicVolume(float musicVolume)
    {
        //Changes the value of the Music volume in the mixer.
        MainMixer.SetFloat("MusicVolume", musicVolume);

        PlayerPrefs.SetFloat("MusicVol", musicVolume);
    }

    //Sets the SFX volume
    public void SetSFXVolume(float sfxVolume)
    {
        //Changes the value of the SFX master in the mixer.
        MainMixer.SetFloat("SFXVolume", sfxVolume);

        PlayerPrefs.SetFloat("SFXVol", sfxVolume);
    }


    //Sets the sceen to fullscreen mode, isFullScreen is true by default.
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        
        //Checks the value of isFullScreen and sets the screen to exclusive fullscreen or windowed mode.
        if(isFullScreen == true)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else if(isFullScreen == false)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }
}
