using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
	public float speed = 6f;            // The speed that the player will move at.
	public Text displayText;
	public Camera cam;
	
	Vector3 movement;                   // The vector to store the direction of the player's movement.
	private Animator anim;              // Reference to the animator component.
	Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
	int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
	float camRayLength = 100f;          // The length of the ray from the camera into the scene.

	void Awake ()
	{
		// Create a layer mask for the floor layer.
		floorMask = LayerMask.GetMask ("Floor");
		
		// Set up references.
		anim = GetComponent <Animator> ();
		playerRigidbody = GetComponent <Rigidbody> ();

		if (anim.layerCount >= 2) {
			anim.SetLayerWeight(1,1);
		}
	}
	
	
	void FixedUpdate ()
	{
		// Store the input axes.
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");

		
		// Move the player around the scene.
		Move (h, v, cam.transform);
		
		// Turn the player to face the mouse cursor.
		//Turning ();
		
		// Animate the player.
		Animating (h, v);
		displayText.text = ((transform.position - cam.transform.position).magnitude).ToString();
	}
	
	void Move (float h, float v, Transform tCam)
	{	
		float norm2 = h * h + v * v;
		float norm = Mathf.Sqrt (norm2);

		Vector3 newV = Vector3.ProjectOnPlane (tCam.forward, Vector3.down);
		Vector3 newH = Vector3.ProjectOnPlane (tCam.right, Vector3.up);



		// Set the movement vector based on the axis input.
		movement = newV * v + newH * h;

		
		// Normalise the movement vector and make it proportional to the speed per second.
		movement = movement.normalized * speed * Time.deltaTime;
		
		// Move the player to it's current position plus the movement.
		playerRigidbody.MovePosition (transform.position + movement);
		//playerRigidbody.MoveRotation (Quaternion.Euler (playerRigidbody.rotation.eulerAngles + new Vector3(0f, 2*h,0f)));

		if (movement != Vector3.zero) {
			Quaternion newR = Quaternion.LookRotation(movement);
			playerRigidbody.transform.rotation = Quaternion.Slerp (playerRigidbody.transform.rotation, newR, Time.deltaTime * 8);
		}

	}
	
	void Turning ()
	{

		// Create a ray from the mouse cursor on screen in the direction of the camera.
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		
		// Create a RaycastHit variable to store information about what was hit by the ray.
		RaycastHit floorHit;
		
		// Perform the raycast and if it hits something on the floor layer...
		if(Physics.Raycast (camRay, out floorHit, camRayLength, floorMask))
		{
			// Create a vector from the player to the point on the floor the raycast from the mouse hit.
			Vector3 playerToMouse = floorHit.point - transform.position;
			
			// Ensure the vector is entirely along the floor plane.
			playerToMouse.y = 0f;
			
			// Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
			Quaternion newRotation = Quaternion.LookRotation (playerToMouse);
			
			// Set the player's rotation to this new rotation.
			playerRigidbody.MoveRotation (newRotation);
		}
	}
	
	void Animating (float h, float v)
	{
		// Create a boolean that is true if either of the input axes is non-zero.
		bool walking = (h != 0f ||v != 0f);
		
		// Tell the animator whether or not the player is walking.
		anim.SetBool ("IsWalking", walking);
	}
}