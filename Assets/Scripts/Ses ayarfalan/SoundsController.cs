using UnityEngine;
using UnityEngine.UI;

public class SoundsController : MonoBehaviour
{
    public Slider volumeSlider;

    public AudioSource[] sesliSeyler;

    public GameObject MainMusic;
    

    private void Start()
    {
        PlayerPrefs.SetFloat("anan", 0.30f);
        volumeSlider.value = PlayerPrefs.GetFloat("anan");

        volumeSlider.value = 0.3f;
        for (int i = 0; i < sesliSeyler.Length; i++)
        {
            sesliSeyler[i].volume = volumeSlider.value;
        }
        MainMusic.GetComponent<AudioSource>().volume = volumeSlider.value;

    }

    private void Update()
    {
        
        if (MainMusic == null)
        {
            MainMusic = GameObject.Find("Music");
        }
    }


    public void OnVolumeChanged() 
    {
        for (int i = 0; i < sesliSeyler.Length; i++)
        {
            sesliSeyler[i].volume = volumeSlider.value;
            PlayerPrefs.SetFloat("anan", volumeSlider.value);
        }
        MainMusic.GetComponent<AudioSource>().volume = volumeSlider.value;

    }

}
