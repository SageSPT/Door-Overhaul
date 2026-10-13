using BepInEx.Configuration;

namespace DoorOverhaul
{
    internal static class Settings
    {
        private const string General = "1. General";
        private const string Peek = "2. Peek";
        private const string Quiet = "3. Open Quietly";
        private const string Destruction = "4. Destruction";
        private const string DestructionDbg = "5. Destruction Debug";

        public static ConfigEntry<bool> KickWeaponReady;
        public static ConfigEntry<bool> KickWeaponForward;
        public static ConfigEntry<bool> BodyPush;
        public static ConfigEntry<float> BodyPushStrength;

        public static ConfigEntry<float> SecondsToFullyOpen;
        public static ConfigEntry<int> MinimumPeekPercent;
        public static ConfigEntry<int> QuickPeekPercent;
        public static ConfigEntry<bool> SilentPeek;

        public static ConfigEntry<int> QuietOpenPercent;
        public static ConfigEntry<float> QuietSwingSlowdown;
        public static ConfigEntry<float> QuietVolume;

        public static ConfigEntry<bool> DestructionEnabled;
        public static ConfigEntry<bool> ShowHoles;
        public static ConfigEntry<bool> HingeCollapse;
        public static ConfigEntry<bool> LeafBlocksPlayers;
        public static ConfigEntry<float> HoleSize;
        public static ConfigEntry<float> ThroughPenWoodThin;
        public static ConfigEntry<float> ThroughPenWoodThick;
        public static ConfigEntry<float> ThroughPenMetalThin;
        public static ConfigEntry<float> CellSizeCm;

        public static ConfigEntry<bool> ShowImpacts;
        public static ConfigEntry<bool> ShowDoorInfo;
        public static ConfigEntry<bool> ShowDamageMap;

        private static int _order = 1000;

        public static void Bind(ConfigFile config)
        {
            KickWeaponReady = config.Bind(General, "Kick with weapon ready", true, Describe(
                "While kicking a door your weapon stays ready and can be hip-fired. Aiming down sights is blocked during the kick."));
            KickWeaponForward = config.Bind(General, "Kick: keep weapon pointed forward", true, Describe(
                "During a kick the weapon keeps pointing where you look (and hip-fire goes there) instead of turning " +
                "sideways with the kicking body. Needs 'Kick with weapon ready'."));
            BodyPush = config.Bind(General, "Push doors with your body", true, Describe(
                "Walking into an open or peeked door pushes it further open. Shut (latched) doors don't move."));
            BodyPushStrength = config.Bind(General, "Body push strength", 1f, Describe(
                "How readily the door swings away when you walk into it (1 = moves at your walking speed).",
                new AcceptableValueRange<float>(0.3f, 3f)));

            SecondsToFullyOpen = config.Bind(Peek, "Seconds to fully open", 2.5f, Describe(
                "With Peek or Open Further picked in the scroll list, hold F: this is how long it takes to go from " +
                "closed to fully open. Let go to stop.",
                new AcceptableValueRange<float>(0.5f, 10f)));
            MinimumPeekPercent = config.Bind(Peek, "Initial crack (%)", 8, Describe(
                "How far the door unlatches when you start holding Peek on a closed door. Holding opens it further from there.",
                new AcceptableValueRange<int>(1, 50)));
            QuickPeekPercent = config.Bind(Peek, "Tap peek (%)", 33, Describe(
                "How far a quick tap of F on Peek opens the door. (A tap on Open Further opens it fully.)",
                new AcceptableValueRange<int>(5, 100)));
            SilentPeek = config.Bind(Peek, "Silent peek", false, Describe(
                "Muffle the opening sound when peeking, like Open Quietly does."));

            QuietOpenPercent = config.Bind(Quiet, "Open amount (%)", 60, Describe(
                "How far Open Quietly opens the door.",
                new AcceptableValueRange<int>(10, 100)));
            QuietSwingSlowdown = config.Bind(Quiet, "Swing slowdown", 2f, Describe(
                "How many times slower than normal the door swings when opened quietly.",
                new AcceptableValueRange<float>(1f, 5f)));
            QuietVolume = config.Bind(Quiet, "Sound volume", 0f, Describe(
                "Volume of the opening/squeak sound when opening quietly (0 = silent, 1 = normal).",
                new AcceptableValueRange<float>(0f, 1f)));

            DestructionEnabled = config.Bind(Destruction, "Destructible doors", true, Describe(
                "Master switch. Hits on supported doors tear holes that you and bullets can see and shoot through. " +
                "Pieces that break loose fall and stay on the floor; small bits burst into splinters. Hanging and falling doors are a separate experimental switch below. " +
                "Turns itself off while Fika is installed (not synced yet)."));
            ShowHoles = config.Bind(Destruction, "Show holes", true, Describe(
                "Cut the damage into the door's own model and its bullet collider. Off = damage is only simulated " +
                "(debug overlay), doors stay intact; turning it off mid-raid restores the doors."));
            HingeCollapse = config.Bind(Destruction, "Hanging and falling doors (experimental)", false, Describe(
                "Hinges can be shot out: a door on one hinge hangs from it, with none it falls out of its frame. Still rough " +
                "(it can jam, clip or behave oddly). Off = hinges are indestructible and the door stays a door; holes, falling " +
                "pieces and the latch work either way. Applies to new hits."));
            LeafBlocksPlayers = config.Bind(Destruction, "Hanging and fallen doors block players", true, Describe(
                "(Experimental doors) A door hanging from one hinge, or lying on the floor, is solid for players and bots (step over a flat one). " +
                "Off = you walk through it like through the small pieces. Applies to doors that come loose afterwards."));
            HoleSize = config.Bind(Destruction, "Hole size", 2f, Describe(
                "How much wood each hit tears out. 1 = a 5.56 round takes one ~5 cm piece; 2 = about a 10 cm hole per round " +
                "and ~15 cm from 10 rounds in one spot; 3 = very destructive. Applies to new hits.",
                new AcceptableValueRange<float>(0.5f, 3f)));
            ThroughPenWoodThin = config.Bind(Destruction, "Punch-through penetration: thin wood", 25f, Describe(
                "Penetration power a round needs to punch a hole through thin wooden doors. Weaker rounds still damage the surface, latch and hinges.",
                new AcceptableValueRange<float>(1f, 100f)));
            ThroughPenWoodThick = config.Bind(Destruction, "Punch-through penetration: thick wood", 30f, Describe(
                "Same for thick wooden doors.", new AcceptableValueRange<float>(1f, 100f)));
            ThroughPenMetalThin = config.Bind(Destruction, "Punch-through penetration: thin metal", 40f, Describe(
                "Same for thin metal doors (holes and hinge/latch damage only, no chunks).", new AcceptableValueRange<float>(1f, 100f)));
            CellSizeCm = config.Bind(Destruction, "Fracture cell size (cm)", 5f, Describe(
                "Size of the pieces a door can lose. Smaller = finer holes, more work per door. Applies to doors not yet damaged this raid.",
                new AcceptableValueRange<float>(3f, 10f)));

            ShowImpacts = config.Bind(DestructionDbg, "Show impacts", false, Describe(
                "Mark recent door hits on screen with penetration, damage and whether they would punch through."));
            ShowDoorInfo = config.Bind(DestructionDbg, "Show door info", false, Describe(
                "Show the looked-at door's id, type, material and whether it is destructible (and why not)."));
            ShowDamageMap = config.Bind(DestructionDbg, "Show damage map", false, Describe(
                "Show the looked-at door's simulated fracture cells: health, holes and pieces that would fall."));
        }

        private static ConfigDescription Describe(string text, AcceptableValueBase range = null)
        {
            return new ConfigDescription(text, range, new ConfigurationManagerAttributes { Order = _order-- });
        }
    }

    internal sealed class ConfigurationManagerAttributes
    {
        public int? Order;
    }
}
