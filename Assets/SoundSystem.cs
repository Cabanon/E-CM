using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    private LayerMask mask;
    private LayerMask Buildings;
    private bool isSelected;
    private bool wasSelected = false;
    public List<GameObject> sounds = new List<GameObject>();
    public List<GameObject> earedSounds = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        mask = LayerMask.GetMask("Sound");
        Buildings = LayerMask.GetMask("Buildings");
    }

    // Update is called once per frame
    void Update()
    {
        sounds.RemoveAll(item => item == null);
        earedSounds.RemoveAll(item => item == null);
        earing();

        isSelected = transform.gameObject.GetComponentInParent<Character>().isSelected;
        showSounds();

    }



    private void OnTriggerEnter(Collider other) // add the sound in range in the sounds list
    {
        if (other.gameObject.layer == 10) //Put 10 (value of Sound Layer) because mask.value() didn't work and was equal to 1024 but why ???
        {
            sounds.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) // remove the sound not in range in the sounds list
    {
        if (other.gameObject.layer == 10) //Put 10 (value of Sound Layer) because mask.value() didn't work and was equal to 1024 but why ???
        {
            if (isSelected)
            {
                other.transform.gameObject.GetComponent<Sound>().DestroySoundEffect();
            }
            sounds.Remove(other.gameObject);
            earedSounds.Remove(other.gameObject);
        }
    }

    private void earing()
    {
        foreach (GameObject sound in sounds)
        {
            RaycastHit[] hits;
            Vector3 fromPosition = gameObject.transform.position;
            Vector3 toPosition = sound.transform.position;
            Vector3 direction = toPosition - fromPosition;
            float distance = Vector3.Distance(fromPosition, toPosition);

            hits = Physics.RaycastAll(fromPosition,direction, distance, Buildings);
            int wallNumber = hits.Length;
            int soundForce = sound.GetComponent<Sound>().soundForce;

            if (wallNumber <= soundForce)
            {
                earedSounds.Add(sound);
            }

            if (wallNumber > soundForce)
            {
                earedSounds.Remove(sound);

                if (isSelected)
                {
                    sound.transform.gameObject.GetComponent<Sound>().DestroySoundEffect();
                }
            }
        }
    }

    private void showSounds()
    {
        if (isSelected)
        {
            foreach (GameObject sound in earedSounds) //display the soundWave sound effect for the selected character
            {
                sound.transform.gameObject.GetComponent<Sound>().AfficheSoundEffect();
            }
            wasSelected = true;
        }

        if (wasSelected) //remove the soundWave sound effect when the character is unselected
        {
            if (isSelected == false)
            {
                foreach (GameObject sound in earedSounds)
                {
                    sound.transform.gameObject.GetComponent<Sound>().DestroySoundEffect();
                }
                wasSelected = false;
            }
        }
    }

}