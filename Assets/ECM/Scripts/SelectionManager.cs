using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour {

    public static SelectionManager instance;
    public Camera cam;
    public Character selectedCharacter = null;
    public bool center;
    private LayerMask mask;
    private Character character;
    private Character visualCharacter = null;
    private GameObject diary;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        cam = Camera.main;
        mask = LayerMask.GetMask("Detectable");
        diary = GameObject.Find("CharacterInfoCanvas");
    }

    private void Update()
    {
        RaycastHit[] hits;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        hits = Physics.RaycastAll(ray, mask);

        var distance = 0f;
        character = null;
        foreach (RaycastHit hit in hits) //select the closest character below the mouse cursor
        {
            if (1 / hit.distance > distance && hit.collider.gameObject.tag == "Player")
            {
                distance = hit.distance;
                character = hit.collider.gameObject.GetComponent<Character>();
            }
        }

        if (visualCharacter != character) //show and hide character name
        {
            if (visualCharacter != null || character == null) { if (visualCharacter.isHighlighted == true && visualCharacter.isSelected == false) { visualCharacter.OffVisual(); } }
            visualCharacter = character;
            if (visualCharacter != null) { if (visualCharacter.isHighlighted == false) { visualCharacter.OnVisual(); } }
        }


        if (Input.GetMouseButtonDown(0)) //select character
        {
            if (character != null)
            {
                if (selectedCharacter != null)
                    selectedCharacter.Deselect();
                Camera.main.GetComponent<CameraMovement>().center = true;
                selectedCharacter = character;
                character.Select();
                diary.GetComponent<DiaryController>().showInfo = true;
                
            }
        }
    }

}
