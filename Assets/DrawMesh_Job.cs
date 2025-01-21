using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Collections;
using System.Linq;
using Unity.Mathematics;
using static Unity.Mathematics.math;
using Urg;

public enum URG_MeshType
{
    Update,
    Object
}

[System.Serializable]
public struct SensedObject
{
    public Vector3 p0;
    public Vector3 p1;
    public Vector3 center;

    private Vector3[] _vs;

    public Vector3[] vertices
    {
        get
        {
            if (_vs == null)
                _vs = new Vector3[5];
            var width = (p1 - p0).magnitude;
            _vs[0] = p0;
            _vs[1] = center;
            _vs[2] = p1;
            _vs[3] = p0 + center.normalized * width * 0.5f;
            _vs[4] = p1 + center.normalized * width * 0.5f;
            return _vs;
        }
    }
}

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class DrawMesh_Job : MonoBehaviour
{
    [Header("Mesh使用方式")]
    public URG_MeshType m_MeshType;

    [Space, Header("雷達")]
    public UrgSensor urg;

    public Camera RaydaCamera;
    public Transform Center;

    public float StepAngleRadians;
    public float OffsetRadians;

    [Header("使用參數")]
    public Mesh mesh;

    public List<float> Unman_DistanceArray;
    public float[] pre_DistanceArray;
    public float[] DistanceArray;
    public List<SensedObject> DistanceArray_PersonIn;
    public Vector3[] VerticesArray;

    public short[] triangle;

    [Range(0f, 5.0f)] public float objThreshold = 0.5f;
    [Range(0, 10)] public float minWidth = 0.05f;
    [Range(0, 2)] public float distanceMinLimit = 0.2f;
    [Range(0, 65)] public float distanceMaxLimit = 0.2f;

    public Area _area;
    public Bounds sensingArea;
    public Bounds _newsensinArea;
    public Material m_mat;
    public Material mat;
    [Range(0, 1)] public float Offset;
    private object lockObj;
    private Mesh _mesh;
    // private ComputeBuffer verticesBuffer;
    private List<Vector3> verticesData;

    private Mesh sensedObjMesh
    {
        get
        {
            if (_mesh == null)
            {
                _mesh = new Mesh();
                _mesh.vertices = Enumerable.Repeat(Vector3.zero, 5).ToArray();
                _mesh.SetIndices(
                    new[] {
                        0, 1,3,
                        3, 1, 4,
                        4, 1, 2
                    }, MeshTopology.Triangles, 0);
                _mesh.MarkDynamic();
            }
            GetComponent<MeshFilter>().mesh = _mesh;
            return _mesh;
        }
    }

    public void SetSectorVertice()
    {
        mesh.SetVertices(VerticesArray);
    }

    private void OnEnable()
    {
        if (m_MeshType == URG_MeshType.Object) m_mat = new Material(mat);

        urg.OnDistanceReceived += URG_Distance;

        urg.AddFilter(new TemporalMedianFilter(3));
        urg.AddFilter(new SpatialMedianFilter(3));

        pre_DistanceArray = new float[DistanceArray.Length];
        //pref_DistanceArray = new float[DistanceArray.Length];
        //DistoVertice();

        if (m_MeshType == URG_MeshType.Update)
        {
            SetSector();
        }
    }

    private void OnValidate()
    {
        if (m_MeshType == URG_MeshType.Update)
        {
            name = "Sector_Update";
        }
        if (m_MeshType == URG_MeshType.Object)
        {
            name = "Sector_Object";
        }
    }

    private void Update()
    {
        //if (m_MeshType == URG_MeshType.Object) DrawMesh();
        //Vector3 pos = transform.TransformPoint(transform.position);
        //Debug.Log(pos);
        if (m_MeshType == URG_MeshType.Update)
        {
            //DistoVertice();
            SetSectorVertice();
        }
        if (_newsensinArea.center != transform.localPosition)
        {
            _newsensinArea.center = transform.localPosition;
        }

        if (_newsensinArea.extents != sensingArea.extents)
        {
            _newsensinArea.extents = sensingArea.extents;
        }
    }

    private void OnDisable()
    {
        urg.OnDistanceReceived -= URG_Distance;
    }

    private void Start()
    {
        lockObj = new object();
        // verticesBuffer = new ComputeBuffer(1080, sizeof(float) * 3);
        verticesData = new List<Vector3>();
    }

    #region Material

    public void SetMaterialColor(Color color)
    {
        m_mat.SetColor("_Color", color);
    }

    public void SetUpdateMaterialColor(Color color)
    {
        if (m_MeshType == URG_MeshType.Update)
        {
            GetComponent<MeshRenderer>().material.SetColor("_Color", color);
        }
    }

    #endregion Material

    #region SetSector

    private void SetSector()
    {
        Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
        Mesh.MeshData meshData = meshDataArray[0];

        var vertexAttributes = new NativeArray<VertexAttributeDescriptor>(1, Allocator.Temp, NativeArrayOptions.ClearMemory);
        vertexAttributes[0] = new VertexAttributeDescriptor(dimension: 3);

        meshData.SetVertexBufferParams(VerticesArray.Length, vertexAttributes);
        vertexAttributes.Dispose();

        NativeArray<float3> positions = meshData.GetVertexData<float3>();

        for (int i = 0; i < VerticesArray.Length; i++)
        {
            positions[i] = math.lerp(positions[i], VerticesArray[i], 0.001f);
        }

        meshData.SetIndexBufferParams(triangle.Length, IndexFormat.UInt16);
        NativeArray<short> triangleIndices = meshData.GetIndexData<short>();

        for (int j = 0; j < triangle.Length; j++)
        {
            triangleIndices[j] = (short)triangle[j];
        }

        var bounds = new Bounds(Vector3.zero, new Vector3(1f, 1f, 0));

        meshData.subMeshCount = 1;
        meshData.SetSubMesh(0, new SubMeshDescriptor(0, triangle.Length) { bounds = bounds, vertexCount = VerticesArray.Length }, MeshUpdateFlags.DontRecalculateBounds);

        var _mesh = new Mesh
        {
            bounds = bounds,
            name = "Procedural"
        };

        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, _mesh);
        GetComponent<MeshFilter>().mesh = _mesh;
        mesh = _mesh;
    }

    #endregion SetSector

    #region GetData

    public void GetUnmanDistance()
    {
        Unman_DistanceArray.Clear();
        for (int i = 0; i < DistanceArray.Length; i++)
        {
            Unman_DistanceArray.Add(DistanceArray[i]);
        }
    }


    private void URG_Distance(DistanceRecord data)
    {
        DistanceArray = data.RawDistances;

        StepAngleRadians = Mathf.Deg2Rad * 0.25f;
        OffsetRadians = Mathf.Deg2Rad * -135;

        if (VerticesArray == null || VerticesArray.Length < 0)
        {
            VerticesArray = new Vector3[DistanceArray.Length + 1];
            VerticesArray[0] = Vector3.zero;
        }

        for (int i = 0; i < data.RawDistances.Length; i++)
        {
            float angle = StepAngleRadians * i + OffsetRadians;
            float sin = Mathf.Sin(angle);
            float Cos = Mathf.Cos(angle);
            Vector3 dir = new Vector3(sin, Cos, 0);

            var d = data.RawDistances[i] - Offset;
            bool c = false;

            if (m_MeshType == URG_MeshType.Update)
            {
                if (Unman_DistanceArray.Count > 0)
                {
                    if (pre_DistanceArray[i] != 0)
                    {
                        if (Unman_DistanceArray[i] - 0.5f > 0)
                        {
                            if (data.RawDistances[i] < Unman_DistanceArray[i] - 0.5f)
                            {
                                if (Mathf.Abs(data.RawDistances[i] - pre_DistanceArray[i]) < distanceMinLimit)
                                {
                                    d = pre_DistanceArray[i] - Offset;

                                    c = true;
                                }
                            }
                            else
                            {
                                d = Unman_DistanceArray[i];
                            }
                        }
                        else
                        {
                            if (Mathf.Abs(data.RawDistances[i] - pre_DistanceArray[i]) < distanceMinLimit)
                            {
                                d = pre_DistanceArray[i] - Offset;
                                c = true;
                            }
                        }
                    }
                    else if (pre_DistanceArray[i] == 0)
                    {
                        pre_DistanceArray[i] = data.RawDistances[i];
                    }

                    VerticesArray[DistanceArray.Length - i] = dir * d;

                    if (!c) pre_DistanceArray[i] = data.RawDistances[i];
                }
            }
        }
    }

    [ContextMenu("GetData")]
    private void GetData()
    {
        //DistoVertice();
        Settriangle();
    }

    private void GetPointFromDistance(int step, float distance, ref Vector3 pos)
    {
        float angle = step * StepAngleRadians + OffsetRadians;
        pos.x = Mathf.Sin(angle) * distance;
        pos.y = Mathf.Cos(angle) * distance;
    }



    [ContextMenu("SetTriange")]
    private void Settriangle()
    {
        triangle = new short[(VerticesArray.Length - 2) * 3];
        int z = 0;
        for (int i = 0; i < VerticesArray.Length - 2; i++, z += 3)
        {
            triangle[z + 2] = 0;
            triangle[z + 1] = (short)(i + 2);
            triangle[z] = (short)(i + 1);
        }
    }

    [ContextMenu("SJ")]
    private void SaveJson()
    {
        Sector sector = new Sector();
        sector.TriangleArray = triangle;

        string savesource = JsonUtility.ToJson(sector);

        StreamWriter streamWriter = new StreamWriter(Application.streamingAssetsPath + "/../" + "sector.txt");
        streamWriter.Write(savesource);
        streamWriter.Close();
    }

    #endregion GetData
}

public class Sector
{
    public short[] TriangleArray;
}