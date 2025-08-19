using System;
using Scripts.Interaction;
using UnityEngine;

namespace Scripts.Physics {
    public class PhysicalHoldable : MonoBehaviour, IHoldablePhysics {
        public event Action<PhysicalHoldable> OnTriggerEnterEvent;
        public bool IsHeld { get; set; }
        Rigidbody _rb;
        BoxCollider _col;
        Vector3 _triggerColliderSize;
        Vector3 _colliderSize;
        void Awake() {
            _rb = GetComponentInChildren<Rigidbody>();
            _col = GetComponentInChildren<BoxCollider>();
            _colliderSize = _col.bounds.size;
            _triggerColliderSize = _colliderSize * 0.5f; //TODO: DO A BETTER SOLUTION, REMOVE HARDCODED 0.5 VALUE, MAKE MORE ROBUST
        }

        void OnTriggerEnter(Collider other) {
            OnTriggerEnterEvent?.Invoke(this);
        }

        void OnPickUp() {
            if (IsHeld)
                return;
            _rb.isKinematic = true;
            _col.isTrigger = true;
            _col.size = _triggerColliderSize;
            IsHeld = true;
        }
        void OnPutDown() {
            if (!IsHeld)
                return;
            _rb.isKinematic = false;
            _col.isTrigger = false;
            _col.size = _colliderSize;
            IsHeld = false;
        }
        
        public void PhysicalInteract(ObjectEventType eventType) {
            switch (eventType) {
                case ObjectEventType.PickedUp:
                    OnPickUp();
                    break;
                case ObjectEventType.PutDown:
                    OnPutDown();
                    break;
                case ObjectEventType.Custom:
                default:
                    throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
            }
        }
    }
}
