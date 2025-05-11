using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class HOME : MonoBehaviour
{
    public GameObject settingpanel;
    AudioSource audio;
    public AudioClip clicksound ;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void playbtn()
    {
        SceneManager.LoadScene("PLAY");
        audio.clip = clicksound;
        audio.Play();
        settingpanel.SetActive(false);
    }
    public void levelbtn()
    {
        SceneManager.LoadScene("LEVEL");
        settingpanel.SetActive(false);
    }
    public void settingbtn()
    {
        settingpanel.SetActive(true);
    }
    public void closebtn()
    {
        settingpanel.SetActive(false);
    }
}
