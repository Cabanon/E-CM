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
        //Debug.Log(transform.gameObject.GetComponentInParent<Character>().isSelected);
    }

    // Update is called once per frame
    void Update()
    {
        isSelected = transform.gameObject.GetComponentInParent<Character>().isSelected;

        if (isSelected)
        {
            foreach(GameObject sound in sounds)
            {
                sound.transform.gameObject.GetComponent<Sound>().AfficheSoundEffect();
            }
            wasSelected = true;
        }

        if (wasSelected)
        {
            if (isSelected == false)
            {
                Debug.Log("Ta mère en slip de guerre");
                foreach (GameObject sound in sounds)
                {
                    sound.transform.gameObject.GetComponent<Sound>().DestroySoundEffect();
                }
                wasSelected = false;
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10) //Put 10 because mask.value() didn't work and was equal to 1024 but why ???
        {
            sounds.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == mask)
        {
            sounds.Remove(other.gameObject);
        }
    }

}