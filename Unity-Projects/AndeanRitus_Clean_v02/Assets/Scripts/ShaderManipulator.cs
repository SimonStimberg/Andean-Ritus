using UnityEngine;

public class ShaderManipulator : MonoBehaviour
{
    Renderer rend;
	public float AmountOfWobbleness = 50.1f;
    [Range(0,1)]
    public float Strength = 0.1f;
    [Range(0,5)]
    public float Wavelength = 1.0f;
    private float lerpStrength = 0.0f;

    void Start()
    {
        rend = GetComponent<Renderer> ();

        // Use the Specular shader on the material
        // rend.material.shader = Shader.Find("Distortion");
    }

    void Update()
    {
        // Animate the Shininess value
        lerpStrength = Mathf.Lerp(lerpStrength, 0.0f, 0.05f);
        
        rend.material.SetFloat("_Manipulator", AmountOfWobbleness);
        rend.material.SetFloat("_Strength", lerpStrength);
        rend.material.SetFloat("_Wavelength", Wavelength);
    }

    public void Distort()
    {
        lerpStrength = Strength;
    }
}