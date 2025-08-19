using Scripts.Settings;
using Scripts.Systems;
using Scripts.UI;
using UnityEngine;

namespace Scripts.Interaction {
    public class InteractionController : MonoBehaviour, ITick, IRequireGameSettings, IRequirePlayer, IRequireUI {

        bool _busyHoldingItem;
        IInteractable _interactable;
        float _interactionDistance;
        LayerMask _interactionLayer;
        Camera _playerCamera;
        Transform _playerCameraTransform;
        bool _interactionKeyPressed;
        Vector3 _pickUpOffset;
        RelativePose _relativePose;
        UIModelsReferences _uiRefs;

        public bool IsHoldingANoStealObject() => _busyHoldingItem;

        public void Inject(GameSettingsSO settings) {
            _interactionDistance = settings.InteractionDistance;
            _interactionLayer = settings.InteractionLayer;
            _pickUpOffset = settings.PickUpOffset;
        }
        public void Inject(Player.Player player) {
            _playerCamera = player.PlayerCamera;
            _playerCameraTransform = player.CameraTransform;
        }
        
        public void Tick()
        {
            if (_busyHoldingItem)
            {
                if (_interactable == null)
                {
                    _busyHoldingItem = false;
                    _uiRefs.InteractionTipUIModel.HideTip();
                    return;
                }

                _relativePose.ApplyTo(_playerCameraTransform, _interactable.GetTransform());
                _uiRefs.InteractionTipUIModel.ShowTip(_interactable.GetExitInteractionPrompt());

                if (Input.GetKeyDown(_interactable.GetKeyCode()))
                {
                    TryPutDownObject(_interactable);
                }
                return;
            }

            CheckForInteractable();

            if (_interactable != null)
            {
                _uiRefs.InteractionTipUIModel.ShowTip(_interactable.GetInteractionPrompt());

                if (Input.GetKeyDown(_interactable.GetKeyCode()))
                {
                    _interactable.Interact(ObjectEventType.PickedUp);
                    _relativePose = new RelativePose(_playerCameraTransform, _interactable.GetTransform(), _pickUpOffset);
                    _busyHoldingItem = true;
                }
            }
            else
            {
                _uiRefs.InteractionTipUIModel.HideTip();
            }
        }

        public void TryPutDownObject(IInteractable stealObject) {
            if (stealObject == _interactable && _busyHoldingItem) {
                _interactable.Interact(ObjectEventType.PutDown);
                _busyHoldingItem = false;
            }
        }


        void CheckForInteractable() {
            Ray ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);
            if (UnityEngine.Physics.Raycast(ray, out RaycastHit hit, _interactionDistance, _interactionLayer)) {
                _interactable = hit.collider.GetComponent<IInteractable>();
            }
            else {
                _interactable = null;
            }
        }

        public void Inject(UIModelsReferences uiModelsReferences) {
            _uiRefs = uiModelsReferences;
        }
    }
}