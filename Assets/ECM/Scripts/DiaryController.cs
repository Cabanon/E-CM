using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaryController : MonoBehaviour {

    private GameObject character;
    public Text name;
    public RectTransform content;

    private GameObject owner;

    public void AddEntries(Queue<DiaryEntry> entries, GameObject owner)
    {
        if (!GameObject.ReferenceEquals(this.owner, owner))
            return;

        foreach (DiaryEntry entry in entries)
            AddEntry(entry, owner);
    }

    public void LinkCharacter(GameObject charac)
    {   
        character = charac;
        name.text = charac.name;
    }

	void Update () {
        List<GameObject> visible = character.GetComponent<DetectionComponent>().getVisibleNeighbours();
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
		foreach (var charac in visible) {   
            AddName(charac.name);
        }
	}

    public void AddEntry(DiaryEntry entry, GameObject owner)
    {
        //GameObject item = Instantiate(listItemPrefab, content.transform) as GameObject;
        //item.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = entry.time;
        //item.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = entry.description;
    }

    void AddName(string name)
    {
        GameObject item = new GameObject();
        item.AddComponent<Text>();
        item.GetComponent<Text>().text = name;
        item.transform.SetParent(content);
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