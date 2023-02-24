using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fov : MonoBehaviour
{
    private Mesh mesh;
    private Vector3 origin;
    private Vector3 playerposition; 

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        
        origin = Vector3.zero;
        playerposition = Vector3.zero;
    }

    // Start is called before the first frame update
    void Update()
    {
        float fov = 360f;
        int rayCount = 250;
        float currentAngle = 0f;
        float angleIncrease = fov / rayCount;
        float viewDistance = 3f;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;
        vertices[0] = playerposition;
        Debug.Log("Origin in update " + origin);

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            //Vector3 VectorFromAngle = new Vector3(Mathf.Cos(currentAngle * (Mathf.PI / 180f)), Mathf.Sin(currentAngle * (Mathf.PI / 180f)));
            Vector3 vertex = origin + GetVectorFromAngle(currentAngle) * viewDistance;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(
                origin,
                GetVectorFromAngle(currentAngle),
                viewDistance
            );
            if (raycastHit2D.collider == null)
            {
                vertex = origin + GetVectorFromAngle(currentAngle) * viewDistance;
            }
            else
            {
                vertex = raycastHit2D.point;
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

    public void SetOrigin(Vector3 origin)
    {
        Debug.Log("SetOrigin "+ origin);
        this.origin = origin;
    }

    public void SetCoordinates(Vector3 playerposition)
    {
        this.playerposition = playerposition;
    }
}
