using UnityEngine;

namespace Scripts.Systems {
    public interface ITick { void Tick(); }
    public interface IFixedTick { void FixedTick(); }

    public sealed class UpdateBus : MonoBehaviour {
        ITick[] _ticks = System.Array.Empty<ITick>();
        IFixedTick[] _fixedTicks = System.Array.Empty<IFixedTick>();

        public void Set(ITick[] ticks, IFixedTick[] fixedTicks) {
            _ticks = ticks ?? System.Array.Empty<ITick>();
            _fixedTicks = fixedTicks ?? System.Array.Empty<IFixedTick>();
        }

        void Update() {
            for (int i = 0; i < _ticks.Length; i++) _ticks[i].Tick();
        }

        void FixedUpdate() {
            for (int i = 0; i < _fixedTicks.Length; i++) _fixedTicks[i].FixedTick();
        }
    }
}