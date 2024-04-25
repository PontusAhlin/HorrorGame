using UnityEngine;
using System.Collections;

namespace Footsteps {

	public class CameraView : MonoBehaviour {

		[SerializeField] float minTiltAngle = -70f;
		[SerializeField] float maxTiltAngle = 80f;
		[SerializeField] float sensitivity = 3f;
		[SerializeField] bool smooth = true;
		[SerializeField] float smoothFactor = 15f;
		[SerializeField] bool invert;

		[Header("References")]
		[SerializeField] Transform worldCamera;

		Transform characterTransform;

		Quaternion characterTargetRotation;
		Quaternion cameraTargetRotation;
		Vector2 lastMousePosition;
		float xAngle;
		float deltaYAngle;


		void Start() {
			characterTransform = transform;
			Cursor.lockState = CursorLockMode.Locked;

			if(!worldCamera) {
				Debug.LogError("Please assign 'world_camera' in the inspector, fps controller will not work.");
				enabled = false;

				return;
			}
		}

		void FixedUpdate() {
			// Modify the angle based on the user input
			xAngle += Input.GetAxis("Mouse Y") * sensitivity * (invert ? -1 : 1);
			xAngle = Mathf.Clamp(xAngle, minTiltAngle, maxTiltAngle);
			deltaYAngle = Input.GetAxis("Mouse X") * sensitivity;

			characterTransform.rotation *= Quaternion.Euler(Vector3.up * deltaYAngle);
			cameraTargetRotation = Quaternion.Euler (-xAngle, 0f, 0f);

			// Rotate the camera 
			worldCamera.localRotation = smooth ? Quaternion.Lerp(worldCamera.localRotation, cameraTargetRotation, Time.fixedDeltaTime * smoothFactor) : cameraTargetRotation;
		}
	}
}
