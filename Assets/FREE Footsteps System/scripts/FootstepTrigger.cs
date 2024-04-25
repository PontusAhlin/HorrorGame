using UnityEngine;
using System.Collections;

namespace Footsteps {

	[RequireComponent(typeof(Collider), typeof(Rigidbody))]
	public class FootstepTrigger : MonoBehaviour {

		Collider thisCollider;
		CharacterFootsteps footsteps;


		void Start() {
			thisCollider = GetComponent<Collider>();
			footsteps = GetComponentInParent<CharacterFootsteps>();
			Rigidbody thisRigidbody = GetComponent<Rigidbody>();

			if(thisCollider) {
				thisCollider.isTrigger = true;
				SetCollisions();
			}

			if(thisRigidbody) thisRigidbody.isKinematic = true;

			string errorMessage = "";

			if(!footsteps) errorMessage = "No 'CharacterFootsteps' script found as a parent, this footstep trigger will not work";
			else if(!thisCollider) errorMessage = "Please attach a collider marked as a trigger to this gameobject, this footstep trigger will not work";
			else if(!thisRigidbody) errorMessage = "Please attach a rigidbody to this gameobject, this footstep trigger will not work";

			if(errorMessage != "") {
				Debug.LogError(errorMessage);
				enabled = false;

				return;
			}
		}

		void OnEnable() {
			SetCollisions();
		}

		void OnTriggerEnter(Collider other) {
			if(footsteps) {
				footsteps.TryPlayFootstep();
			}
		}

		void SetCollisions() {
			if(!footsteps) return;

			Collider[] allColliders = footsteps.GetComponentsInChildren<Collider>();

			foreach(var collider in allColliders) {
				if(collider != GetComponent<Collider>()) {
					Physics.IgnoreCollision(thisCollider, collider);
				}
			}
		}
	}
}
