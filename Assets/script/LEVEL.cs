using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.ComponentModel;
public class LEVEL : MonoBehaviour
{
    public Button[] allbtn;
    int levelno=1,maxlevelno=0;
   
    // Start is called before the first frame update
    void Start()
    {
        levelno = PlayerPrefs.GetInt("levelno", 1);
        maxlevelno = PlayerPrefs.GetInt("maxlevel", 0);
        for (int i = 0; i <=maxlevelno; i++)
        {
            allbtn[i].interactable = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void home()
    {
        SceneManager.LoadScene("HOME");
    }
    public void back()
    {
        SceneManager.LoadScene("PLAY");
    }
    public void levelselect(int levelno)
    {
        PlayerPrefs.SetInt("levelno", levelno);
        SceneManager.LoadScene("PLAY");
    } 
}
