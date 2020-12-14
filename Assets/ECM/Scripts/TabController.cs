using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    public bool viewToggle = false;
    public bool hearingToggle = false;
    public bool diaryToggle = false;
    public bool needsToggle = false;

    public void Hearing()
    {
        viewToggle = false;
        hearingToggle = true;
        needsToggle = false;
        diaryToggle = false;
    }

    public void View()
    {
        viewToggle = true;
        hearingToggle = false;
        needsToggle = false;
        diaryToggle = false;
    }

    public void Diary()
    {
        viewToggle = false;
        hearingToggle = false;
        needsToggle = false;
        diaryToggle = true;
    }

    public void needs()
    {
        viewToggle = false;
        hearingToggle = false;
        needsToggle = true;
        diaryToggle = false;
    }
 
    public void Close()
    {
        gameObject.GetComponent<DiaryController>().showInfo = false;
    }
}

