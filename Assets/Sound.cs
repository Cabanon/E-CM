using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public float soundIntensity; //distance detection of the sound
    public float soundForce; //number of wall that can be pass by sound
    public float lifeTime;

    public bool isSoundEffect = false;
    public GameObject soundEffect = null;
    public GameObject soundWave;

    public void AfficheSoundEffect ()
    {
       if (soundEffect == null)
        {
            soundEffect = Instantiate(soundWave, transform.gameObject.transform.parent.position + soundWave.transform.position, soundWave.transform.rotation, transform.gameObject.transform.parent);
        }
    }

    public void DestroySoundEffect()
    {
        if (soundEffect != null) {
            Destroy(soundEffect);
            soundEffect = null;
        }
    }
}
