using BepInEx;
using BepInEx.Logging;
using DoorOverhaul.Doors;
using DoorOverhaul.Patches;
using UnityEngine;

namespace DoorOverhaul
{
    [BepInPlugin(Guid, Name, Version)]
    [BepInDependency("com.SPT.core", "4.1.0")]
    public class DoorOverhaulPlugin : BaseUnityPlugin
    {
        public const string Guid = "com.sage.dooroverhaul";
        public const string Name = "Door Overhaul";
        public const string Author = "Sage";
        public const string Version = "1.0.0";

        internal static ManualLogSource Log;

        private void Awake()
        {
            Log = Logger;

            Settings.Bind(Config);

            EnsureHost();

            Enable(new AddDoorActionsPatch());
            Enable(new InteractKeyCapturePatch());
            Enable(new PartialOpenSwingPatch());
            Enable(new QuietOpenSoundPatch());
            Enable(new RaidEndCleanupPatch());
            Enable(new Destruction.Patches.DestructionRaidStartPatch());
            Enable(new Destruction.Patches.DestructionPortalPatch());
            Enable(new Destruction.Patches.DestructionBreachPatch());
            Enable(new Destruction.Patches.DestructionInteractGuardPatch());
            Enable(new Destruction.Patches.DestructionSetStateGuardPatch());
            Enable(new Destruction.Patches.DestructionKickGuardPatch());
            Enable(new KickWeaponReadyPatch());
            Enable(new KickWeaponForwardPatch());

            Log.LogInfo($"{Name} {Version} by {Author} loaded" + (_failedPatches > 0 ? $" ({_failedPatches} patch(es) failed, see above)" : string.Empty));
        }

        private static int _failedPatches;

        private static void Enable(SPT.Reflection.Patching.ModulePatch patch)
        {
            try
            {
                patch.Enable();
            }
            catch (System.Exception ex)
            {
                _failedPatches++;
                Log.LogError($"{patch.GetType().Name} could not be applied, that feature is off: {ex.Message}");
            }
        }

        private void Update()
        {
            EnsureHost();
        }

        private static GameObject _host;
        private static void EnsureHost()
        {
            if (_host != null)
            {
                return;
            }

            _host = new GameObject("DoorOverhaul");
            _host.hideFlags = HideFlags.HideAndDontSave;
            DontDestroyOnLoad(_host);
            _host.AddComponent<DoorRuntime>();
            _host.AddComponent<Destruction.DestructionController>();
        }
    }
}
