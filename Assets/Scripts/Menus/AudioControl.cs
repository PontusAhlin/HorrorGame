/**
    * This script is used to control the volume of the game.
    * It uses the PlayerPrefs to store the volume value.
    *
    * Author(s): Arnob Sarker
    */
using UnityEngine;
using UnityEngine.UI;

public class AudioControl : MonoBehaviour
{

    [Tooltip("The slider used to control the volume.")]
    [SerializeField] Slider volumeSlider;

    // Update is called once per frame
    void Awake()
    {
        Debug.Log("Volume: " + PlayerPrefs.GetFloat("musicVolume"));
        if(!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}
