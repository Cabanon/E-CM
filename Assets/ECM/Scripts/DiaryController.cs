using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaryController : MonoBehaviour {

    private GameObject character;
    public Text name;

    public void AddEntries(Queue<DiaryEntry> entries)
    {
        foreach (DiaryEntry entry in entries)
            AddEntry(entry);
    }

    public void LinkCharacter(GameObject charac)
    {   
        character = charac;
        name.text = charac.name;
    }

	void Update () {
		
	}

    public void AddEntry(DiaryEntry entry)
    {
        //GameObject item = Instantiate(listItemPrefab, content.transform) as GameObject;
        //item.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = entry.time;
        //item.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = entry.description;
    }

}

public struct DiaryEntry
{
    public string time;
    public string description;

    public DiaryEntry(TimeOfDay time, string description)
    {
        this.time = time.ToString();
        this.description = description;
    }
}
