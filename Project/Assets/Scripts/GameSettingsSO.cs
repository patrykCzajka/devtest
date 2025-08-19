using UnityEngine;

namespace Scripts.Settings {
    [CreateAssetMenu(fileName = "GameSettingsSO", menuName = "Scriptable Objects/GameSettingsSO")]
    public class GameSettingsSO : ScriptableObject {
        [Header("Player Parameters")] [SerializeField] [Range(1f, 9f)]
        float playerSpeed = 3;

        [SerializeField] [Range(1f, 9f)] float playerHoldingObjectSpeed = 2;
        [SerializeField] LayerMask visibleObjectsLayers;

        [Header("Game Completion Parameters")]
        [SerializeField] int lockdownGateCloseTimeInSeconds = 10;

        [Header("Surveillance Movement Parameters")] [SerializeField] [Range(0f, 89f)]
        float cameraRotationAngle = 45;

        [SerializeField] [Range(1f, 50f)] float cameraRotationSpeed = 15;
        [SerializeField] [Range(1f, 10f)] float cameraEdgeRotationPauseDuration = 2;

        [Header("Surveillance Detection Parameters")] [SerializeField] [Range(1f, 99f)]
        float recognitionDistance = 99;

        [SerializeField] [Range(1f, 179f)] float recognitionAngle = 25;
        [SerializeField] [Range(1f, 9f)] float detectionTime = 4;
        [SerializeField] bool stealthRetrievalOn = true;
        [SerializeField] float stealthRetrievalRate = 0.5f;
        [SerializeField] float stealingStatusStealthLoseSpeed = 2f;
        [SerializeField] Material redStealingMat;
        [SerializeField] Material redInSightMat;
        [SerializeField] Material outOfRangeBlackMat;

        [Header("Interaction Parameters")] [SerializeField]
        LayerMask interactionLayer;

        [SerializeField] [Range(0f, 5f)] float interactionDistance = 3;
        [SerializeField] Vector3 pickUpOffset;

        public float PlayerSpeed => playerSpeed;
        public float PlayerHoldingObjectSpeed => playerHoldingObjectSpeed;
        public float DetectionTime => detectionTime;

        public bool StealthRetrievalOn => stealthRetrievalOn;
        public float StealthRetrievalRate => stealthRetrievalRate;
        public float StealingStatusStealthLoseSpeed => stealingStatusStealthLoseSpeed;
        public float CameraRotationAngle => cameraRotationAngle;
        public float CameraRotationSpeed => cameraRotationSpeed;
        public float CameraEdgeRotationPauseDuration => cameraEdgeRotationPauseDuration;
        public float RecognitionAngle => recognitionAngle;
        public float RecognitionDistance => recognitionDistance;

        public LayerMask VisibleObjectsLayers =>
            visibleObjectsLayers; //all visible objects' layers apart from player and interactables

        public LayerMask InteractionLayer => interactionLayer;
        public float InteractionDistance => interactionDistance;
        public Material RedStealingMat => redStealingMat;
        public Material RedInSightMat => redInSightMat;
        public Material OutOfRangeBlackMat => outOfRangeBlackMat;
        public int LockdownGateCloseTimeInSeconds => lockdownGateCloseTimeInSeconds;
        public Vector3 PickUpOffset => pickUpOffset;
    }
}