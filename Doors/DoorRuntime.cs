using Comfort.Common;
using EFT;
using UnityEngine;

namespace DoorOverhaul.Doors
{
    internal sealed class DoorRuntime : MonoBehaviour
    {
        public static DoorRuntime Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void Update()
        {
            Player player = Singleton<GameWorld>.Instantiated ? Singleton<GameWorld>.Instance.MainPlayer : null;
            if (player != null)
            {
                Patches.KickWeaponReady.Tick(player);
                DoorPusher.Tick(player);
            }

            PeekDriver.Tick();
        }
    }
}
