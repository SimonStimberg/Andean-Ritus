using UnityEngine;
using System.Collections;
	
public class NoiseDeformer : MonoBehaviour
{
	// public float perlinScale = 4.56f;
	// public float waveSpeed = 1f;
	// public float waveHeight = 2f;
	
	// private Mesh mesh;

	private Perlin noise;
	private Vector3[] baseVertices;

	public float NoiseScale = 0.5f;
	public float speed = 0.8f;

	public float idleIntensity = 0.3f;
	public float stimIntensity = 3.0f;


	void Start()
	{
		noise = new Perlin ();

	}

	
	
	void Update()
	{
		stimIntensity = Mathf.Lerp(stimIntensity, idleIntensity, 0.05f);
		AnimateMesh(stimIntensity);
	}


	// void Stimulate(float intensity)
	// {
	// 	stimIntensity = intensity;
	// }
	



	void AnimateMesh(float intensity)
	{
		Mesh mesh = GetComponent< MeshFilter >().mesh;
	
		
	
		// float intensity = intensityGlobal;

		float scale = NoiseScale * intensity;

		if(baseVertices == null)
		{
			baseVertices = mesh.vertices;
		}




		Vector3[] vertices = new Vector3[baseVertices.Length];

		float timex = Time.time * speed + 0.1365143f;
    	float timey = Time.time * speed + 1.21688f;
    	float timez = Time.time * speed + 2.5564f;

		



		
		
		
		for (int i = 0; i < vertices.Length; i++)
		{
			Vector3 vertex = baseVertices[i];

			vertex.x += noise.Noise(timex + vertex.x, timex + vertex.y, timex + vertex.z) * scale;
        	vertex.y += noise.Noise(timey + vertex.x, timey + vertex.y, timey + vertex.z) * scale;
        	vertex.z += noise.Noise(timez + vertex.x, timez + vertex.y, timez + vertex.z) * scale;
        
        	vertices[i] = vertex;




			// float pX = ( vertices[i].x * perlinScale ) + ( Time.timeSinceLevelLoad * waveSpeed );
			// float pZ = ( vertices[i].z * perlinScale ) + ( Time.timeSinceLevelLoad * waveSpeed );
	
			// vertices[i].y = ( Mathf.PerlinNoise( pX, pZ ) - 0.5f ) * waveHeight;
		}
	
		mesh.vertices = vertices;

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();


	}
}



