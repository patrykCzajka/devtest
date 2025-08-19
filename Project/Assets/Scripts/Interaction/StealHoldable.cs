using System;
using Scripts.Physics;
using UnityEngine;

namespace Scripts.Interaction {
    public class StealHoldable : PhysicalHoldable, IInteractable {
        [SerializeField] KeyCode interactionKey = KeyCode.E;
        [SerializeField] Transform thisTransform;
        public event Action<ObjectEvent> OnObjectEvent;
        
        public void Interact(ObjectEventType eventType) {
            PhysicalInteract(eventType);
            OnObjectEvent?.Invoke(new ObjectEvent(eventType, thisTransform.position, this));
        }

        public string GetInteractionPrompt() {
            return "Press E to pick up";
        }

        public string GetExitInteractionPrompt() {
            return "Press E to put down";
        }

        public KeyCode GetKeyCode() {
            return interactionKey;
        }

        public Transform GetTransform() => thisTransform;
        
    }
    
    public enum ObjectEventType { PickedUp, PutDown, Custom }

    public readonly struct ObjectEvent
    {
        public readonly ObjectEventType Type;
        public readonly Vector3 position;
        public readonly IInteractable sender;

        public ObjectEvent(ObjectEventType type, Vector3 position, IInteractable sender)
        {
            Type = type; 
            this.position = position;
            this.sender = sender;
        }
    }
}
