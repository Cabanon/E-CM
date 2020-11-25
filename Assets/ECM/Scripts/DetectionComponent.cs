using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionComponent : MonoBehaviour {

    public List<GameObject> visible;
    private float viewDistance = 5f;
    private float fieldOfView = 120;
    private LayerMask mask;

    public float detectionRadius = 5f;

    void Start () {
        mask = LayerMask.GetMask("Detectable");
    }
	
	void FixedUpdate () {
        visible = new List<GameObject>(); 
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, viewDistance, mask);
        foreach (var hitCollider in hitColliders)
        {   
            GameObject neighbour = hitCollider.gameObject;
            float angle = Vector3.Angle(transform.forward, neighbour.transform.position - transform.position);
            if (neighbour.transform != transform & angle < fieldOfView / 2) {
                visible.Add(neighbour);
            }
        }
	}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, viewDistance);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -60, 0) * transform.forward * 5);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 60, 0) * transform.forward * 5);
    }
}
