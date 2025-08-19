using System.Collections.Generic;
using System.Linq;
using Scripts.Interaction;
using Scripts.Physics;
using Scripts.UI;
using UnityEngine;

namespace Scripts.Systems {
    public class LootProgressService : MonoBehaviour, IRequireArenaParcel, IRequirePlayer, IRequireUI, IRequireGameFlow {
        readonly List<StealHoldable> _stealObjects = new();
        ArenaParcel _arenaParcel;
        InteractionController _interactionController;
        int _numberOfObjectsStolen;
        int _numberOfObjectsRequiredToSteal;

        Dictionary<IInteractable, ArenaParcel.FlooringType> objectToFlooringType =
            new Dictionary<IInteractable, ArenaParcel.FlooringType>();

        UIModelsReferences _uiRefs;
        GameFlowService _gameFlowService;

        void OnEnable() {
            foreach (var obs in FindObjectsByType<StealHoldable>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)) {
                Register(obs);
                obs.OnTriggerEnterEvent += HandleOnTriggerEnter;
                _numberOfObjectsRequiredToSteal++;
            }
        }

        void HandleOnTriggerEnter(PhysicalHoldable obj) {
            _interactionController.TryPutDownObject(obj as IInteractable);
        }

        void OnDisable() {
            if (_stealObjects == null) return;

            for (int i = 0; i < _stealObjects.Count; i++) {
                var obs = _stealObjects[i];
                obs.OnTriggerEnterEvent -= HandleOnTriggerEnter;
                if (obs != null)
                    obs.OnObjectEvent -= HandleObjectEvent;
            }

            _stealObjects.Clear();
            _numberOfObjectsRequiredToSteal = 0;
        }

        public void Inject(ArenaParcel arenaParcel) {
            _arenaParcel = arenaParcel;
        }

        void Register(StealHoldable observable) {
            if (observable == null) return;
            _stealObjects.Add(observable);
            observable.OnObjectEvent += HandleObjectEvent;
        }

        void HandleObjectEvent(ObjectEvent evt) {
            switch (evt.Type) {
                case ObjectEventType.PickedUp: OnAnyPickedUp(evt); break;
                case ObjectEventType.PutDown: OnAnyPutDown(evt); break;
                case ObjectEventType.Custom:
                default: OnAnyCustom(evt); break;
            }

            UpdateObjectsStolenAndNotify();
        }


        void OnAnyPutDown(ObjectEvent evt) {
            if (objectToFlooringType.ContainsKey(evt.sender)) {
                objectToFlooringType[evt.sender] = _arenaParcel.GetFlooringType(evt.position);
                return;
            }

            objectToFlooringType.Add(evt.sender, _arenaParcel.GetFlooringType(evt.position));
        }

        void OnAnyPickedUp(ObjectEvent evt) {
            if (!objectToFlooringType.TryAdd(evt.sender, ArenaParcel.FlooringType.Undefined)) {
                objectToFlooringType[evt.sender] = ArenaParcel.FlooringType.Undefined;
            }
        }

        void UpdateObjectsStolenAndNotify() {
            _numberOfObjectsStolen = objectToFlooringType.Count(x => x.Value == ArenaParcel.FlooringType.Outside);
            _uiRefs.PlayerProgressUIModel.VisualizeStealProgress(_numberOfObjectsStolen,
                _numberOfObjectsRequiredToSteal);
            if (_numberOfObjectsStolen >= _numberOfObjectsRequiredToSteal)
                _gameFlowService.TriggerGameFinished();
        }

        void OnAnyCustom(ObjectEvent _) {
            /* ... */
        }

        public void Inject(Player.Player player) {
            _interactionController = player.InteractionController;
        }

        public void Inject(UIModelsReferences ui) {
            _uiRefs = ui;
        }

        public void Inject(GameFlowService gameFlowService) {
            _gameFlowService = gameFlowService;
        }
    }
}