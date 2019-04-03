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

    private int objCounter = 1;

    void Start () {
  
		
	}


    public void CreatePlanet(float x, float y, float z)
    {
        CreatePlanetGameObject();
        //do whatever else you need to do with the sphere mesh
        RecalculateMesh(x, y, z);
    }

    void CreatePlanetGameObject()
    {
        planet = new GameObject();

        planet.name = "MysticalSphere"+objCounter;

        objCounter++;
        



        planetMeshFilter = planet.AddComponent<MeshFilter>();
        planetMesh = planetMeshFilter.mesh;
        planetMeshRenderer = planet.AddComponent<MeshRenderer>();
        //need to set the material up top
        planetMeshRenderer.material = planetMaterial;
        planet.transform.localScale = new Vector3(planetSize, planetSize, planetSize);
        IcoSphere.Create(planet, levelOfRefinement);
    }

    void RecalculateMesh(float x, float y, float z)
    {
        planetMesh.RecalculateBounds();
        planetMesh.RecalculateTangents();
        planetMesh.RecalculateNormals();

        planet.transform.position = new Vector3(x, y, z);
        Debug.Log("Position: " + x + " " + y + " " + z);
    }
}