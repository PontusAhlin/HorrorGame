using UnityEngine;
using System.Collections;

namespace Footsteps {

	[RequireComponent(typeof(Rigidbody), typeof(Animator))]
	public class TopDownController : MonoBehaviour {

		[SerializeField] Transform cameraPivot;
		[SerializeField] float jogSpeed = 5f;
		[SerializeField] float rotationSpeed = 270f;
		[SerializeField] float turningOnSpotRotationSpeed = 360f;

		Transform thisTransform;
		Animator thisAnimator;
		Rigidbody thisRigidbody;

		AnimatorStateInfo currentLocomotionInfo;
		Quaternion targetRotation;
		Vector3 movementDirection;
		Vector2 directionalInput;
		float moveSpeed;
		bool turningOnSpot;
		bool move;


		void Start() {
			thisTransform = transform;
			thisAnimator = GetComponent<Animator>();
			thisRigidbody = GetComponent<Rigidbody>();

			if(!thisAnimator || !thisRigidbody) {
				Debug.LogError("Please assign both a rigidbody and an animator to this gameobject, top down controller will not function.");
				enabled = false;
			}
		}

		void FixedUpdate() {
			UpdateAnimator();
			RotateCharacter();
			MoveCharacter();
			print(directionalInput);
		}

		void UpdateAnimator() {
			currentLocomotionInfo = thisAnimator.GetCurrentAnimatorStateInfo(0);

			// Get player input
			directionalInput.x = Input.GetAxisRaw("Horizontal");
			directionalInput.y = Input.GetAxisRaw("Vertical");
			moveSpeed = Mathf.Clamp01(directionalInput.magnitude);
			moveSpeed += (moveSpeed > 0f ? (Input.GetKey(KeyCode.LeftShift) ? 1f : 0f) : 0f);
			move = Input.GetButton("Horizontal") || Input.GetButton("Vertical");

			// Handle the locomotion animations
			thisAnimator.SetFloat("move_speed", moveSpeed, 0.3f, Time.fixedDeltaTime);
			thisAnimator.SetBool("move", move);
		}

		void MoveCharacter() {
			Vector3 velocity = thisTransform.forward * moveSpeed * jogSpeed;
			velocity.y = thisRigidbody.velocity.y;
			thisRigidbody.velocity = velocity;
		}

		void RotateCharacter() {
			movementDirection = cameraPivot.right * directionalInput.x + cameraPivot.forward * directionalInput.y;
			bool inIdle = currentLocomotionInfo.IsName("idle");
			float deltaAngle = 0f;
			float targetRotationSpeed = rotationSpeed;

			if(turningOnSpot) targetRotationSpeed = turningOnSpotRotationSpeed;

			if(inIdle) {
				Vector3 targetDirection = new Vector3(movementDirection.x, 0f, movementDirection.z);
				deltaAngle = Vector3.Angle(targetDirection, transform.forward);
				float angleSign = Mathf.Sign(Vector3.Cross(transform.forward, targetDirection).y);
				deltaAngle *= angleSign;
			}

			turningOnSpot = Mathf.Abs(deltaAngle) > 30f && inIdle;

			if(movementDirection != Vector3.zero) {
				targetRotation = Quaternion.LookRotation(movementDirection);
				thisTransform.rotation = Quaternion.RotateTowards(thisTransform.rotation, targetRotation, Time.deltaTime * targetRotationSpeed);
			}
		}
	}
}
