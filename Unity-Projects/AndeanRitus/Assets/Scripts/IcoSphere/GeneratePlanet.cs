using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePlanet : MonoBehaviour
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
        CreatePlanet();

		
	}


    public void CreatePlanet()
    {
        CreatePlanetGameObject();
        //do whatever else you need to do with the sphere mesh
        RecalculateMesh();
    }

    void CreatePlanetGameObject()
    {
        planet = this.gameObject;
        



        planetMeshFilter = planet.GetComponent<MeshFilter>();
        planetMesh = planetMeshFilter.mesh;
        planetMeshRenderer = planet.GetComponent<MeshRenderer>();
        //need to set the material up top
        planetMeshRenderer.material = planetMaterial;
        planet.transform.localScale = new Vector3(planetSize, planetSize, planetSize);
        IcoSphere.Create(planet, levelOfRefinement);
    }

    void RecalculateMesh()
    {
        planetMesh.RecalculateBounds();
        planetMesh.RecalculateTangents();
        planetMesh.RecalculateNormals();
    }
}