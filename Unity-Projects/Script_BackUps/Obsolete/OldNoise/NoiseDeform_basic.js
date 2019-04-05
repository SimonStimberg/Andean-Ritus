#pragma strict

var NoiseScale = 0.5;
var speed = 0.8;
var recalculateNormals = true;

private var baseVertices : Vector3[];
private var noise : Perlin;
public var meNum : int = 0;
public var intensityGlobal : float = 0.3;
private var manager : OSCManager;

function Start ()
{
    noise = new Perlin ();
    // manager = GameObject.Find("OSC Receiver").GetComponent("OSCManager");
    
}

function Update () 
{
    Deform();
    
    

    // var csScript: YourCsScript = GetComponent(YourCsScript);
    // var stepNow = manager.step;
    // var pitchNow = GameObject.Find("OSC Receiver").GetComponent(OSCManager).pitch;

    // if(stepNow == meNum)
    // {
    //     intensityGlobal = 4.0;
    // }
    // else
    // {
    //     intensityGlobal = lerpy(intensityGlobal, 0.3, 0.1);
    // }
    // Deform(intensityGlobal);
}

// function lerpy(start: float, end : float, amt : float)
// {
//     return (1-amt) * start + amt * end;
// }

function Deform ()
{
    var mesh : Mesh = GetComponent(MeshFilter).mesh;
        
    var intensity = intensityGlobal;
    //var intensity = GetComponent(OSCReceiver2).NoiseIntensity;

    var scale = NoiseScale * intensity;

    if (baseVertices == null)
        baseVertices = mesh.vertices;
        
    var vertices = new Vector3[baseVertices.Length];
    
    var timex = Time.time * speed + 0.1365143;
    var timey = Time.time * speed + 1.21688;
    var timez = Time.time * speed + 2.5564;
    for (var i=0;i<vertices.Length;i++)
    {
        var vertex = baseVertices[i];
                
        vertex.x += noise.Noise(timex + vertex.x, timex + vertex.y, timex + vertex.z) * scale;
        vertex.y += noise.Noise(timey + vertex.x, timey + vertex.y, timey + vertex.z) * scale;
        vertex.z += noise.Noise(timez + vertex.x, timez + vertex.y, timez + vertex.z) * scale;
        
        vertices[i] = vertex;
    }
    
    mesh.vertices = vertices;
    
    if (recalculateNormals)	
        mesh.RecalculateNormals();
    mesh.RecalculateBounds();

}