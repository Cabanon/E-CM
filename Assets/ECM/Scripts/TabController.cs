using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    public bool viewToggle = false;
    public bool hearingToggle = false;
    
    public void Hearing()
    {
        viewToggle = false;
        hearingToggle = true;
    }

    public void View()
    {
        viewToggle = true;
        hearingToggle = false;
    }
 
    public void Close()
    {
        gameObject.GetComponent<DiaryController>().showInfo = false;
    }
}

