using UnityEngine;

public class ShaderManipulator : MonoBehaviour
{
    Renderer rend;
	public float AmountOfWobbleness = 50.1f;

    void Start()
    {
        rend = GetComponent<Renderer> ();

        // Use the Specular shader on the material
        // rend.material.shader = Shader.Find("Distortion");
    }

    void Update()
    {
        // Animate the Shininess value
        
        rend.material.SetFloat("_Manipulator", AmountOfWobbleness);
    }
}