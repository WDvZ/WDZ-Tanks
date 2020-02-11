using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public TMP_InputField username;
    public Slider musicVolumeSlider;
    public Slider effectsVolumeSlider;
    string uname;

    // Start is called before the first frame update
    void Start()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        effectsVolumeSlider.value = PlayerPrefs.GetFloat("EffectsVolume", 0.75f);
        uname = PlayerPrefs.GetString("username"); // Get the username, no default
        Debug.Log("uname is :" + uname);
        if (uname==null || uname == "") // missing or empty
        {
            PlayerPrefs.SetString("username", createUname());
            uname = PlayerPrefs.GetString("username");
        }
        username.text = uname;
    }

    public void updateName()
    {
        PlayerPrefs.SetString("username", username.text);
    }

    public void updateMusicVolume()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
        AudioManager.instance.musicVolumeChanged();
    }

    public void updateEffectsVolume()
    {
        PlayerPrefs.SetFloat("EffectsVolume", effectsVolumeSlider.value);
        AudioManager.instance.effectVolumeChanged();
    }

    private string createUname()
    {
        // Get the text asset
        TextAsset fnamesAsset = Resources.Load("fnames") as TextAsset; // Loads text file in assets folder
        string[] fnames = fnamesAsset.text.Split('\n'); // Split into lines
        Debug.Log(fnames[0]); // Print first name in list to log

        TextAsset lnamesAsset = Resources.Load("lnames") as TextAsset;
        string[] lnames = lnamesAsset.text.Split('\n');
        Debug.Log(lnames[0]);

        //Create a random username
        string uname = fnames[Random.Range(0, fnames.Length - 1)].Trim();
        uname += "-" + lnames[Random.Range(0, lnames.Length - 1)].Trim();

        Debug.Log(uname); // Log the generated name
        return uname;
    }
}
