using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    //variables that should be modified when the sound is instantiate (soundProperties)
    public float soundIntensity; //distance detection of the sound
    public float soundForce; //number of wall that can be pass by sound
    public float lifeTime;
    public string soundName;

    //variables necessary to make the code work
    [HideInInspector]
    public bool isSoundEffect = false;
    private ParticleSystem ps;
    private SphereCollider collid;
    public GameObject soundEffect = null;
    public GameObject soundWave;

    private void Start()
    {
        collid = this.gameObject.GetComponent<SphereCollider>();
        InitialiseSound();
    }

    public void AfficheSoundEffect () //create the soundEffect, this function is used by the Script SoundSystem
    {
       if (soundEffect == null)
        {
            soundEffect = Instantiate(soundWave, transform.gameObject.transform.parent.position + soundWave.transform.position, soundWave.transform.rotation, transform);
            ps = soundEffect.GetComponent<ParticleSystem>();
            var main = ps.main;
            main.startSize = soundIntensity;
        }
    }

    public void DestroySoundEffect() //destroy the soundEffect, this function is used by the Script SoundSystem
    {
        if (soundEffect != null) {
            Destroy(soundEffect);
            soundEffect = null;
        }
    }

    public void InitialiseSound () //initialise some sound parameters this function is called by start but must also be called when the sound is created, after the modification of the properties
    {
        collid.radius = soundIntensity;
        if (lifeTime > 0)
        {
            Destroy(this.gameObject, lifeTime);
        }
    }
}
