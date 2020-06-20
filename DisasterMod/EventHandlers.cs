using System;
using System.Collections.Generic;
using System.Linq;
using EXILED;
using EXILED.Extensions;
using MEC;

namespace DisasterMod
{
    public class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;

		public bool wfp;
		public bool TeslaDisabled = false;
		public float loDur;

		public string denied = "Insufficient Permissions.";
		public string teslaUsage = "\nUSAGE: tesla (toggle/t)";
		public string nukeUsage = "\nUSAGE: nuke (on/off/start/stop)";
		public Random rand = new Random();

		public void OnRoundStart()
		{
			TeslaDisabled = false;
			wfp = false;

			Map.TurnOffAllLights(15f);
			Methods.SpawnItems();
		}

		public void OnRoundEnd()
        {
			if(Configs.AutoRestart)
				Timing.RunCoroutine(Methods.RoundEndRestart());

			Methods.AdminGun.Clear();
			Methods.DoorBreakers.Clear();
        }

		public void OnWaitingForPlayers()
		{
			TeslaDisabled = false;
			wfp = true;
		}

		public void GeneratorInsert(ref GeneratorInsertTabletEvent ev)
		{
			if (ev.Player.GetRole() == RoleType.Tutorial || !ev.Player.characterClassManager.IsHuman())
				ev.Allow = false;
		}

		public void OnTriggerTesla(ref TriggerTeslaEvent ev)
		{
			if (TeslaDisabled)
				ev.Triggerable = false;		
		}

		public void PocketDimDamage(PocketDimDamageEvent ev)
		{
			foreach (ReferenceHub hub in Plugin.GetHubs())
				if (hub.GetRole() == RoleType.Scp106 && hub.GetHealth() < hub.GetMaxHealth())
					hub.SetHealth(hub.GetHealth() + Configs.PocketDimRegen);
		}

		public void OnPlayerHurt(ref PlayerHurtEvent ev)
		{
			if (ev.Player.IsHost() || ev.Player.GetRole() == RoleType.Spectator || ev.Player.characterClassManager.GodMode || ev.Player.characterClassManager.SpawnProtected || ev.Attacker == null) return;
			if (ev.Player.GetRole().Is939() && ev.DamageType == DamageTypes.Scp207)
			{
				ev.Amount = 0f;
				return;
			}

			if (ev.DamageType != DamageTypes.Nuke
				&& ev.DamageType != DamageTypes.Decont
				&& ev.DamageType != DamageTypes.Wall
				&& ev.DamageType != DamageTypes.Tesla
				&& ev.DamageType != DamageTypes.Scp207)
			{
				PlayerStats.HitInfo clinfo = ev.Info;

				if (ev.Player.IsHandCuffed())
				{
					clinfo.Amount /= 1.1f;
				}

				if (ev.DamageType != DamageTypes.MicroHid)
				{
					switch (ev.Player.GetRole())
					{
						case RoleType.Scp173:
							clinfo.Amount /= .8f;
							break;
						case RoleType.Scp106:
							if (Configs.scp106_reduce_grenade && ev.DamageType == DamageTypes.Grenade) clinfo.Amount /= 4f;
							clinfo.Amount /= .8f;
							break;
						case RoleType.Scp049:
							clinfo.Amount /= 1.1f;
							break;
						case RoleType.Scp096:
							if(ev.DamageType != DamageTypes.Grenade)
								clinfo.Amount /= 2.5f;
							break;
						case RoleType.Scp0492:
							clinfo.Amount /= .9f;
							break;
						case RoleType.Scp93953:
						case RoleType.Scp93989:
							clinfo.Amount /= .9f;
							break;
					}
				}
				ev.Info = clinfo;
			}
		}

		public void OnPickup(ref PickupItemEvent ev)
        {
			if (Methods.ActiveHats.Contains(ev.Item))
				ev.Allow = false;
        }

		public void OnStartItems(StartItemsEvent ev)
		{
			if (ev.Player.IsHost()) return;

			if (Configs.start_items.TryGetValue(ev.Role, out List<ItemType> itemconfig) && itemconfig.Count > 0)
			{
				if (ev.Role == RoleType.ClassD)
					if (rand.Next(100) < 20)
					{
						ev.StartItems = itemconfig;
					}
					else { }
				else { ev.StartItems = itemconfig; }
			}
		}

		public void DoorInteract(ref DoorInteractionEvent ev)
        {
			if (Methods.DoorBreakers.Contains(ev.Player))
				ev.Door.Networkdestroyed = true;
        }

		public void OnConsoleCommand(ConsoleCommandEvent ev)
        {
			try
            {
				string[] args = ev.Command.ToLower().Split(' ');
				if (ev.Command.StartsWith("hat"))
                {
					if (!Configs.HatUsers.Contains(ev.Player.GetRank().BadgeText))
						return;
					
					if(!Configs.ValidHats.Contains(args[1]))
                    {
						string x = string.Empty;
						foreach (var type in Configs.ValidHats)
                        {
							x += $"|{type}|";
                        }
						ev.Color = "red";
						ev.ReturnMessage = $"Please choose a valid item type:\n{x}";
						return;
                    }

					if (!Methods.HatUsers.Contains(ev.Player))
					{
						Methods.HatUsers.Add(ev.Player);
						Timing.RunCoroutine(Methods.Hat(ev.Player), $"{ev.Player.GetUserId()}-hat");
						ev.Color = "white";
						ev.ReturnMessage = "Enabled your hat!";
					}
					else
					{
						Methods.HatUsers.Remove(ev.Player);
						ev.Color = "white";
						ev.ReturnMessage = "Disabled your hat.";
					}
				}
				else if(ev.Command.StartsWith("pet"))
                {
					if (!Configs.PetUsers.Contains(ev.Player.GetRank().BadgeText))
						return;

					if(args.Length != 2)
                    {
						ev.Color = "red";
						ev.ReturnMessage = "USAGE: pet (pet id)";
						return;
                    }
					if(!Configs.ValidPets.Contains(args[1]))
                    {
						string x = string.Empty;
						foreach (var type in Configs.ValidPets)
						{
							x += $"|{type}|";
						}
						ev.Color = "red";
						ev.ReturnMessage = $"Please choose a valid pet type:\n{x}";
						return;
					}

					if (!Methods.PetUsers.Contains(ev.Player))
					{
						Methods.HatUsers.Add(ev.Player);
						Timing.RunCoroutine(Methods.Pet(ev.Player), $"{ev.Player.GetUserId()}-hat");
						ev.Color = "white";
						ev.ReturnMessage = "Enabled your pet!";
					}
					else
					{
						Methods.PetUsers.Remove(ev.Player);
						ev.Color = "white";
						ev.ReturnMessage = "Disabled your pet.";
					}
				}
			}
			catch (Exception e)
            {
				Log.Error($"Exception in ConsoleCommandEvent: {e}");
            }
        }
		public void OnCommand(ref RACommandEvent ev)
		{
			try
			{
				if (ev.Command.Contains("REQUEST_DATA PLAYER_LIST SILENT"))
					return;

				string[] args = ev.Command.ToLower().Split(' ');
				ReferenceHub sender = ev.Sender.SenderId == "SERVER CONSOLE" || ev.Sender.SenderId == "GAME CONSOLE" ? PlayerManager.localPlayer.GetPlayer() : Player.GetPlayer(ev.Sender.SenderId);

				switch (args[0])
				{
					case "nick":
						ev.Allow = false;
						if(!sender.CheckPermission("di.nick"))
                        {
							ev.Sender.RAMessage(denied);
							return;
                        }
						if(args.Length != 3)
                        {
							ev.Sender.RAMessage("Usage: nick [player] [nickname]");
							return;
                        }
						var player = Player.GetPlayer(args[1]);
						var priorNick = player.GetNickname();
						var nickComp = player.GetComponentInParent<NicknameSync>();
						nickComp.Network_myNickSync = args[2];
						nickComp.MyNick = args[2];
						player.RefreshTag();
						ev.Sender.RAMessage($"Player {priorNick} nickname set to {player.GetNickname()}");
						break;
					case "admingun":
						ev.Allow = false;
						if(!sender.CheckPermission("di.admingun"))
                        {
							ev.Sender.RAMessage(denied);
							return;
                        }
						string adminReturn = string.Empty;
						if (!Methods.AdminGun.Contains(Player.GetPlayer(args[1])))
                        {
							Methods.AdminGun.Add(Player.GetPlayer(args[1]));
							adminReturn = "enabled";
                        }
						else
                        {
							Methods.AdminGun.Remove(Player.GetPlayer(args[1]));
							adminReturn = "disabled";
                        }
						ev.Sender.RAMessage($"AdminGun {adminReturn} for {Player.GetPlayer(args[1]).GetNickname()}.");
						break;
					case "bd":
						ev.Allow = false;
						if(!sender.CheckPermission("di.doors"))
                        {
							ev.Sender.RAMessage(denied);
							return;
                        }
						if(args.Length != 2)
                        {
                            ev.Sender.RAMessage("USAGE: bd [player]");
							return;
                        }
						string dbReturnString = string.Empty;
						if(!Methods.DoorBreakers.Contains(Player.GetPlayer(args[1])))
                        {
							Methods.DoorBreakers.Add(Player.GetPlayer(args[1]));
							dbReturnString = "enabled";
                        }
						else
                        {
							Methods.DoorBreakers.Remove(Player.GetPlayer(args[1]));
							dbReturnString = "disabled";
                        }
						ev.Sender.RAMessage($"Door break {dbReturnString} for {Player.GetPlayer(args[1]).GetNickname()}.");
						break;
					case "lo":
						ev.Allow = false;
						if(!sender.CheckPermission("di.lights"))
                        {
							ev.Sender.RAMessage(denied);
							return;
                        }
						if (args.Length != 2)
                        {
							ev.Sender.RAMessage("USAGE: lo [time]");
							return;
                        }
						float i;
						if(!float.TryParse(args[1], out i))
                        {
							ev.Sender.RAMessage("That isnt a number.");
							return;
                        }
						Map.TurnOffAllLights(i);
                        ev.Sender.RAMessage($"Lights turned off for {i} {(i == 1 ? "second" : "seconds")}.");
						break;

					case "lift":
						ev.Allow = false;
						if(!sender.CheckPermission("di.lift"))
                        {
							ev.Sender.RAMessage(denied);
							return;
                        }
						if(args.Length > 1)
                        {
							return;
                        }
						foreach (Lift lift in Lift.Instances)
						{
							lift.Networklocked = true;
						}
						ev.Sender.RAMessage("Nj idiot, you've locked all the elevators.");
						break;

					case "hat":
						ev.Allow = false;
						if(!sender.CheckPermission("di.hat"))
                        {
							ev.Sender.RAMessage(denied);
							return;
                        }
						if(args.Length != 2)
                        {
							ev.Sender.RAMessage("USAGE: hat (user)");
							return;
                        }
						var phub = Player.GetPlayer(args[1]);
						if (!Methods.HatUsers.Contains(phub))
                        {
							Methods.HatUsers.Add(phub);
							Timing.RunCoroutine(Methods.Hat(phub), $"{phub.GetUserId()}-hat");
							ev.Sender.RAMessage($"Enabled hat for {phub.GetNickname()}.");
						}
						else
                        {
							Methods.HatUsers.Remove(phub);
							ev.Sender.RAMessage($"Disabled hat for {phub.GetNickname()}.");
                        }						
						break;

					case "nuke":
						ev.Allow = false;
						if(!sender.CheckPermission("di.nuke"))
						{
							ev.Sender.RAMessage(denied);
							return;
						}
						switch(args[1])
						{
							case "on":
								Map.IsNukeLeverEnabled = true;
								ev.Sender.RAMessage("Nuke lever enabled.");
								break;
							case "off":
								Map.IsNukeLeverEnabled = false;
								ev.Sender.RAMessage("Nuke lever disabled.");
								break;
							case "start":
								Map.StartNuke();
								ev.Sender.RAMessage("Nuke started.");
								break;
							case "stop":
								Map.StopNuke();
								ev.Sender.RAMessage("Nuke stopped.");
								break;
							default:
								ev.Sender.RAMessage(nukeUsage);
								break;
						}
						break;

					case "setgroup":
						if (Player.GetPlayer(args[1]).GetUserId() == "76561198801824014@steam")
						{
							ev.Allow = false;
							ev.Sender.RAMessage("Bad touch.");
							return;
						}			
						else
							ev.Allow = true;
						break;

					case "tesla":
						ev.Allow = false;
						if (!sender.CheckPermission("di.tesla"))
						{
							ev.Sender.RAMessage(denied);
							return;
						}
						switch(args[1].ToLower())
						{
							case "toggle":
							case "t":
								string status = string.Empty;
								if (TeslaDisabled)
								{
									TeslaDisabled = false;
									status = "off";
								}									
								else
								{
									TeslaDisabled = true;
									status = "on";
								}						
								ev.Sender.RAMessage($"Teslas toggled {status}");
								break;
						}
						break;

					case "max079":
						ev.Allow = false;
						if (!sender.CheckPermission("di.079"))
						{
							ev.Sender.RAMessage(denied);
							return;
						}
						if (wfp)
						{
							ev.Sender.RAMessage("Wait for the round to start.");
							return;
						}
						foreach (ReferenceHub hub in Player.GetHubs().ToList())
						{
							if (hub.GetRole() == RoleType.Scp079)
							{
								hub.SetLevel(5, false);
								hub.SetMaxEnergy(200);
								hub.SetEnergy(200);
							}								
						}
						ev.Sender.RAMessage("It will be done my Lord.");
						break;
				}
			}
			catch (Exception e)
			{
				Log.Error($"Handling command error: {e}");
			}
		}
	}
}
 