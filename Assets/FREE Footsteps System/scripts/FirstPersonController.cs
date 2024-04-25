using UnityEngine;

namespace Footsteps {

	[RequireComponent(typeof(CharacterController))]
	public class FirstPersonController : MonoBehaviour {

		public Vector3 velocity { get; private set; }
		public bool isJumping { get; private set; }
		public bool isGrounded { get; private set; }
		public bool previouslyGrounded { get; private set; }

		[Header("Movement Settings")]
		[SerializeField] float forwardSpeed = 5f;
		[SerializeField] float backwardSpeed = 4f;
		[SerializeField] float strafeSpeed = 5f;
		[SerializeField] float runMultiplier = 1.8f;
		[SerializeField] float acceleration = 18f;
		[SerializeField] float deceleration = 12f;
		[SerializeField] float movementEnergy = 6f;

		[Header("Jump Settings")]
		[SerializeField] float jumpBaseSpeed = 5f;
		[SerializeField] float jumpExtraSpeed = 1f;
		[SerializeField] float gravity = -20f;
		[SerializeField] [Range(0f, 1f)] float airControl = 0.2f;

		[Header("References")]
		[SerializeField] Transform worldCamera;

		// References
		Transform thisTransform;
		CharacterController thisCharacterController;

		// System
		Vector3 targetDirection;
		Vector2 movementInput;
		float targetSpeed;
		float currentSpeed;
		float remainedExtraJumpSpeed;

		// States
		bool jump;


		void Start() {
			thisTransform = transform;
			thisCharacterController = GetComponent<CharacterController>();

			// Searching for potential errors
			string errorMessage = "none";

			if(!thisCharacterController) errorMessage = "The script 'CharacterMotor' needs a CharacterController to work, none was found, this script will not work.";
			else if(!worldCamera) errorMessage = "Please assign 'world_camera' in the inspector, fps controller will not work.";

			if(errorMessage != "none") {
				enabled = false;
				Debug.LogError(errorMessage);

				return;
			}
		}

		void Update() {
			HandleUserInput();
		}

		void FixedUpdate() {
			previouslyGrounded = isGrounded;
			isGrounded = thisCharacterController.isGrounded;
			velocity = thisCharacterController.velocity;

			float accelRate = movementInput.sqrMagnitude > 0f ? acceleration : deceleration;
			float controlModifier = (isGrounded ? 1f : airControl);

			currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, (Time.fixedDeltaTime * accelRate * controlModifier));
			Vector3 targetVelocity = targetDirection.normalized * currentSpeed;
			targetVelocity.y = thisCharacterController.velocity.y + gravity * Time.fixedDeltaTime;

			if(jump && isGrounded) {
				// Jumping
				targetVelocity = new Vector3(targetVelocity.x, jumpBaseSpeed, targetVelocity.z);
				isJumping = true;
			}
			else if(isGrounded && !previouslyGrounded) {
				if(isJumping) isJumping = false;

				remainedExtraJumpSpeed = jumpExtraSpeed;
			} 

			if(jump && thisCharacterController.velocity.y > 0f) {
				float jumpSpeedIncrement = remainedExtraJumpSpeed * Time.fixedDeltaTime;
				remainedExtraJumpSpeed -= jumpSpeedIncrement;

				if(jumpSpeedIncrement > 0f) {
					targetVelocity.y += jumpSpeedIncrement;
				}
			}

			Vector3 vel = Vector3.MoveTowards(thisCharacterController.velocity, targetVelocity, Time.fixedDeltaTime * movementEnergy);
			vel.y = targetVelocity.y;
			thisCharacterController.Move(vel * Time.fixedDeltaTime);

			jump = false;
		}

		void HandleUserInput() {
			float h = Input.GetAxisRaw("Horizontal");
			float v = Input.GetAxisRaw("Vertical");

			movementInput = new Vector2(h, v);

			jump = Input.GetButton("Jump");
			targetSpeed = 0f;

			if(movementInput.x > 0f || movementInput.x < 0f) {
				targetSpeed = strafeSpeed;
			}

			if(movementInput.y < 0f) {
				targetSpeed = backwardSpeed;
			}

			if(movementInput.y > 0f) {
				targetSpeed = forwardSpeed;
			}

			if(Input.GetKey(KeyCode.LeftShift)) {
				targetSpeed *= runMultiplier;
			}

			if(Mathf.Abs(h) != 0f || Mathf.Abs(v) != 0f) {
				targetDirection = thisTransform.forward * movementInput.y + thisTransform.right * movementInput.x;
			}
		}
	}
}