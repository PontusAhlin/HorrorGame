using UnityEngine;
using System.Collections;

namespace Footsteps {

	public class CameraFollow : MonoBehaviour {

		[SerializeField] Transform target;
		[SerializeField] float followLerpFactor = 5f;

		Transform thisTransform;


		void Start() {
			if(!target) enabled = false;

			thisTransform = transform;
		}

		void FixedUpdate() {
			thisTransform.position = Vector3.Lerp(thisTransform.position, target.position, Time.deltaTime * followLerpFactor);
		}
	}
}
