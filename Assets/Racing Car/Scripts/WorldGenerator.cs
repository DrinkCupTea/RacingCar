using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] private Material meshMaterial;
    [SerializeField] private Vector2 dimensions;
    [SerializeField] private float perlinScale;
    [SerializeField] private float offset;
    [SerializeField] private float waveHeithg;
    [SerializeField] private float scale;
    [SerializeField] private float worldMoveSpeed;
    [SerializeField] private int startTransitionLength;

    private Vector3[] beginPoints;
    private readonly float randomness = 0.1f;
    private readonly GameObject[] pieces = new GameObject[2];
    // Start is called before the first frame update
    void Start()
    {
        beginPoints = new Vector3[(int)dimensions.x + 1];
        for (int i = 0; i < 2; i++)
        {
            GenerateWorldPiece(i);
        }
    }

    void LateUpdate()
    {
        if (pieces[1] && pieces[1].transform.position.z <= -(dimensions.y * scale * Mathf.PI / 4f))
        {
            StartCoroutine(nameof(UpdateWorldPieces));
        }
    }

    IEnumerable UpdateWorldPieces()
    {
        Destroy(pieces[0]);
        pieces[0] = pieces[1];
        pieces[1] = CreateCylinder();
        pieces[1].transform.position = pieces[0].transform.position + Vector3.forward * (dimensions.y * scale * Mathf.PI);
        pieces[1].transform.rotation = pieces[0].transform.rotation;

        UpdateSinglePiece(pieces[1]);

        return null;
    }


    void GenerateWorldPiece(int i)
    {
        pieces[i] = CreateCylinder();
        pieces[i].transform.Translate(Vector3.forward * (dimensions.y * scale * Mathf.PI) * i);
        UpdateSinglePiece(pieces[i]);
    }

    void UpdateSinglePiece(GameObject piece)
    {
        BasicMovement basicMovement = piece.AddComponent<BasicMovement>();
        basicMovement.setSpeed(worldMoveSpeed);

        GameObject endPoint = new();
        endPoint.transform.position = piece.transform.position + Vector3.forward * (dimensions.y * scale * Mathf.PI);
        endPoint.transform.parent = piece.transform;
        endPoint.name = "End Point";
        offset += randomness;
    }

    GameObject CreateCylinder()
    {
        GameObject newCylinder = new()
        {
            name = "World Piece"
        };


        MeshFilter meshFilter = newCylinder.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = newCylinder.AddComponent<MeshRenderer>();

        meshRenderer.material = meshMaterial;
        meshFilter.mesh = GenerateMesh();

        newCylinder.AddComponent<MeshCollider>();
        return newCylinder;
    }

    Mesh GenerateMesh()
    {
        Mesh mesh = new()
        {
            name = "Mesh"
        };

        Vector3[] vertices = null;
        Vector2[] uvs = null;
        int[] triangles = null;

        CreateShape(ref vertices, ref uvs, ref triangles);

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    void CreateShape(ref Vector3[] vertices, ref Vector2[] uvs, ref int[] triangles)
    {
        int xCount = (int)dimensions.x;
        int zCount = (int)dimensions.y;

        vertices = new Vector3[(xCount + 1) * (zCount + 1)];
        uvs = new Vector2[(xCount + 1) * (zCount + 1)];

        float radius = xCount * 0.5f * scale;
        int index = 0;
        for (int x = 0; x <= xCount; x++)
        {
            for (int z = 0; z <= zCount; z++)
            {
                float angle = x * Mathf.PI * 2f / xCount;
                vertices[index] = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, z * scale * Mathf.PI);

                uvs[index] = new Vector2(x * scale, z * scale);

                float pX = (vertices[index].x * perlinScale) + offset;
                float pZ = (vertices[index].z * perlinScale) + offset;

                Vector3 center = new Vector3(0, 0, vertices[index].z);
                vertices[index] += (center - vertices[index]).normalized * Mathf.PerlinNoise(pX, pZ) * waveHeithg;

                if (z < startTransitionLength && beginPoints[0] != Vector3.zero)
                {
                    float perlinPercentage = z / (float)startTransitionLength;
                    Vector3 beginPoint = new(beginPoints[x].x, beginPoints[x].y, vertices[index].z);
                    vertices[index] = (perlinPercentage * vertices[index]) + ((1 - perlinPercentage) * beginPoint);
                }
                else if (z == zCount)
                {
                    beginPoints[x] = vertices[index];
                }

                index++;
            }
        }

        triangles = new int[xCount * zCount * 6];
        int[] boxBase = null;

        index = 0;
        for (int x = 0; x < xCount; x++)
        {
            boxBase = new int[] {
                x * (zCount + 1),
                x * (zCount + 1) + 1,
                (x + 1) * (zCount + 1),
                x * (zCount + 1) + 1,
                (x + 1) * (zCount + 1) + 1,
                (x + 1) * (zCount + 1),
            };

            for (int z = 0; z < zCount; z++)
            {
                for (int i = 0; i < 6; i++)
                {
                    boxBase[i] = boxBase[i] + 1;
                }

                for (int j = 0; j < 6; j++)
                {
                    triangles[index + j] = boxBase[j] - 1;
                }

                index += 6;
            }
        }
    }

    public GameObject GetWorldPiece()
    {
        return pieces[0];
    }
}
