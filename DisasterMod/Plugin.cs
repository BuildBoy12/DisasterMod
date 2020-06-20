using EXILED;
using Harmony;

namespace DisasterMod
{
    public class Plugin : EXILED.Plugin
	{
		public EventHandlers EventHandlers;

		HarmonyInstance instance = HarmonyInstance.Create($"com.build.dicore.{PatchCounter}");
		public static int PatchCounter;

		public override void OnEnable()
		{
			Configs.Reload();
			if (!Configs.Enabled)
				return;
				
			EventHandlers = new EventHandlers(this);
			instance.PatchAll();

			Events.RemoteAdminCommandEvent += EventHandlers.OnCommand;
			Events.TriggerTeslaEvent += EventHandlers.OnTriggerTesla;
			Events.RoundStartEvent += EventHandlers.OnRoundStart;
			Events.WaitingForPlayersEvent += EventHandlers.OnWaitingForPlayers;
			Events.PocketDimDamageEvent += EventHandlers.PocketDimDamage;
			Events.GeneratorInsertedEvent += EventHandlers.GeneratorInsert;
			Events.StartItemsEvent += EventHandlers.OnStartItems;
			Events.PlayerHurtEvent += EventHandlers.OnPlayerHurt;
			Events.ConsoleCommandEvent += EventHandlers.OnConsoleCommand;
			Events.RoundEndEvent += EventHandlers.OnRoundEnd;
			Events.PickupItemEvent += EventHandlers.OnPickup;
			Events.DoorInteractEvent += EventHandlers.DoorInteract;

			Log.Info($"DI Core Loaded");
		}
		
		public override void OnDisable()
		{
			Events.RemoteAdminCommandEvent -= EventHandlers.OnCommand;
			Events.TriggerTeslaEvent -= EventHandlers.OnTriggerTesla;
			Events.RoundStartEvent -= EventHandlers.OnRoundStart;
			Events.WaitingForPlayersEvent -= EventHandlers.OnWaitingForPlayers;
			Events.PocketDimDamageEvent -= EventHandlers.PocketDimDamage;
			Events.GeneratorInsertedEvent -= EventHandlers.GeneratorInsert;
			Events.StartItemsEvent -= EventHandlers.OnStartItems;
			Events.PlayerHurtEvent -= EventHandlers.OnPlayerHurt;
			Events.ConsoleCommandEvent -= EventHandlers.OnConsoleCommand;
			Events.RoundEndEvent -= EventHandlers.OnRoundEnd;
			Events.PickupItemEvent -= EventHandlers.OnPickup;
			Events.DoorInteractEvent -= EventHandlers.DoorInteract;

			EventHandlers = null;
			instance.UnpatchAll();
		}

		public override void OnReload()
		{
			
		}

		public override string getName => "DisasterMod";		
	}
}