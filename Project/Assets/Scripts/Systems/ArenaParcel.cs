using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Systems {
    public class ArenaParcel : MonoBehaviour { //TODO: create a serializable cacheable grid with coordinates of specific types of flooring, for ex housefloor1, outside etc
        [SerializeField] float floorLevel;
        [SerializeField] float gridPointRadius = 0.1f;
        [SerializeField] int gridSize = 1;
        [SerializeField] List<GridPoint> _gridPointsSerialized;
        
        Bounds _gameBounds;
        Dictionary<Vector3, FlooringType> _gridPointsDictionary =  new Dictionary<Vector3, FlooringType>();
        const string INSIDE_FLOORING_TAG_NAME = "InsideFlooring"; //TODO: create a const master struct or place it in scriptable object data container

        public enum FlooringType {
            Inside,
            Outside,
            Undefined
        }

        void Awake() {
            foreach (var gridPoint in _gridPointsSerialized) {
                _gridPointsDictionary.Add(gridPoint.position, gridPoint.flooring);
            }
        }

        [ContextMenu("Setup")]
        void Setup() {
            _gridPointsSerialized = new List<GridPoint>();
            _gridPointsDictionary =  new Dictionary<Vector3, FlooringType>();
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(this, "ArenaParcel Setup");
#endif
            EstablishGrounds();
            GenerateGridPoints();
        }
        void EstablishGrounds() {
            _gameBounds = new Bounds(Vector3.zero, Vector3.one);
            foreach (var renderer in FindObjectsByType<Renderer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)) {
                _gameBounds.Encapsulate(renderer.bounds);
            }
        }
        public FlooringType GetFlooringType(Vector3 position) {
            float x = Mathf.Round(position.x / gridSize) * gridSize;
            float y = floorLevel;
            float z = Mathf.Round(position.z / gridSize) * gridSize;
            return _gridPointsDictionary.GetValueOrDefault(new Vector3(x,y,z), FlooringType.Undefined);
        }
        
        void GenerateGridPoints()
        {
            Vector3 min = _gameBounds.min;
            Vector3 max = _gameBounds.max;

            float startX = Mathf.Floor(min.x / gridSize) * gridSize;
            float endX   = Mathf.Ceil(max.x / gridSize) * gridSize;

            float startZ = Mathf.Floor(min.z / gridSize) * gridSize;
            float endZ   = Mathf.Ceil(max.z / gridSize) * gridSize;

            for (float x = startX; x <= endX; x += gridSize)
            {
                for (float z = startZ; z <= endZ; z += gridSize)
                {
                    Vector3 point = new Vector3(x, floorLevel, z);
                    var raycastHits = UnityEngine.Physics.RaycastAll(point + Vector3.up * 10, Vector3.down * 20);
                    _gridPointsSerialized.Add(new GridPoint(point, GetFlooringType(raycastHits)));
                }
            }
        }

        FlooringType GetFlooringType(RaycastHit[] hits) {
            foreach (var raycastHit in hits) {
                if(raycastHit.transform.gameObject.CompareTag(INSIDE_FLOORING_TAG_NAME))
                    return FlooringType.Inside;
            }
            return FlooringType.Outside;
        }

        void OnDrawGizmos() {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(_gameBounds.center, _gameBounds.size);
            foreach (var gridPoint in _gridPointsSerialized) {
                Gizmos.color = gridPoint.flooring == FlooringType.Outside ? Color.green : Color.red;   
                Gizmos.DrawSphere(gridPoint.position, gridPointRadius);
            }
        }
        
        
        [System.Serializable]
        public struct GridPoint {
            public Vector3 position;
            public FlooringType flooring;

            public GridPoint(Vector3 position, FlooringType flooring) {
                this.position = position;
                this.flooring = flooring;
            }
        }
    }
}