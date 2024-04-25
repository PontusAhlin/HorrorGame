using UnityEngine;
using UnityEngine.UI;

namespace Footsteps {

	public class DemoUI : MonoBehaviour {

		[SerializeField] GameObject topDownController;
		[SerializeField] GameObject topDownCamera;
		[SerializeField] GameObject firstPersonController;


		public void ActivateTopDown() {
			if(!topDownController.activeSelf) topDownController.transform.position = firstPersonController.transform.position;

			firstPersonController.SetActive(false);
			topDownController.SetActive(true);
			topDownCamera.SetActive(true);
		}

		public void ActivateFirstPerson() {
			if(!firstPersonController.activeSelf) firstPersonController.transform.position = topDownController.transform.position;

			firstPersonController.SetActive(true);
			topDownController.SetActive(false);
			topDownCamera.SetActive(false);
		}
	}
}
