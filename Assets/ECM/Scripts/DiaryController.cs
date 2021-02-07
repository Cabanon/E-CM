using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaryController : MonoBehaviour {

    private GameObject character;
    public Text title;
    public RectTransform content;
    public GameObject listItem;

    private GameObject owner;

    [HideInInspector]
    public bool viewToggle = false;
    [HideInInspector]
    public bool hearingToggle = false;
    [HideInInspector]
    public bool diaryToggle = false;
    [HideInInspector]
    public bool needsToggle = false;
    [HideInInspector]
    public bool showInfo = true;
    public GameObject info;

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
        title.text = charac.name + "\n(" + character.GetComponent<Character>().job.ToString()+")";
        showInfo = true;
    }

    public void Unlink() {
        character = null;
        title.text = "No character selected";
    }

    public void Toggle() {
        showInfo = !showInfo;
    }

	void Update () 
    {
        if (Input.GetButtonDown("Info"))
        {
            Toggle();
        }

        info.SetActive(showInfo);

        if (character)
        {
            viewToggle = gameObject.GetComponent<TabController>().viewToggle;
            hearingToggle = gameObject.GetComponent<TabController>().hearingToggle;
            diaryToggle = gameObject.GetComponent<TabController>().diaryToggle;
            needsToggle = gameObject.GetComponent<TabController>().needsToggle;

            showHearing();
            showView();
            showDiary();
            showNeeds();
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
        GameObject item = Instantiate(listItem);
        item.GetComponent<Text>().text = name;
        item.transform.SetParent(content);
    }

    public void SetOwner(GameObject owner)
    {
        this.owner = owner;
    }

    private void showView()
    {
        if (viewToggle)
        {
            List<GameObject> visible = character.GetComponent<DetectionComponent>().getVisibleNeighbours();
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }
            foreach (var charac in visible)
            {
                AddName(charac.name);
            }
        }
    }

    private void showHearing()
    {
        if (hearingToggle)
        {
            List<GameObject> audible = character.GetComponentInChildren<SoundSystem>().earedSounds;
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }
            foreach (var sound in audible)
            {
                AddName(sound.gameObject.GetComponent<Sound>().soundName);
            }
        }
    }

    private void showDiary()
    {
        if (diaryToggle)
        {
            Queue<DiaryEntry> diary = character.GetComponent<Character>().diary;

            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }
            foreach (DiaryEntry entry in diary)
            {
                string entree = entry.time + " : " + entry.description;
                AddName(entree);
            }
        }
    }

    private void showNeeds()
    {
        if (needsToggle)
        {
            float toiletBuildup = Mathf.Round(character.GetComponent<Character>().toiletBuildup*100);
            float foodBuildup = Mathf.Round(character.GetComponent<Character>().foodBuildup * 100);
            float cafeineBuildup = Mathf.Round(character.GetComponent<Character>().cafeineBuildup * 100);

            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }

            string toiletNeed = "Toilet Need : " + toiletBuildup.ToString() + "/100";
            string foodNeed = "Food Need : " + foodBuildup.ToString() + "/100";
            string cafeintNeed = "Cafein Need : " + cafeineBuildup.ToString() + "/100";

            AddName(toiletNeed);
            AddName(foodNeed);
            AddName(cafeintNeed);
        }
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