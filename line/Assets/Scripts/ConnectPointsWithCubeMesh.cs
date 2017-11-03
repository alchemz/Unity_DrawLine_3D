using UnityEngine;
using System.Collections;

public class ConnectPointsWithCubeMesh : MonoBehaviour
{

    // Material used for the connecting lines
    public Material lineMat;

    public float radius = 0.05f;

    // Connect all of the `points` to the `mainPoint`
    public GameObject mainPoint;
    public GameObject[] points;

    // Fill in this with the default Unity Cube mesh
    // We will account for the cube pivot/origin being in the middle.
    public Mesh cubeMesh;


    GameObject[] ringGameObjects;

    // Use this for initialization
    void Start()
    {
        this.ringGameObjects = new GameObject[points.Length];
        //this.connectingRings = new ProceduralRing[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            // Make a gameobject that we will put the ring on
            // And then put it as a child on the gameobject that has this Command and Control script
            this.ringGameObjects[i] = new GameObject();
            this.ringGameObjects[i].name = "Connecting ring #" + i;
            this.ringGameObjects[i].transform.parent = this.gameObject.transform;

            // We make a offset gameobject to counteract the default cubemesh pivot/origin being in the middle
            GameObject ringOffsetCubeMeshObject = new GameObject();
            ringOffsetCubeMeshObject.transform.parent = this.ringGameObjects[i].transform;

            // Offset the cube so that the pivot/origin is at the bottom in relation to the outer ring     gameobject.
            ringOffsetCubeMeshObject.transform.localPosition = new Vector3(0f, 1f, 0f);
            // Set the radius
            ringOffsetCubeMeshObject.transform.localScale = new Vector3(radius, 1f, radius);

            // Create the the Mesh and renderer to show the connecting ring
            MeshFilter ringMesh = ringOffsetCubeMeshObject.AddComponent<MeshFilter>();
            ringMesh.mesh = this.cubeMesh;

            MeshRenderer ringRenderer = ringOffsetCubeMeshObject.AddComponent<MeshRenderer>();
            ringRenderer.material = lineMat;

        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < points.Length; i++)
        {
            // Move the ring to the point
            this.ringGameObjects[i].transform.position = this.points[i].transform.position;

            this.ringGameObjects[i].transform.position = 0.5f * (this.points[i].transform.position + this.mainPoint.transform.position);
            var delta = this.points[i].transform.position - this.mainPoint.transform.position;
            this.ringGameObjects[i].transform.position += delta;

            // Match the scale to the distance
            float cubeDistance = Vector3.Distance(this.points[i].transform.position, this.mainPoint.transform.position);
            this.ringGameObjects[i].transform.localScale = new Vector3(this.ringGameObjects[i].transform.localScale.x, cubeDistance, this.ringGameObjects[i].transform.localScale.z);

            // Make the cube look at the main point.
            // Since the cube is pointing up(y) and the forward is z, we need to offset by 90 degrees.
            this.ringGameObjects[i].transform.LookAt(this.mainPoint.transform, Vector3.up);
            this.ringGameObjects[i].transform.rotation *= Quaternion.Euler(90, 0, 0);
        }
    }
}