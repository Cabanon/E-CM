using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    private LayerMask mask;
    private bool isSelected;
    private bool wasSelected = false;
    public List<GameObject> sounds = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        mask = LayerMask.GetMask("Sound");
    }

    // Update is called once per frame
    void Update()
    {
        isSelected = transform.gameObject.GetComponentInParent<Character>().isSelected;

        if (isSelected) 
        {
            foreach(GameObject sound in sounds) //display the soundWave sound effect for the selected character
            {
                sound.transform.gameObject.GetComponent<Sound>().AfficheSoundEffect();
            }
            wasSelected = true;
        }

        if (wasSelected) //remove the soundWave sound effect when the character is unselected
        {
            if (isSelected == false)
            {
                foreach (GameObject sound in sounds)
                {
                    sound.transform.gameObject.GetComponent<Sound>().DestroySoundEffect();
                }
                wasSelected = false;
            }
        }
    }



    private void OnTriggerEnter(Collider other) // add the sound in range in the sounds list
    {
        if (other.gameObject.layer == 10) //Put 10 because mask.value() didn't work and was equal to 1024 but why ???
        {
            sounds.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) // remove the sound not in range in the sounds list
    {
        if (other.gameObject.layer == 10) //Put 10 because mask.value() didn't work and was equal to 1024 but why ???
        {
            if (isSelected)
            {
                other.transform.gameObject.GetComponent<Sound>().DestroySoundEffect();
            }
            sounds.Remove(other.gameObject);
        }
    }

}