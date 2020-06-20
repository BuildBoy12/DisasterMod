using System;
using System.Collections.Generic;

namespace DisasterMod
{
	internal static class Configs
    {
		internal static bool Enabled;
		internal static bool AutoRestart;

		internal static float PocketDimRegen;
		internal static float PocketDeathRegen;

		internal static Dictionary<RoleType, List<ItemType>> start_items;

        //SMod Emulation
        internal static int auto_warhead_start;
        internal static bool auto_warhead_start_lock;

        //Balancing
        internal static bool stop_respawn_after_detonation;

        internal static bool scp106_reduce_grenade;
        internal static bool scp096_high_sensitivity;

		//SCP-079
		internal static float scp079_cost_camera;
		internal static float scp079_cost_lock;
		internal static float scp079_cost_lock_start;
		internal static float scp079_cost_lock_minimum;
		internal static float scp079_cost_door_default;
		internal static float scp079_cost_door_contlv1;
		internal static float scp079_cost_door_contlv2;
		internal static float scp079_cost_door_contlv3;
		internal static float scp079_cost_door_armlv1;
		internal static float scp079_cost_door_armlv2;
		internal static float scp079_cost_door_armlv3;
		internal static float scp079_cost_door_exit;
		internal static float scp079_cost_door_intercom;
		internal static float scp079_cost_door_checkpoint;
		internal static float scp079_cost_lockdown;
		internal static float scp079_cost_tesla;
		internal static float scp079_cost_elevator_use;
		internal static float scp079_cost_elevator_teleport;
		internal static float scp079_cost_speaker_start;
		internal static float scp079_cost_speaker_update;

		//Donator fun
		internal static List<string> HatUsers;
		internal static List<string> PetUsers;
		internal static List<string> ValidHats;
		internal static List<string> ValidPets;

		internal static void Reload()
		{
			Enabled = Plugin.Config.GetBool("di_Enabled", true);
			AutoRestart = Plugin.Config.GetBool("di_AutoRestart", false);
			PocketDimRegen = Plugin.Config.GetFloat("di_PocketRegen", 1F);
			PocketDeathRegen = Plugin.Config.GetFloat("di_PocketDeathRegen", 10F);

			start_items = new Dictionary<RoleType, List<ItemType>>
			{
				{ RoleType.ClassD, new List<ItemType>(Plugin.Config.GetStringList("dm_defaultitem_classd").ConvertAll((string x) => { return (ItemType)Enum.Parse(typeof(ItemType), x); })) },
				{ RoleType.Scientist, new List<ItemType>(Plugin.Config.GetStringList("dm_defaultitem_scientist").ConvertAll((string x) => { return (ItemType)Enum.Parse(typeof(ItemType), x); })) },
				{ RoleType.FacilityGuard, new List<ItemType>(Plugin.Config.GetStringList("dm_defaultitem_guard").ConvertAll((string x) => { return (ItemType)Enum.Parse(typeof(ItemType), x); })) },
				{ RoleType.NtfCadet, new List<ItemType>(Plugin.Config.GetStringList("dm_defaultitem_cadet").ConvertAll((string x) => { return (ItemType)Enum.Parse(typeof(ItemType), x); })) },
				{ RoleType.NtfLieutenant, new List<ItemType>(Plugin.Config.GetStringList("dm_defaultitem_lieutenant").ConvertAll((string x) => { return (ItemType)Enum.Parse(typeof(ItemType), x); })) },
				{ RoleType.NtfCommander, new List<ItemType>(Plugin.Config.GetStringList("dm_defaultitem_commander").ConvertAll((string x) => { return (ItemType)Enum.Parse(typeof(ItemType), x); })) },
				{ RoleType.NtfScientist, new List<ItemType>(Plugin.Config.GetStringList("dm_defaultitem_ntfscientist").ConvertAll((string x) => { return (ItemType)Enum.Parse(typeof(ItemType), x); })) },
				{ RoleType.ChaosInsurgency, new List<ItemType>(Plugin.Config.GetStringList("dm_defaultitem_ci").ConvertAll((string x) => { return (ItemType)Enum.Parse(typeof(ItemType), x); })) }
			};

			stop_respawn_after_detonation = Plugin.Config.GetBool("dm_stop_respawn_after_detonation", true);

			scp106_reduce_grenade = Plugin.Config.GetBool("dm_scp096_reduce_grenade", true);
			scp096_high_sensitivity = Plugin.Config.GetBool("dm_scp096_highsensitivity", true);

			scp079_cost_camera = Plugin.Config.GetFloat("dm_scp079_cost_camera", 2);
			scp079_cost_lock = Plugin.Config.GetFloat("dm_scp079_cost_lock", 10);
			scp079_cost_lock_start = Plugin.Config.GetFloat("dm_scp079_cost_lock_start", 5);
			scp079_cost_lock_minimum = Plugin.Config.GetFloat("dm_scp079_cost_lock_minimum", 10);
			scp079_cost_door_default = Plugin.Config.GetFloat("dm_scp079_cost_door_default", 3);
			scp079_cost_door_contlv1 = Plugin.Config.GetFloat("dm_scp079_cost_door_contlv1", 50);
			scp079_cost_door_contlv2 = Plugin.Config.GetFloat("dm_scp079_cost_door_contlv2", 80);
			scp079_cost_door_contlv3 = Plugin.Config.GetFloat("dm_scp079_cost_door_contlv3", 110);
			scp079_cost_door_armlv1 = Plugin.Config.GetFloat("dm_scp079_cost_door_armlv1", 50);
			scp079_cost_door_armlv2 = Plugin.Config.GetFloat("dm_scp079_cost_door_armlv2", 60);
			scp079_cost_door_armlv3 = Plugin.Config.GetFloat("dm_scp079_cost_door_armlv3", 70);
			scp079_cost_door_exit = Plugin.Config.GetFloat("dm_scp079_cost_door_exit", 50);
			scp079_cost_door_intercom = Plugin.Config.GetFloat("dm_scp079_cost_door_intercom", 25);
			scp079_cost_door_checkpoint = Plugin.Config.GetFloat("dm_scp079_cost_door_checkpoint", 10);
			scp079_cost_lockdown = Plugin.Config.GetFloat("dm_scp079_cost_lockdown", 60);
			scp079_cost_tesla = Plugin.Config.GetFloat("dm_scp079_cost_tesla", 60);
			scp079_cost_elevator_teleport = Plugin.Config.GetFloat("dm_scp079_cost_elevator_teleport", 20);
			scp079_cost_elevator_use = Plugin.Config.GetFloat("dm_scp079_cost_elevator_use", 10);
			scp079_cost_speaker_start = Plugin.Config.GetFloat("dm_scp079_cost_speaker_start", 5);
			scp079_cost_speaker_update = Plugin.Config.GetFloat("dm_scp079_cost_speaker_update", 0.5f);

			HatUsers = Plugin.Config.GetStringList("dm_hat_users");
			PetUsers = Plugin.Config.GetStringList("dm_pet_users");
			ValidHats = Plugin.Config.GetStringList("dm_hat_types");
			ValidPets = Plugin.Config.GetStringList("dm_pet_types");
		}
	}
}
