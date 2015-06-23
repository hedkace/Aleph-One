using UnityEngine;
using System.Collections;

public class CamCon : MonoBehaviour {

	public GameObject target;
	public int waitTime;
	public float angularSpeed;
	public float keepDistance;

	private Transform tTarget;
	private Animator tAnim;
	
	private Vector3 focus;
	private Vector3 oldPos;
	private int t;





	// Use this for initialization
	void Start () {
		tAnim = target.GetComponent<Animator> ();
		t = 0;
		tTarget = target.transform;
		oldPos = tTarget.position;
		focus = oldPos - transform.position;

	}
	
	// Update is called once per frame
	void LateUpdate () {
		focus = tTarget.position - transform.position;
		if (focus.magnitude >= keepDistance) {
			focus = focus.normalized*keepDistance;
			transform.position = tTarget.position - focus;
		} else if (!tAnim.GetBool("IsWalking")) {
			t++;
			if (t > waitTime) {
				t = waitTime;
				Vector3 temp = tTarget.transform.forward.normalized;
				temp *= focus.magnitude;
				focus = Vector3.RotateTowards(focus,temp,angularSpeed,focus.magnitude);
				transform.position = tTarget.position - focus;

			}
		} else {
			t = 0;
		}
	
	}
}
