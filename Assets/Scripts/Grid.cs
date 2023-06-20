using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour
{

    public int xSize, ySize;

    private Vector3[] vertices;

    private Mesh mesh;

    private void Awake()
    {
        StartCoroutine(Generate());
    }

    private IEnumerator Generate()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f);

        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";

        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
                yield return wait;
            }
        }
        mesh.vertices = vertices;
        /*
        int[] triangles = new int[6];
        triangles[0] = 0;
        triangles[1] = xSize + 1;
        triangles[2] = 1;
        triangles[3] = 1;
        triangles[4] = xSize + 1;
        triangles[5] = xSize + 2;*/

        int[] triangles = new int[xSize * ySize * 6];
        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                int ti = x * 6 + (y * xSize * 6);
                int vi = x + (y * (xSize + 1));
                triangles[ti] = vi;
                triangles[ti + 1] = triangles[ti + 4] = xSize + vi + 1;
                triangles[ti + 2] = triangles[ti + 3] = vi + 1;
                triangles[ti + 5] = xSize + vi + 2;
                mesh.triangles = triangles;
                yield return wait;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }

        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}
