using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public float soundIntensity; //distance detection of the sound
    public float soundForce; //number of wall that can be pass by sound
    public float lifeTime;
    public int updatePeriod = 30; // Only execute computation intensive function every x frame
    private int frameCount;
    public GameObject soundEffect = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        frameCount++;
        if (frameCount >= updatePeriod)
        {
            frameCount = 0;
           // SearchTarget();
        }
    }

    void SearchTarget()
    {
        var collidersInRange = Physics.OverlapSphere(transform.position, soundIntensity);

        foreach (var currentCollider in collidersInRange)
        {
                if (currentCollider.tag == "Player")
                {

                    
                }
            }

    }

}
