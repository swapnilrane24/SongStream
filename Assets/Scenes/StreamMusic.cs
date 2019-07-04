using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StreamMusic : MonoBehaviour
{
    public string url;
    public AudioSource source;
    public Slider slider;

    private TextAsset data;

    public QuestionStruct questionStruct;

    void Start()
    {
        questionStruct = new QuestionStruct();

        StartCoroutine(LoadJson());

        slider.minValue = 0;
        slider.maxValue = 1;
        source = GetComponent<AudioSource>();
        StartCoroutine(GetAudio(url));
    }

    private IEnumerator GetAudio(string url)
    {
        WWW www = new WWW(url);
        StartCoroutine(ShowProgress(www));
        yield return www;
        if (string.IsNullOrEmpty(www.error) == false)
        {
            Debug.Log("Did not work");
            yield break;
        }

        source.clip = www.GetAudioClip(false, true);
        Debug.Log("Loaded Clip");
    }

    private void Update()
    {
        if (source.clip != null)
        {
            if (!source.isPlaying && source.clip.isReadyToPlay)
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

    IEnumerator LoadJson()
    {
        ResourceRequest LoadRequest = Resources.LoadAsync("SongInfo");
        yield return LoadRequest;
        data = LoadRequest.asset as TextAsset;

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
    }
}
