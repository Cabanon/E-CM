using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class googly : MonoBehaviour
{
    public GameObject eye1;
    public GameObject eye2;

    // Start is called before the first frame update
    void Start()
    {
        float val1 = Random.Range( -0.05f,  0.05f);
        float val2 = Random.Range(-0.05f, 0.05f);
        float val3 = Random.Range(-0.05f, 0.05f);
        float val4 = Random.Range(-0.05f, 0.05f);

        eye1.transform.localPosition += new Vector3(val1, val2, 0f);
        eye2.transform.localPosition += new Vector3(val3, val4, 0f);
    }

 
}
