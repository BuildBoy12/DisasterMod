using Assets._Scripts.Dissonance;
using EXILED;
using EXILED.Extensions;
using Harmony;

namespace DisasterMod
{
	[HarmonyPatch(typeof(PlayerMovementSync), nameof(PlayerMovementSync.AntiCheatKillPlayer))]
	public static class PreventAnticheatKill
    {
		public static bool Prefix(PlayerMovementSync __instance, string message)
        {
			Log.Warn($"{__instance._hub.GetNickname()}: {message}");
			return false;
        }
    }

	[HarmonyPatch(typeof(DissonanceUserSetup), nameof(DissonanceUserSetup.CallCmdAltIsActive))]
	public static class Scp0492Speak
	{
		public static bool Prefix(DissonanceUserSetup __instance, bool value)
		{
			CharacterClassManager ccm = __instance.gameObject.GetComponent<CharacterClassManager>();

			if (ccm.CurClass == RoleType.Scp0492) 
				__instance.MimicAs939 = value;

			return true;
		}
	}

	[HarmonyPatch(typeof(MTFRespawn), nameof(MTFRespawn.SummonChopper))]
	public static class StopChopperAfterDetonatedPatch
	{
		public static bool Prefix()
		{
			if (Configs.stop_respawn_after_detonation && AlphaWarheadController.Host.detonated) return false;
			else return true;
		}
	}

	[HarmonyPatch(typeof(PlayerInteract), nameof(PlayerInteract.CallCmdContain106))]
	public static class Scp106ContainPatch
    {
		public static bool Prefix()
        {
			foreach (ReferenceHub hub in Player.GetHubs())
            {
				if(hub.GetRole() == RoleType.Scp106 && hub.GetGodMode())
                {
					return false;
                }
            }
			return true;
        }
    }

	[HarmonyPatch(typeof(Scp079PlayerScript), nameof(Scp079PlayerScript.Start))]
	public static class Scp079ManaPatch
	{
		public static void Postfix(Scp079PlayerScript __instance)
		{
			foreach (Scp079PlayerScript.Ability079 ability in __instance.abilities)
			{
				switch (ability.label)
				{
					case "Camera Switch":
						ability.mana = Configs.scp079_cost_camera;
						break;
					case "Door Lock":
						ability.mana = Configs.scp079_cost_lock;
						break;
					case "Door Lock Start":
						ability.mana = Configs.scp079_cost_lock_start;
						break;
					case "Door Lock Minimum":
						ability.mana = Configs.scp079_cost_lock_minimum;
						break;
					case "Door Interaction DEFAULT":
						ability.mana = Configs.scp079_cost_door_default;
						break;
					case "Door Interaction CONT_LVL_1":
						ability.mana = Configs.scp079_cost_door_contlv1;
						break;
					case "Door Interaction CONT_LVL_2":
						ability.mana = Configs.scp079_cost_door_contlv2;
						break;
					case "Door Interaction CONT_LVL_3":
						ability.mana = Configs.scp079_cost_door_contlv3;
						break;
					case "Door Interaction ARMORY_LVL_1":
						ability.mana = Configs.scp079_cost_door_armlv1;
						break;
					case "Door Interaction ARMORY_LVL_2":
						ability.mana = Configs.scp079_cost_door_armlv2;
						break;
					case "Door Interaction ARMORY_LVL_3":
						ability.mana = Configs.scp079_cost_door_armlv3;
						break;
					case "Door Interaction EXIT_ACC":
						ability.mana = Configs.scp079_cost_door_exit;
						break;
					case "Door Interaction INCOM_ACC":
						ability.mana = Configs.scp079_cost_door_intercom;
						break;
					case "Door Interaction CHCKPOINT_ACC":
						ability.mana = Configs.scp079_cost_door_checkpoint;
						break;
					case "Room Lockdown":
						ability.mana = Configs.scp079_cost_lockdown;
						break;
					case "Tesla Gate Burst":
						ability.mana = Configs.scp079_cost_tesla;
						break;
					case "Elevator Teleport":
						ability.mana = Configs.scp079_cost_elevator_teleport;
						break;
					case "Elevator Use":
						ability.mana = Configs.scp079_cost_elevator_use;
						break;
					case "Speaker Start":
						ability.mana = Configs.scp079_cost_speaker_start;
						break;
					case "Speaker Update":
						ability.mana = Configs.scp079_cost_speaker_update;
						break;
				}
			}
		}
	}
}