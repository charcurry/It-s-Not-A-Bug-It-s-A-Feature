using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubtitleManager : MonoBehaviour
{
    public Text subtitleText;

    private static SubtitleManager _get;
    public static SubtitleManager get
    {
        get
        {
            if (_get == null)
            {
                _get = GameObject.Instantiate(Resources.Load<SubtitleManager>("SubtitleManager")).GetComponent<SubtitleManager>();
            }
            return _get;
        }
    }

    private void Start()
    {
        _get = this;
        subtitleText = GameObject.Find("SubtitleText").GetComponent<Text>();
    }

    public void ShowSubtitle(string subtitle)
    {
        subtitleText.text = subtitle;
        subtitleText.enabled = true;
    }

    public void ClearSubtitles()
    {
        subtitleText.text = "";
        subtitleText.enabled = false;
    }
}
