using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour {
	public GameObject target;

	// Use this for initialization
	void Start () {
		transform.LookAt (target.transform.position + Vector3.up);
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.LookAt (target.transform.position + Vector3.up);
	}
}
