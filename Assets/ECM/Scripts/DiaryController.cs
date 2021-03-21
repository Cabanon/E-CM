using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaryController : MonoBehaviour {

    private Character character;
    public Text title;
    public RectTransform content;
    public GameObject listItem;

    public Image coffeeBar;
    public Image toiletBar;
    public Image foodBar;

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
    private GameObject selectionManager;
    public Text texte_prefab;


    private void Start()
    {
        selectionManager = GameObject.Find("SelectionManager");
        Toggle();
    }

    public void AddEntries(Queue<DiaryEntry> entries, GameObject owner)
    {
        if (!GameObject.ReferenceEquals(this.owner, owner))
            return;

        foreach (DiaryEntry entry in entries)
            AddEntry(entry, owner);
    }

    public void LinkCharacter()
    {   
        character = selectionManager.GetComponent<SelectionManager>().selectedCharacter;
        title.text = character.gameObject.GetComponent<Character>().realName + "\n(" + character.gameObject.GetComponent<Character>().job.ToString()+")";
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
            updateNeeds();
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
        Text texte = Instantiate(texte_prefab, content);
        texte.text = name;
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
                AddName(charac.gameObject.GetComponent<Character>().realName);
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

    private void updateNeeds()
    {
        toiletBar.fillAmount = character.GetComponent<Character>().toiletBuildup;
        foodBar.fillAmount = character.GetComponent<Character>().foodBuildup;
        coffeeBar.fillAmount = character.GetComponent<Character>().cafeineBuildup;
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