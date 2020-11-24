using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    public int updatePeriod = 30; // Only execute computation intensive function every x frame
    private int frameCount;
    private LayerMask mask;
    private Character character;
    private bool isSelected;
    public GameObject soundWave;

    private Vector3 orientation = new Vector3 (90, 0 , 0);
    public int range=1;

   // private Collider[] soundsCache; // Will store result of OverlapSphereNonAlloc function
    //private int soundsMaxNumber = 100;
    //public GameObject[] sounds;

    // Start is called before the first frame update
    void Start()
    {
        mask = LayerMask.GetMask("Sound");
        //sounds = new GameObject[soundsMaxNumber]; // Maximum of 10 neighbours
        //soundsCache = new Collider[soundsMaxNumber];
        frameCount = UnityEngine.Random.Range(0, updatePeriod - 1); // Random offset in frame count so that all character update in different frames
        character = gameObject.GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        isSelected = character.isSelected;
        frameCount++;
        if (frameCount >= updatePeriod)
        {
            frameCount = 0;
            SearchTarget();
        }
    }

    void SearchTarget()
    {
        Collider[] collidersInRange = Physics.OverlapSphere(transform.position, range, mask);

      
        if (isSelected == true)
        {
            foreach (var currentCollider in collidersInRange)
            {
               GameObject isSound = currentCollider.gameObject.GetComponent<Sound>().soundEffect;
                if ( isSound == null )
                {
                    isSound = Instantiate(soundWave, currentCollider.transform.position + soundWave.transform.position, soundWave.transform.rotation, currentCollider.transform);
                    Destroy(isSound, 2f);
                    Invoke("isSound = null", 2f);
                }
                
            }
        }
        

    }

}