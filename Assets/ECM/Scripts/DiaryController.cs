using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiaryController : MonoBehaviour {

    public GameObject content;
    public GameObject listItemPrefab;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI statusText;

    private GameObject owner;

    public void AddEntries(Queue<DiaryEntry> entries, GameObject owner)
    {
        if (!GameObject.ReferenceEquals(this.owner, owner))
            return;

        foreach (DiaryEntry entry in entries)
            AddEntry(entry, owner);
    }

    public void UpdateNameAndStatus(string name, string status)
    {
        this.nameText.text = name;
        this.statusText.text = status;
    }

	void Update () {
		
	}

    public void AddEntry(DiaryEntry entry, GameObject owner)
    {
        if (!GameObject.ReferenceEquals(this.owner, owner))
            return;

        GameObject item = Instantiate(listItemPrefab, content.transform) as GameObject;
        item.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = entry.time;
        item.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = entry.description;
    }

    public void ResetEntries()
    {
        for(int i = 0; i < content.transform.childCount; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
    }

    public void SetOwner(GameObject owner)
    {
        this.owner = owner;
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