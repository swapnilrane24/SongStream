using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using static QuestionStruct;

public class StreamMusic : MonoBehaviour
{
    //http://ice1.somafm.com/groovesalad-128-mp3
    public string url;// "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3"
    public AudioSource source;
    public Slider slider;

    private TextAsset data;

    public QuestionStruct questionStruct;

    public Utils utils;

    void Start()
    {
        utils.getQuestion += LoadJson;
        questionStruct = new QuestionStruct();

        //StartCoroutine(LoadJson());

        slider.minValue = 0;
        slider.maxValue = 1;
        source = GetComponent<AudioSource>();
        //StartCoroutine(GetAudio());
        //StartCoroutine(GetAudioClip());

        //EventManager.GetInstance().AddListener<ActiveChallenge>(LoadJson);
    }

    private IEnumerator GetAudio()
    {
        WWW www = new WWW(url);
        StartCoroutine(ShowProgress(www));

        while (www.progress < 0.1f)
        {
            yield return new WaitForSeconds(0.1f);
        }

        if (string.IsNullOrEmpty(www.error) == false)
        {
            Debug.Log("Did not work");
            yield break;
        }

        AudioClip clip = www.GetAudioClip(false, true);
        //clip = WWWAudioExtensions.GetAudioClip(www, false, true, AudioType.OGGVORBIS);
        clip.name = "Song";
        source.clip = clip;
        Debug.Log("Loaded Clip");
    }

    IEnumerator GetAudioClip()
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.OGGVORBIS))
        {
            StartCoroutine(ShowProgress(www));
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                clip.name = "Song";
                source.clip = clip;
            }
        }
    }

    private void Update()
    {
        if (source.clip != null)
        {
            if (!source.isPlaying /*&& source.clip.isReadyToPlay*/)
                source.Play();
        }
    }

    private IEnumerator ShowProgress(WWW www)
    {
        while (!www.isDone)
        {
            slider.value = www.progress;
            //Debug.Log(string.Format("Downloaded {0:P1}", www.progress));
            yield return new WaitForSeconds(.1f);
        }
        Debug.Log("Done");
    }

    private IEnumerator ShowProgress(UnityWebRequest www)
    {
        while (!www.isDone)
        {
            slider.value = www.downloadProgress;
            //Debug.Log(string.Format("Downloaded {0:P1}", www.progress));
            yield return new WaitForSeconds(.1f);
        }
        Debug.Log("Done");
    }

    //public IEnumerator LoadJson(Action<Question[]> onComplete)
    //{
    //    ResourceRequest LoadRequest = Resources.LoadAsync("SongInfo");
    //    yield return LoadRequest;
    //    data = LoadRequest.asset as TextAsset;

    //    if (data != null)
    //    {
    //        questionStruct = JsonUtility.FromJson<QuestionStruct>(data.text);
    //    }

    //    if (questionStruct.status > 0)
    //    {
    //        for (int i = 0; i < questionStruct.status; i++)
    //        {
    //            string options = questionStruct.questions[i].options;
    //            string[] optionArry = options.Split(',');
    //            questionStruct.questions[i].option = optionArry;
    //        }
    //    }

    //    onComplete(questionStruct.questions);
    //}

    public void LoadJson(Action<Question[]> onComplete)
    {
        data = Resources.Load("SongInfo") as TextAsset;

        if (data != null)
        {
            questionStruct = JsonUtility.FromJson<QuestionStruct>(data.text);
        }

        if (questionStruct.status > 0)
        {
            for (int i = 0; i < questionStruct.status; i++)
            {
                string options = questionStruct.questions[i].options;
                string[] optionArry = options.Split(',');
                questionStruct.questions[i].option = optionArry;
            }
        }

        onComplete(questionStruct.questions);
    }
}
