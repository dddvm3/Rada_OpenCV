using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

//using HKY;
//using AmplifyShaderEditor;

namespace Urg
{
    [System.Serializable]
    public class Group
    {
        public List<int> id;
        public List<float> dist;
        public int ID;
        private readonly Vector3[] cachedDirs;
        private Vector3 _position;
        private bool positionSet;

        public Group(int value)
        {
            id = new List<int>();
            dist = new List<float>();
            ID = value;
        }

        public int medianId
        { get { return id[id.Count / 2]; } }

        public float medianDist
        { get { return dist[dist.Count / 2]; } }

        public Vector3 position
        {
            get
            {
                if (!positionSet) Debug.LogError("position has not bee set yet");
                return _position;
            }
            set { _position = value; }
        }

        public void GetPosition()
        {
            position = CalculatePosition();
            positionSet = true;
        }

        private Vector2 CalculatePosition()
        {
            return CalculatePosition(cachedDirs[medianId], medianDist);
        }

        private Vector2 CalculatePosition(Vector3 dir, float dist)
        {
            float angle = Vector3.Angle(dir, Vector3.right);
            float theta = angle * Mathf.Deg2Rad;
            float x = Mathf.Cos(theta) * dist;
            float y = Mathf.Sin(theta) * dist;
            return new Vector2(x, y);
        }
    }

    public class ProcessedObject
    {
        public readonly System.Guid guid;
        public float size;
        public float birthTime;
        public int missingFrame = 0;
        public bool useSmooth = true;
        private static readonly int MISSING_FRAME_LIMIT = 100;
        private Vector3 currentVelocity;
        private Vector3 oldPosition;
        private float posSmoothTime = 0.2f;

        public ProcessedObject(Vector3 position, float size, float objectPositionSmoothTime = 0.6f)
        {
            guid = System.Guid.NewGuid();
            this.position = position;
            this.size = size;

            currentVelocity = new Vector3();
            birthTime = Time.time;
        }

        public Vector3 position { get; private set; }
        public Vector3 deltaMovement { get; private set; }

        public float age
        { get { return Time.time - birthTime; } }

        public bool clear { get; private set; }

        public void Update(Vector3 newPos, float newSize)
        {
            size = newSize;

            oldPosition = position;

            if (useSmooth)
            {
                position = Vector3.SmoothDamp(position, newPos, ref currentVelocity, posSmoothTime);
            }
            else
            {
                position = newPos;
            }
            missingFrame = 0;

            deltaMovement = position - oldPosition;
        }

        public void Update()
        {
            missingFrame++;
            if (missingFrame >= MISSING_FRAME_LIMIT)
            {
                clear = true;
            }
        }
    }

    public class DebugRenderer : MonoBehaviour
    {
        public UrgSensor urg;
        public List<float> rawDistance_SpaceSroup;
        public Material Mat_DebugCube;
        public bool bool_closeRay;
        [Range(0, 10)] public float PersonSize = 10;
        [Range(0, 10)] public float LeaveOriginPos;
        [Range(0, 1)] public float DistanceRange;
        public List<GameObject> isMove;

        //public List<Group> rawPersonList;
        public List<Group> PersonList;

        public List<GameObject> ObjectCenter;

        public Vector4 URGpos;
        public float[] URGAngle;
        public Material URG;
        public Mesh _mesh;
        public List<Vector3> URGPos;
        public Material mat;

        [SerializeField] private float[] rawDistances;
        [SerializeField] private float[] RawDistances;

        private List<DetectedLocation> locations = new List<DetectedLocation>();

        private List<List<int>> clusterIndices;

        private AffineConverter affineConverter;

        private List<GameObject> debugObjects;

        private Object syncLock = new Object();
        private System.Diagnostics.Stopwatch stopwatch;
        private EuclidianClusterExtraction cluster;

        public void RayLineSwitch()
        {
            if (!bool_closeRay)
            {
                bool_closeRay = true;
            }
            else
            {
                bool_closeRay = false;
            }
        }

        public void Groups()
        {
            PersonList.Clear();
            var allgroup = new List<Group>();

            //int GroupID = 0;
            bool isGroup = false;
            for (int i = 1; i < rawDistances.Length - 1; i++)
            {
                float deltaA = Mathf.Abs(rawDistances[i] - rawDistances[i - 1]);
                float deltaB = Mathf.Abs(rawDistances[i + 1] - rawDistances[i]);

                if ((rawDistances[i] - rawDistance_SpaceSroup[i]) > LeaveOriginPos && (deltaA < DistanceRange && deltaB < DistanceRange))
                {
                    if (!isGroup)
                    {
                        isGroup = true;

                        Group group = new Group(i);

                        group.dist.Add(rawDistances[i]);
                        group.id.Add(i);
                        PersonList.Add(group);
                    }
                    else
                    {
                        var group = PersonList[PersonList.Count - 1];

                        group.id.Add(i);
                        group.dist.Add(rawDistances[i]);
                    }
                }
                else
                {
                    if (isGroup)
                    {
                        isGroup = false;
                    }
                }
            }
            PersonList.RemoveAll(item => item.id.Count < PersonSize);
            //return allgroup;
        }

        private void Awake()
        {
            urg.OnDistanceReceived += Urg_OnDistanceReceived;

            urg.AddFilter(new TemporalMedianFilter(3));
            urg.AddFilter(new SpatialMedianFilter(3));

            debugObjects = new List<GameObject>();
            Material m = Instantiate(Mat_DebugCube);
            for (var i = 0; i < 1081; i++)
            {
                var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.transform.parent = transform;
                obj.transform.localScale = 0.1f * Vector3.one;
                obj.tag = "Player";
                obj.layer = 8;
                obj.name = i.ToString();
                obj.AddComponent<UrgCube>();
                obj.GetComponent<UrgCube>().SetCubeNum(i);
                obj.GetComponent<Renderer>().material = m;
                //obj.GetComponent<BoxCollider>().isTrigger = true;

                obj.AddComponent<Rigidbody>();
                //obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                obj.GetComponent<Rigidbody>().isKinematic = true;
                debugObjects.Add(obj);
            }
        }

        private void OnEnable()
        {
        }

        private void Start()
        {
            for (int i = 0; i < rawDistances.Length; i++)
            {
                rawDistance_SpaceSroup.Add(rawDistances[i]);
                debugObjects[i].GetComponent<UrgCube>().Origin_Dis = rawDistances[i];
            }
            URGAngle = new float[rawDistances.Length];
        }

        private void Update()
        {
            if (urg == null)
            {
                return;
            }

            if (rawDistances != null && rawDistances.Length > 0)
            {
                URGPos.Clear();
                for (int i = 0; i < rawDistances.Length; i++)
                {
                    float distance = rawDistances[i];
                    float angle = urg.StepAngleRadians * i + urg.OffsetRadians;
                    var cos = Mathf.Cos(angle);
                    var sin = Mathf.Sin(angle);
                    var dir = new Vector3(cos, 0, sin);
                    var pos = distance * dir;

                    debugObjects[i].transform.localPosition = pos;
                    URGPos.Add(pos);
                    URGpos = new Vector4(pos.x, pos.z, angle, distance);
                    if (!bool_closeRay)
                    {
                        Debug.DrawRay(urg.transform.position, pos, Color.blue);
                    }
                }
            }

            if (rawDistance_SpaceSroup != null)
            {
                Groups();
            }
            if (Input.GetKey(KeyCode.LeftControl) | Input.GetKeyDown(KeyCode.S))
            {
                //SaveJson();
            }
        }

        private void Urg_OnDistanceReceived(DistanceRecord data)
        {
            //Debug.LogFormat("distance received: SCIP timestamp={0} unity timer={1}", data.Timestamp, stopwatch.ElapsedMilliseconds);
            //Debug.LogFormat("cluster count: {0}", data.ClusteredIndices.Count);
            this.rawDistances = data.RawDistances;
            //this.RawDistances = raw.Distance;

            this.locations = data.FilteredResults;
            this.clusterIndices = data.ClusteredIndices;
        }

        //private void SaveJson()
        //{
        //    //urgDistance urgd = new urgDistance();
        //    urgd.Dis = rawDistances;

        //    string savesource = JsonUtility.ToJson(urgd);

        //    StreamWriter streamWriter = new StreamWriter(Application.streamingAssetsPath + "/../" + "urgDistance.txt");
        //    streamWriter.Write(savesource);
        //    streamWriter.Close();
        //}
    }
}