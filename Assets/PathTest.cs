using UnityEngine;

public class PathTest : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        Debug.Log($"{vertices}");

        var i = 0;
        foreach (var v in vertices) {
            GameObject lightGameObject = new GameObject($"Light {i}");
            Light lightComp = lightGameObject.AddComponent<Light>();
            lightComp.color = Color.white;
            lightGameObject.transform.position = v;
            lightGameObject.transform.parent = gameObject.transform;
            //Instantiate(lightGameObject);
            Debug.Log($"{v} {i}");
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
