using Scripts.Interaction;
using UnityEngine;

namespace Scripts.Player {
    public class Player : MonoBehaviour {
        [SerializeField] Transform playerTransform;
        [SerializeField] Transform playerCameraTransform;
        [SerializeField] Camera playerCamera;
        [SerializeField] InteractionController interactionController;
        
        public Transform PlayerTransform => playerTransform;
        public Transform CameraTransform => playerCameraTransform;
        public InteractionController InteractionController => interactionController;
        public Camera PlayerCamera => playerCamera;

        public bool IsCurrentlyHoldingAnObject() => interactionController.IsHoldingANoStealObject();//any object at the time
    }
}