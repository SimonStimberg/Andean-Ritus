using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateNewPlanet : MonoBehaviour
{
    public Material planetMaterial;
    public float planetSize = 1f;
    public int levelOfRefinement = 3;
    GameObject planet;
    Mesh planetMesh;
    Vector3[] planetVertices;
    int[] planetTriangles;
    MeshRenderer planetMeshRenderer;
    MeshFilter planetMeshFilter;
    MeshCollider planetMeshCollider;


    void Start () {
		
	}


    public void CreatePlanet(int objCounter, float x, float y, float z)
    {
        CreatePlanetGameObject(objCounter);
        //do whatever else you need to do with the sphere mesh
        RecalculateMesh(x, y, z);
    }

    void CreatePlanetGameObject(int objCounter)
    {
        planet = new GameObject();

        planet.name = "MysticalSphere"+objCounter;

        planetMeshFilter = planet.AddComponent<MeshFilter>();
        planetMesh = planetMeshFilter.mesh;
        planetMeshRenderer = planet.AddComponent<MeshRenderer>();

        //need to set the material up top
        planetMeshRenderer.material = planetMaterial;
        planet.AddComponent<NoiseDeformer>();
        planet.transform.localScale = new Vector3(planetSize, planetSize, planetSize);
        IcoSphere.Create(planet, levelOfRefinement);
    }

    void RecalculateMesh(float x, float y, float z)
    {
        planetMesh.RecalculateBounds();
        planetMesh.RecalculateTangents();
        planetMesh.RecalculateNormals();

        planet.transform.position = new Vector3(x, y, z);        
    }
}