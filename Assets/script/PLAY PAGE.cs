using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PLAYPAGE : MonoBehaviour
{
    public GameObject settingpanelpage;
    public GameObject looserpage;
    public GameObject winnerpanel;

    // audio 
    public AudioClip levelopen, btnclicksound,matchaudio,unmatchaudio;
    AudioSource audio;
    bool muted = false;


    // image array
    public Sprite[] allimages;
    // random image list
    List<int> randomimagelist = new List<int>();
    // final image list  
    List<int> finallist = new List<int>();
    // make button prefab
    public GameObject btnprefeb,parentholder;
    // card pair and totalcards 
    int totalcard = 12 , totalpair = 0 ;
     
    bool firsttry =false ,secondtry=false;
    string firstpuzzel,secondpuzzle;
    int fclick, sclick;
    GameObject firstobj,secondobj;
   
    // time variable
    float starttime = 60f;
    public Text timetext;
    float currenttime;
    bool runnigtime = false;

    // card view time line
    float delayTime = 3f;
    int cnt = 0,time=20;

    // level 
    int levelno=1,maxlevelno=0,score=0;
    public Text scorepoint;
    public Text levelvnoprint;

    // grid
    GridLayoutGroup gg;
    int[,] boxArr = { { 400, 400 }, { 320,320 }, { 240, 240 }, { 160, 160 }, { 80, 80 }, { 40, 40 }, { 100, 100 } };
                             //0              1              2            3          4           5           6
    int[] levelxnt = { 12, 14, 16, 18, 20, 22, 24 };
    int[] clickcnt = { 6,7,8,9,10,11,12};

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("muted"))
        {
            PlayerPrefs.SetInt("mute", 0);
        }
        else
        {
            muted = PlayerPrefs.GetInt("muted") == 1;
        }
        audio =GetComponent<AudioSource>();

        score = PlayerPrefs.GetInt("score",score);
        scorepoint.text = "" + score;
        levelno = PlayerPrefs.GetInt("levelno", 1);
        levelvnoprint.text = "level " +levelno;
        maxlevelno = PlayerPrefs.GetInt("maxlevel", 0);
        totalcard = levelxnt[levelno - 1];
        //imagefirstview();
        gg = parentholder.GetComponent<GridLayoutGroup>();
        int curcellsizeX = boxArr[levelno - 1, 0];
        int curcellsizeY = boxArr[levelno - 1, 1];
        //grid size
        gg.cellSize = new Vector2(curcellsizeX,curcellsizeY);
        //space size
        gg.spacing = new Vector2(50, 50);
        // show card

        totalpair = totalcard / 2;
        for(int i=0;i<totalpair;i++)
        {
            int r = Random.Range(0, allimages.Length);
            while(randomimagelist.Contains(r))
            {
                r = Random.Range(0, allimages.Length);
            }
            randomimagelist.Add(r);
        }
        finallist.AddRange(randomimagelist);
        finallist.AddRange(randomimagelist); 

        shufflevalue();
        btngenrate();
        StartCoroutine(timestartline());

    }
    IEnumerator timecountdown()
    {
     
        while(true)
        {
            
            yield return new WaitForSeconds(1f);
            if (cnt != clickcnt[levelno-1])
            {
                time = time - 1;
                timetext.text = "TIME" + time;
                if (time == 0)
                {
                    looserpage.SetActive(true);
                    settingpanelpage.SetActive(false);
                    break;
                }
            }
            else
            {
                time = time - 0;
            }
        }
    }
    IEnumerator timestartline()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(timecountdown());
    }
    void shufflevalue()
    {
        for(int j=0;j<finallist.Count;j++)
        {
            int i = Random.Range(0,finallist.Count);
            int tampNo=finallist[i];
            finallist[i] = finallist[j];
            finallist[j] = tampNo;
        }
    }
    IEnumerator btnclicktime(GameObject p, int imgNo)
    {
        yield return new WaitForSeconds(3f);
        p.GetComponent<Button>().onClick.AddListener(() => btnclick(imgNo, p));
    }
    void btngenrate()
    {
        for(int m=0;m<finallist.Count;m++)
        {
            GameObject p = Instantiate(btnprefeb, parentholder.transform);
            int imgNo = finallist[m];
            StartCoroutine(btnclicktime(p, imgNo));       
            StartCoroutine(imagefirstview(p, imgNo));
        }
      //  StartCoroutine(imagefirstview());
    }
    IEnumerator imagefirstview(GameObject p, int imgNo)
    {
        p.GetComponent<Image>().sprite = allimages[imgNo];
        yield return new WaitForSeconds(3f);
        p.GetComponent<Image>().sprite = default;
    }
    void btnclick(int imageNo,GameObject p)
    {
        print(imageNo);
        p.GetComponent<Image>().sprite = allimages[imageNo];
        if (!firsttry)
        {
            firsttry = true;
            fclick = imageNo;
            audio.clip = btnclicksound;
            audio.Play();
            firstobj = p;
            /*firstpuzzel = allimages[Fckick].ToString();*/
            firstobj.GetComponent<Button>().interactable = false;
        }
        else if (!secondtry)
        {
            secondtry = true;
            sclick = imageNo;
            audio.clip = btnclicksound;
            audio.Play();
            secondobj = p;
            secondobj.GetComponent<Button>().interactable = false;
            if (fclick == sclick)
            {
                print("match");
                audio.clip = matchaudio;
                audio.Play();
                firsttry = false;
                secondtry = false;
                fclick = -1;
                sclick = -1;
                cnt++;
                time += 5;
                print("cnt"+cnt);
                if (cnt == clickcnt[levelno-1])
                { 
                    if(levelno>maxlevelno)
                    {
                        PlayerPrefs.SetInt("maxlevel", levelno);
                        PlayerPrefs.SetInt("score", score + 20);
                        scorepoint.text = ""+score;
                    }
                    winnerpanel.SetActive(true);
                    audio.clip = levelopen;
                    audio.Play();
                }
                
            }
            else
            {
                print("not match");
                audio.clip = unmatchaudio;
                audio.Play();
                firsttry= false;    
                secondtry= false;
                fclick = -1;
                sclick = -1;
                StartCoroutine(pictureopentimezone(firstobj,secondobj));
                firstobj.GetComponent<Button>().interactable = true;
                secondobj.GetComponent<Button>().interactable=true;
            }
        }
    }
    
    // picture matching time 
    IEnumerator pictureopentimezone(GameObject p,GameObject j)
    {
        yield return new WaitForSeconds(1.2f);
        p.GetComponentInChildren<Image>().sprite = default;
        j.GetComponentInChildren<Image>().sprite = default;
    }
  
    public void settingpanel()
    {
        settingpanelpage.SetActive(true);
    }
    public void panelcontrol()
    {
        settingpanelpage.SetActive(false);
    }
    public void restart()
    {
        looserpage.SetActive(false);
        SceneManager.LoadScene("PLAY");
    }
    public void nextlevel()
    { 
        winnerpanel.SetActive(false);
        SceneManager.LoadScene("PLAY");
        PlayerPrefs.SetInt("levelno", levelno + 1);
    }
    public void home()
    {
        SceneManager.LoadScene("HOME");
    }
    public void volume()
    {
        if (muted == false)
        {
            muted = true;
            AudioListener.pause = true;
        }
        else
        {
            muted = false;
            AudioListener.pause = false;
        }
        PlayerPrefs.SetInt("muted", muted ? 1 : 0);
    }
    // Update is called once per frame
    void Update()
    {
       
    }
}