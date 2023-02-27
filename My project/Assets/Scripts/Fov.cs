using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fov : MonoBehaviour
{
    private Mesh mesh;
    private Vector3 origin;
    public LayerMask layer;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Start is called before the first frame update
    void Update()
    {
        transform.position = Vector3.zero;
        origin = transform.parent.position;
        float fov = 360f;
        int rayCount = 360;
        float currentAngle = 0f;
        float angleIncrease = fov / rayCount;
        float viewDistance = 15f;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
for (int i = 0; i <= rayCount; i++)
{
    Vector3 vertex = origin + GetVectorFromAngle(currentAngle) * viewDistance;
    RaycastHit raycastHit;
    bool hasHit =   Physics.Raycast(
        origin,
        GetVectorFromAngle(currentAngle),
        out raycastHit,
        viewDistance,
        layer
    );

    //loop over alle hits
    //indien hit
    //check of de tag Wall
    //zoja 
    //pak kleinste afstand
    //vertex = raycastHit.point;
    
  
    if (!hasHit)
    {
        vertex = origin + GetVectorFromAngle(currentAngle) * viewDistance;
    }
    else
    {
        vertex = raycastHit.point;
    }
    vertices[vertexIndex] = vertex;

    if (i > 0)
    {
        triangles[triangleIndex + 0] = 0;
        triangles[triangleIndex + 1] = vertexIndex - 1;
        triangles[triangleIndex + 2] = vertexIndex;

        triangleIndex += 3;
    }

    vertexIndex++;
    currentAngle -= angleIncrease;
}

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(origin, Vector3.one * 1000f);
    }

    public static Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
}
