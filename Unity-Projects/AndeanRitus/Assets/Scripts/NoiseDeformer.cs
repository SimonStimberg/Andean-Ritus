using UnityEngine;
using System.Collections;
	
public class NoiseDeformer : MonoBehaviour
{

	private Perlin noise;
	private Vector3[] baseVertices;

	public float NoiseScale = 0.5f;
	public float speed = 0.5f;

	public float idleIntensity = 0.3f;
	public float stimIntensity = 3.0f;
	public float viscosity = 0.05f;

	public float size = 1.0f;
	private bool unsized = true;


	void Start()
	{
		noise = new Perlin ();
	}

	
	
	void Update()
	{
		// always lerp the against the idle intensity
		// -> if the object is stiumulated it automatically lerps back to idle
		// viscosity sets the interpolation amount and therefore determines the time it takes to reach idle state
		stimIntensity = Mathf.Lerp(stimIntensity, idleIntensity, viscosity);
		AnimateMesh(stimIntensity);
		if(unsized && size != 1.0f)
		{
			Resize();
		}
	}


	// at first stimulation: resize the sphere to the according pitch
	void Resize()
	{
		Vector3 actualSize = this.transform.localScale;
		this.transform.localScale = new Vector3(actualSize.x * size, actualSize.y * size, actualSize.z * size);
		unsized = false;
	}


	// do the actual deformation
	void AnimateMesh(float intensity)
	{
		Mesh mesh = GetComponent< MeshFilter >().mesh;
	
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
		}
	
		mesh.vertices = vertices;

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

	}
}



