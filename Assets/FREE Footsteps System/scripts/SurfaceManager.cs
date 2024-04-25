using UnityEngine;

[System.Serializable]
public struct SurfaceDefinition {
	public string name;
	public AudioClip[] footsteps;
}

[System.Serializable]
public struct RegisteredMaterial {
	public Texture texture;
	public int surfaceIndex;
}

public class SurfaceManager : MonoBehaviour {

	public static SurfaceManager singleton;

	[SerializeField] SurfaceDefinition[] definedSurfaces;
	[SerializeField] RegisteredMaterial[] registeredTextures;

	int n;


	void Start() {
		if(!singleton) singleton = this;
		else if(singleton != this) Destroy(gameObject);
	}

	public AudioClip GetFootstep(Collider groundCollider, Vector3 worldPosition) {
		int surfaceIndex = GetSurfaceIndex(groundCollider, worldPosition);

		if(surfaceIndex == -1) {
			return null;
		}

		// Getting the footstep sounds based on surface index.
		AudioClip[] footsteps = definedSurfaces[surfaceIndex].footsteps;
		n = Random.Range(1, footsteps.Length);

		// Move picked sound to index 0 so it's not picked next time.
		AudioClip temp = footsteps[n];
		footsteps[n] = footsteps[0];
		footsteps[0] = temp;

		return temp;
	}

	public string[] GetAllSurfaceNames() {
		string[] names = new string[definedSurfaces.Length];

		for(int i = 0;i < names.Length;i ++) names[i] = definedSurfaces[i].name;

		return names;
	}

	// This is for bullet hit particles
	int GetSurfaceIndex(Ray ray, Collider col, Vector3 worldPos) {
		string textureName = "";

		// Case when the ground is a terrain.
		if(col.GetType() == typeof(TerrainCollider)) {
			Terrain terrain = col.GetComponent<Terrain>();
			TerrainData terrainData = terrain.terrainData;
			float[] textureMix = GetTerrainTextureMix(worldPos, terrainData, terrain.GetPosition());
			int textureIndex = GetTextureIndex(worldPos, textureMix);
			textureName = terrainData.splatPrototypes[textureIndex].texture.name;
		}
		// Case when the ground is a normal mesh.
		else {
			textureName = GetMeshMaterialAtPoint(worldPos, ray);
		}
		// Searching for the found texture / material name in registered materials.
		foreach(var material in registeredTextures) {
			if(material.texture.name == textureName) {
				return material.surfaceIndex;
			}
		}

		return -1;
	}

	// This is for footsteps
	int GetSurfaceIndex(Collider col, Vector3 worldPos) {
		string textureName = "";

		// Case when the ground is a terrain.
		if(col.GetType() == typeof(TerrainCollider)) {
			Terrain terrain = col.GetComponent<Terrain>();
			TerrainData terrainData = terrain.terrainData;
			float[] textureMix = GetTerrainTextureMix(worldPos, terrainData, terrain.GetPosition());
			int textureIndex = GetTextureIndex(worldPos, textureMix);
			textureName = terrainData.splatPrototypes[textureIndex].texture.name;
		}
		// Case when the ground is a normal mesh.
		else {
			textureName = GetMeshMaterialAtPoint(worldPos, new Ray(Vector3.zero, Vector3.zero));
		}
		// Searching for the found texture / material name in registered materials.
		foreach(var material in registeredTextures) {
			if(material.texture.name == textureName) {
				return material.surfaceIndex;
			}
		}

		return -1;
	}

	string GetMeshMaterialAtPoint(Vector3 worldPosition, Ray ray) {
		if(ray.direction == Vector3.zero) {
			ray = new Ray(worldPosition + Vector3.up * 0.01f, Vector3.down);
		}

		RaycastHit hit;

		if (!Physics.Raycast (ray, out hit)) {
			return "";
		}

		Renderer r = hit.collider.GetComponent<Renderer>();
		MeshCollider mc = hit.collider as MeshCollider;

		if (r == null || r.sharedMaterial == null || r.sharedMaterial.mainTexture == null || r == null) {
			return "";
		}
		else if(!mc || mc.convex) {
			return r.material.mainTexture.name;
		}

		int materialIndex = -1;
		Mesh m = mc.sharedMesh;
		int triangleIdx = hit.triangleIndex;
		int lookupIdx1 = m.triangles[triangleIdx * 3];
		int lookupIdx2 = m.triangles[triangleIdx * 3 + 1];
		int lookupIdx3 = m.triangles[triangleIdx * 3 + 2];
		int subMeshesNr = m.subMeshCount;

		for(int i = 0;i < subMeshesNr;i ++) {
			int[] tr = m.GetTriangles(i);

			for(int j = 0;j < tr.Length;j += 3) {
				if (tr[j] == lookupIdx1 && tr[j+1] == lookupIdx2 && tr[j+2] == lookupIdx3) {
					materialIndex = i;

					break;
				}
			}

			if (materialIndex != -1) break;
		}

		string textureName = r.materials[materialIndex].mainTexture.name;

		return textureName;
	}

	float[] GetTerrainTextureMix(Vector3 worldPos, TerrainData terrainData, Vector3 terrainPos) {
		// returns an array containing the relative mix of textures
		// on the main terrain at this world position.

		// The number of values in the array will equal the number
		// of textures added to the terrain.

		// calculate which splat map cell the worldPos falls within (ignoring y)
		int mapX = (int)(((worldPos.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth);
		int mapZ = (int)(((worldPos.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight);

		// get the splat data for this cell as a 1x1xN 3d array (where N = number of textures)
		float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

		// extract the 3D array data to a 1D array:
		float[] cellMix = new float[splatmapData.GetUpperBound(2) + 1];

		for(int n = 0;n < cellMix.Length;n++) {
			cellMix[n] = splatmapData[0, 0, n];
		}

		return cellMix;
	}

	int GetTextureIndex(Vector3 worldPos, float[] textureMix) {
		// returns the zero-based index of the most dominant texture
		// on the terrain at this world position.
		float maxMix = 0;
		int maxIndex = 0;

		// loop through each mix value and find the maximum
		for(int n = 0;n < textureMix.Length;n ++){
			if (textureMix[n] > maxMix){
				maxIndex = n;
				maxMix = textureMix[n];
			}
		}

		return maxIndex;
	}
}


