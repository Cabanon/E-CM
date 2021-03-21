using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Character : MonoBehaviour {
    [HideInInspector]
    public bool isSelected = false;
    [HideInInspector]
    public bool isHighlighted = false;
    public DiaryController CharacterUi;
    public DiaryController spawnedUi;
    public Queue<DiaryEntry> diary;
    public Animator animator;

    public string realName;
    public CharacterJob job = CharacterJob.Student;
    public Mood mood;

    public float physicalCondition = 1f; // Will influence speed, activities, ect
    public float studious = 1; // How serious in studies
    public float social = 1; // Qualifies interactions
    public float sqrDetectionRadius = 36f;

    public float toiletNeeds = 1;
    public float foodNeeds = 1;
    public float cafeineNeeds = 1;

    public float toiletBuildup = 0;
    public float foodBuildup = 0;
    public float cafeineBuildup = 0;

    public int level;
    private int actualLevel;
    public GameObject visibility;
    private LayerMask Buildings;

    private Color color = Color.gray;

    public List<Character> friends;


    private void Start()
    {
        Buildings = LayerMask.GetMask("Buildings");
        visibility = GameObject.Find("VisibilityManagerObject");
        animator = this.gameObject.GetComponent<Animator>();
        gameObject.name = realName;
        toiletBuildup = 0;
        foodBuildup = 0;
        cafeineBuildup = 0;
        //friends = new List<Character>();
        TimeManager.instance.EveryMinuteUpdate += UpdateNeeds;
        UpdateNavMeshAgentStats();
        diary = new Queue<DiaryEntry>();
        CharacterUi = GameObject.Find("CharacterInfoCanvas").GetComponent<DiaryController>();
    }

    private void Update()
    {
        if (NeedsCafein())
        {
            animator.SetTrigger("Coffee");
        }

        actualLevel = visibility.GetComponent<VisibilityManager>().level;
        visible();
    }

    public void RandomizeCharacter(string name, CharacterJob job = CharacterJob.Student)
    {
        realName = name;
        gameObject.name = realName;
        physicalCondition = (1+Random.value)/2;
        studious = Random.value;
        social = Random.value;
        toiletNeeds = 1 + Random.value*2;
        foodNeeds = .5f + Random.value;
        cafeineNeeds = Random.value < .5f ? 1000 : 0.5f + Random.value*2; // Some people don't need cafeine and some are addicts
        UpdateNavMeshAgentStats();
        RandomColor();
        this.job = job;

        int pick = Random.Range(0, System.Enum.GetNames(typeof(Mood)).Length);
        mood = (Mood) pick;

    }

    public void UpdateNeeds()
    {
        float buildupSpeed = CharacterManager.instance.needsBuildupSpeed;
        toiletBuildup += buildupSpeed / 240; // 4 hours to reach 1
        foodBuildup += buildupSpeed / 180; // 3 hours to reach 1
        cafeineBuildup += buildupSpeed / 75; // 1h15 to reach 1
    }

    public bool NeedsFood()
    {
        return foodBuildup > foodNeeds;
    }

    public bool NeedsToilet()
    {
        return toiletBuildup > toiletNeeds;
    }

    public bool NeedsCafein()
    {
        return cafeineBuildup > cafeineNeeds;
    }

    public void ResetNeed(int needId)
    {
        switch (needId)
        {
            case 0:
                foodBuildup = 0;
                break;
            case 1:
                toiletBuildup = 0;
                break;
            case 2:
                cafeineBuildup = 0;
                break;
        }
    }

    public void RandomColor()
    {
        MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
        Renderer rend = GetComponentInChildren<Renderer>();

        rend.GetPropertyBlock(propBlock);
        color = new Color(physicalCondition, studious, social);
        propBlock.SetColor("_Color", color);
        rend.SetPropertyBlock(propBlock);
    }

    private void UpdateNavMeshAgentStats()
    {
        NavMeshAgent nav = GetComponent<NavMeshAgent>();
        if (nav != null)
            nav.speed *= physicalCondition;
    }

    public void AddDiaryEntry(string description)
    {
        DiaryEntry entry = new DiaryEntry(TimeManager.instance.timeOfDay, description);
        diary.Enqueue(entry);
    }

    public void Select()
    {
        isSelected = true;
        OnVisual();
        CharacterUi.LinkCharacter();

        MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer rend in renderers)
        {
            rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
    }

    public void Deselect()
    {
        isSelected = false;
        OffVisual();
        CharacterUi.Unlink();
    }

    public void OnVisual() //display character name and change its color
    {
        MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
        Renderer rend = GetComponentInChildren<Renderer>();
        rend.GetPropertyBlock(propBlock);
        propBlock.SetColor("_Color", color * 0.5f);
        rend.SetPropertyBlock(propBlock);
        isHighlighted = true;
    }

    public void OffVisual() // hide character name and give it back its initial color
    {
        MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
        Renderer rend = GetComponentInChildren<Renderer>();
        rend.GetPropertyBlock(propBlock);
        propBlock.SetColor("_Color", color);
        rend.SetPropertyBlock(propBlock);
        isHighlighted = false;
    }


    private void visible() 
    {
        RaycastHit hit;
        Vector3 fromPosition = gameObject.transform.position;
        Vector3 direction = new Vector3(0,-1,0);
        float distance = 50;

        bool ok = Physics.Raycast(fromPosition, direction, out hit, distance, Buildings);

        if (hit.collider.gameObject.layer == 11) //Buildings layer
        {

                string name = hit.transform.parent.name;
                int childLevel = (int)char.GetNumericValue(name[name.Length - 1]); // Extract level from parent name
                if (name[name.Length - 2] == '-')
                    childLevel *= -1;
                level = childLevel;
            

            MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer rend in renderers)
            {
                if ((level <= actualLevel) && (!isSelected))
                {
                     rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                }
                if ((level > actualLevel) && (!isSelected))
                {
                     rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                }
            }

            if (isSelected && GameObject.Find("MainCamera").GetComponent<CameraMovement>().follow)
            {
                int variable = actualLevel - level;
                if (variable < 0)
                {
                    while (variable != 0)
                    {
                        visibility.GetComponent<VisibilityManager>().ChangeLevel(+1);
                        variable += 1;
                    }
                }
                if (variable > 0)
                {
                    while (variable != 0)
                    {
                        visibility.GetComponent<VisibilityManager>().ChangeLevel(-1);
                        variable += -1;
                    }
                }
            }
        }
    }

    private void OnCollisionExit(Collision other) 
    {
        if (other.collider.gameObject.layer == 11) //buildings layer 
        {
            //level = 0;
        }
    }
}

public enum Mood { Calm, Happy, Flirty, Tired, Bored, Depressed, Sad, Angry, Hungry, Thirsty, Neutral }

 //Dictionary<string, Sprite> moods = new Dictionary<string, Sprite> {
 //   {"Calm", Resources.Load<Sprite>("Calm")},
 //   {"Happy", Resources.Load<Sprite>("Happy")}
//};

//Happy, Flirty, Tired, Bored, Depressed, Sad, Angry, Hungry, Thirsty, Neutral}

public enum CharacterJob { Student, Professor, Administration, Worker}