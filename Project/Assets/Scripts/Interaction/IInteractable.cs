
using UnityEngine;

namespace Scripts.Interaction {
    public interface IInteractable {
        void Interact(ObjectEventType eventType);
        string GetInteractionPrompt(); 
        string GetExitInteractionPrompt(); 
        KeyCode GetKeyCode();
        
        Transform GetTransform();
    }
}