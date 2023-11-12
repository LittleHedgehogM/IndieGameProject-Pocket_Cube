using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

//using UnityEngine.Video;

public class SubtitleBlock: MonoBehaviour
{
    //public float offset;
    List<SubtitleBlock> subt;
    public Text subtitletext;
    //public VideoPlayer vp;
    //string subcontent;
    //public TextAsset subTitleSource;
    public TMP_Text testText;
    private int index = 0;
    public string subtitle_file_name;
    List<String> words;

    void Start()
    {
        TextAsset mytxtData = Resources.Load("Textimg/" + subtitle_file_name) as TextAsset;
        //string test = mytxtData.text;

        //testText.text = subtitletext;
        testText.text = "Here is typed text";

        words = new List<string>(mytxtData.text.Split('\n'));
        Debug.Log(words[0]);

        //subTitleSource = mytxtData;
        /* if (subTitleSource == null) return;
         subt = SubtitleBlock.ParseSubtitles(subTitleSource.text);
         StartCoroutine(DisplaySubtitles());*/
        //StartCoroutine(playvideo());
    }

    public float localtimer;
    void Update()
    {
        //if (vp != null && vp.isPlaying)
        //localtimer += offset;
        //testText.text = DateTime.Now.ToString();
        testText.text = words[index];
        localtimer += Time.deltaTime;
        if (localtimer >= 5)
        {
            if(index <= words.Count)
            {
                index++;
            }
            localtimer = 0;
        }
    }

    public IEnumerator DisplaySubtitles()
    {
        for (int j = 0; j < subt.Count; j++)
        {
            var i = subt[j];
            subtitletext.text = "";
            if (i.From <= localtimer && i.To >= localtimer)
            {
                subtitletext.text = i.Text;
                yield return new WaitForSeconds((float)i.Length);

            }
            else if (i.From > localtimer)
            {
                yield return new WaitForSeconds(Mathf.Min((float)i.From - localtimer, 0.1f));
                j--;
            }
        }
        subtitletext.text = "";
        yield return null;
    }

    public int Index { get; private set; }
    public double Length { get; private set; }
    public double From { get; private set; }
    public double To { get; private set; }
    public string Text { get; private set; }

    public SubtitleBlock(int index, double from, double to, string text)
    {
        this.Index = index;
        this.From = from;
        this.To = to;
        this.Length = to - from;
        this.Text = text;
    }
    public override string ToString()
    {
        return "Index: " + Index + " From: " + From + " To: " + To + " Text: " + Text;
    }
    static public List<SubtitleBlock> ParseSubtitles(string content)
    {
        var subtitles = new List<SubtitleBlock>();
        var regex = new Regex($@"(?<index>\d*\s*)\n(?<start>\d*:\d*:\d*,\d*)\s*-->\s*(?<end>\d*:\d*:\d*,\d*)\s*\n(?<content>.*)\n(?<content2>.*)\n");
        var matches = regex.Matches(content);
        foreach (Match match in matches)
        {
            var groups = match.Groups;
            int ind = int.Parse(groups["index"].Value);
            TimeSpan fromtime, totime;
            TimeSpan.TryParse(groups["start"].Value.Replace(',', '.'), out fromtime);
            TimeSpan.TryParse(groups["end"].Value.Replace(',', '.'), out totime);
            string contenttext = groups["content"].Value;
            subtitles.Add(new SubtitleBlock(ind, fromtime.TotalSeconds, totime.TotalSeconds, contenttext));
        }
        return subtitles;
    }
}
