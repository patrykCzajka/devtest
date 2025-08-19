using System.Collections.Generic;
using Scripts.Settings;
using UnityEngine;

namespace Scripts.Systems {
    public interface IInitialize { void Initialize(); }
    
    public interface IRequireGameFlow { void Inject(GameFlowService gameFlowService); }
    public interface IRequireGameSettings { void Inject(GameSettingsSO settings); }
    public interface IRequireUI { void Inject(UI.UIModelsReferences ui); }
    public interface IRequirePlayer { void Inject(Player.Player player); }
    public interface IRequireArenaParcel { void Inject(ArenaParcel parcel); }

    public sealed class Bootstrapper : MonoBehaviour {
        [Header("References")]
        [SerializeField] GameSettingsSO gameSettings;
        [SerializeField] GameFlowService gameFlowService;
        [SerializeField] UI.UIModelsReferences uiModels;
        [SerializeField] Player.Player player;
        [SerializeField] ArenaParcel arenaParcel;
        [SerializeField] UpdateBus updateBus;

        void Start() {
            //One pass: gather all behaviors at once
            var behaviours = FindObjectsOfType<MonoBehaviour>(true);

            //Inject dependencies
            foreach (var b in behaviours) {
                if (b is IRequireGameSettings gs) gs.Inject(gameSettings);
                if (b is IRequireGameFlow gf) gf.Inject(gameFlowService);
                if (b is IRequireUI ui) ui.Inject(uiModels);
                if (b is IRequirePlayer rp) rp.Inject(player);
                if (b is IRequireArenaParcel ap) ap.Inject(arenaParcel);
            }

            //Initialize
            var inits = new List<IInitialize>(behaviours.Length);
            foreach (var b in behaviours) if (b is IInitialize init) inits.Add(init);
            for (int i = 0; i < inits.Count; i++) inits[i].Initialize();

            var ticks = new List<ITick>(behaviours.Length);
            var fTicks = new List<IFixedTick>(behaviours.Length);
            foreach (var b in behaviours) {
                if (b is ITick t) ticks.Add(t);
                if (b is IFixedTick ft) fTicks.Add(ft);
            }
            updateBus.Set(ticks.ToArray(), fTicks.ToArray());
        }
    }
}