using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateWater : MonoBehaviour
{
    //number of point in 1 unit
    [SerializeField] private GameObject pointPrefab;

    [Header("parameter")]
    [SerializeField] private float quality;
    [SerializeField] private float length;
    [SerializeField] private float height;

    private List<GameObject> waterPoints;
    private Mesh mesh;
    private Vector3[] verticles;
    private int[] triangles;
    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }
    private void Start()
    {
        waterPoints = new List<GameObject>();
        Generate();
    }
    private void Update()
    {
        UpdateMesh();
    }
    private void Generate()
    {
        int numberPoints = (int)Mathf.Ceil(quality * length);
        float pointGap = 1 / quality;
        for(int i = 0; i < numberPoints; ++i)
        {
            GameObject tempWaterPoint = Instantiate(pointPrefab, transform.position + Vector3.left * length * 0.5f + Vector3.right * pointGap * i, Quaternion.identity, transform).transform.GetChild(0).gameObject;
            if (i == 0)
            {
                Destroy(tempWaterPoint.GetComponent<SpringJoint2D>());
            }
            else
            {
                tempWaterPoint.GetComponent<SpringJoint2D>().connectedBody = waterPoints[i - 1].GetComponent<Rigidbody2D>();
            }
            waterPoints.Add(tempWaterPoint);
        }
        verticles = new Vector3[waterPoints.Count * 2];
        triangles = new int[(waterPoints.Count - 1) * 2 * 3];

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        for(int i = 1; i < (waterPoints.Count - 1) * 2; ++i)
        {
            triangles[i * 3] = triangles[i * 3 - 2];
            triangles[i * 3 + 1] = triangles[i * 3 - 1];
            triangles[i * 3 + 2] = triangles[i * 3 - 1] + 1;
        }
    }
    private void UpdateMesh()
    {
        for(int i = 0; i < waterPoints.Count; ++i)
        {
            verticles[i * 2] = waterPoints[i].transform.position - transform.position;
            verticles[i * 2 + 1] = new Vector3(waterPoints[i].transform.position.x, transform.position.y - height, 0) - transform.position;
        }
        mesh.Clear();
        mesh.vertices = verticles;
        mesh.triangles = triangles;
    }
}
