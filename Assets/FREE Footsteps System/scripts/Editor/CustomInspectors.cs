using UnityEngine;
using UnityEditor;

namespace Footsteps {

	// Custom inspector of type 'CharacterFootsteps'
	[CustomEditor(typeof(CharacterFootsteps))]
	public class FootstepsInspector : Editor {

		[MenuItem("FootstepsCreator/CreateFootstepsDatabase")]
		static void CreateSurfaceManager() {
			if(!FindObjectOfType<SurfaceManager>()) {
				GameObject database = Instantiate<GameObject>(Resources.Load<GameObject>("footsteps_database"));
				database.name = "footsteps_database";
			}
			else {
				Debug.Log("A footsteps_database already exists in the current scene.");
			}
		}

		public override void OnInspectorGUI () {
			serializedObject.Update();

			Rect rect = new Rect();
			SerializedProperty triggeredBy = serializedObject.FindProperty("triggeredBy");

			GUILayout.Box("Hover over every setting to see what it means, for more info on them, read the documentation.");

			EditorGUILayout.LabelField("Trigger Settings", new GUIStyle() { fontStyle = FontStyle.BoldAndItalic });
			EditorGUILayout.PropertyField(triggeredBy);

			// If the dropwdown shows COLLISION_DETECTION
			if(triggeredBy.enumValueIndex == 0) {
				GUILayout.Box("You need to have a footstep trigger properly sized for each foot, for this method to work, please look into documentation for more info.");
			}
			// If the dropdown shows DISTANCE_TRAVELED
			else {
				SerializedProperty distanceBetweenSteps = serializedObject.FindProperty("distanceBetweenSteps");
				SerializedProperty controllerType = serializedObject.FindProperty("controllerType");
				SerializedProperty characterRigidbody = serializedObject.FindProperty("characterRigidbody");
				SerializedProperty characterController = serializedObject.FindProperty("characterController");

				rect = EditorGUILayout.GetControlRect();
				EditorGUI.indentLevel ++;

				// Distance between steps
				EditorGUI.PropertyField(rect, distanceBetweenSteps);

				// Controller type
				rect.y = rect.yMax + 2;
				EditorGUI.PropertyField(rect, controllerType);

				rect.y = rect.yMax + 2;
				EditorGUI.indentLevel ++;
				// If the dropdown shows RIGIDBODY
				if(controllerType.enumValueIndex == 0) {
					EditorGUI.PropertyField(rect, characterRigidbody);
				}
				// If the dropdown shows CHARACTER_CONTROLLER
				else {
					EditorGUI.PropertyField(rect, characterController);
				}

				for(int i = 0;i < 6;i ++) EditorGUILayout.Space();
				EditorGUI.indentLevel -= 2;
			}

			SerializedProperty audioSource = serializedObject.FindProperty("audioSource");
			SerializedProperty minVolume = serializedObject.FindProperty("minVolume");
			SerializedProperty maxVolume = serializedObject.FindProperty("maxVolume");
			SerializedProperty debugMode = serializedObject.FindProperty("debugMode");
			SerializedProperty groundCheckHeight = serializedObject.FindProperty("groundCheckHeight");
			SerializedProperty groundCheckRadius = serializedObject.FindProperty("groundCheckRadius");
			SerializedProperty groundCheckDistance = serializedObject.FindProperty("groundCheckDistance");
			SerializedProperty groundLayers = serializedObject.FindProperty("groundLayers");

			EditorGUILayout.LabelField("General Settings", new GUIStyle() { fontStyle = FontStyle.BoldAndItalic });
			rect = EditorGUILayout.GetControlRect();

			// Audio Source
			EditorGUI.PropertyField(rect, audioSource);

			// Min Volume
			rect.y = rect.yMax + 2;
			EditorGUI.PropertyField(rect, minVolume);

			// Max Volume
			rect.y = rect.yMax + 2;
			EditorGUI.PropertyField(rect, maxVolume);

			for(int i = 0;i < 6;i ++) EditorGUILayout.Space();

			EditorGUILayout.LabelField("Ground Check Settings", new GUIStyle() { fontStyle = FontStyle.BoldAndItalic });
			rect = EditorGUILayout.GetControlRect();

			// Debug Mode
			EditorGUI.PropertyField(rect, debugMode);

			// Ground Check Height
			rect.y = rect.yMax + 2;
			EditorGUI.PropertyField(rect, groundCheckHeight);

			// Ground Check Radius
			rect.y = rect.yMax + 2;
			EditorGUI.PropertyField(rect, groundCheckRadius);

			// Ground Check Distance
			rect.y = rect.yMax + 2;
			EditorGUI.PropertyField(rect, groundCheckDistance);

			// Ground Layers
			rect.y = rect.yMax + 2;
			EditorGUI.PropertyField(rect, groundLayers);

			for(int i = 0;i < 12;i ++) EditorGUILayout.Space();

			serializedObject.ApplyModifiedProperties();
		}
	}

	// Custom property inspector of type 'RegisteredMaterial'
	[CustomPropertyDrawer(typeof(RegisteredMaterial))]
	public class MaterialDrawer : PropertyDrawer {

		SurfaceManager surfManag = null;


		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
			if(!surfManag) {
				surfManag = GameObject.FindObjectOfType<SurfaceManager>();

				return;
			}

			SerializedProperty texture = property.FindPropertyRelative("texture");
			SerializedProperty surfaceIndex = property.FindPropertyRelative("surfaceIndex");

			// Showing labels for the fields
			GUI.Label(position, "Texture");
			position.x = position.width / 2f;
			GUI.Label(position, "GroundType");

			// Set the new rect 
			position.height = 16f;
			position.y = position.yMax;
			position.x = 0f;
			position.width /= 2.2f;

			// Draw the texture field
			EditorGUI.PropertyField(position, texture, GUIContent.none);

			// Draw the type field
			position.x = position.xMax;
			surfaceIndex.intValue = EditorGUI.Popup(position, surfaceIndex.intValue, surfManag.GetAllSurfaceNames());
		}

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
			return 32f;
		}
	}
}
