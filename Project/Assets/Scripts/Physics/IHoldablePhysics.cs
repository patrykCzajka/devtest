using Scripts.Interaction;

namespace Scripts.Physics {
    public interface IHoldablePhysics {
        bool IsHeld { get; set; }

        void PhysicalInteract(ObjectEventType eventType);
    }
}