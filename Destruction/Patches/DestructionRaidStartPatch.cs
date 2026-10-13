using System;
using System.Reflection;
using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;

namespace DoorOverhaul.Destruction.Patches
{
    internal class DestructionRaidStartPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(GameWorld), nameof(GameWorld.OnGameStarted));
        }

        [PatchPostfix]
        private static void Postfix(GameWorld __instance)
        {
            try
            {
                DoorDamageIntake.Clear();
                if (Settings.DestructionEnabled.Value)
                {
                    DoorDestructionRegistry.Build(__instance);
                }
            }
            catch (Exception ex)
            {
                DoorOverhaulPlugin.Log.LogError($"Destruction: raid start failed: {ex}");
            }
        }
    }
}
