using Mirror;
using EXILED;
using EXILED.Extensions;
using MEC;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RemoteAdmin;
using Telepathy;

namespace DisasterMod
{
    public class Methods
    {
        public static List<ReferenceHub> DoorBreakers = new List<ReferenceHub>();
        public static List<ReferenceHub> AdminGun = new List<ReferenceHub>();
        public static List<ReferenceHub> HatUsers = new List<ReferenceHub>();
        public static List<ReferenceHub> PetUsers = new List<ReferenceHub>();
        public static List<Pickup> ActiveHats = new List<Pickup>();
        public static void SpawnItems()
        {
            Vector3 pos = new Vector3(37.5f, 989f, -33.6f);
            PlayerManager.localPlayer.GetComponent<Inventory>().SetPickup(ItemType.GunE11SR, 100, pos, Quaternion.Euler(Vector3.zero), 0, 0, 0);

            Vector3 pos2 = new Vector3(42.75f, 990f, -37.5f);
            PlayerManager.localPlayer.GetComponent<Inventory>().SetPickup(ItemType.WeaponManagerTablet, 100, pos2, Quaternion.Euler(Vector3.zero), 0, 0, 0);
        }

        public static IEnumerator<float> Hat(ReferenceHub hub)
        {
            Pickup pickup = PlayerManager.localPlayer.GetComponent<Inventory>().SetPickup(ItemType.SCP268, 100, hub.PlayerCameraReference.transform.position, hub.gameObject.transform.localRotation, 0, 0, 0);
            ActiveHats.Add(pickup);
            while (HatUsers.Contains(hub))
            {        
                pickup.transform.position = hub.PlayerCameraReference.transform.position;
                pickup.transform.rotation = hub.gameObject.transform.localRotation;
                pickup.transform.Rotate(270, 0, 0);
                yield return Timing.WaitForOneFrame;
            }
            pickup.Delete();
            ActiveHats.Remove(pickup);
        }

        public static IEnumerator<float> Pet(ReferenceHub hub)
        {
            var dummy = SpawnDummyModel(hub, hub.transform.position, hub.gameObject.transform.localRotation, RoleType.Scp173, .3f, .3f, .3f);
            NetworkServer.Spawn(dummy);
            while (PetUsers.Contains(hub))
            {
                dummy.transform.position = new Vector3(hub.transform.position.x, hub.transform.position.y, hub.transform.position.z + 1f);
                yield return Timing.WaitForOneFrame;
            }
        }

        public static GameObject SpawnDummyModel(ReferenceHub hub, Vector3 position, Quaternion rotation, RoleType role, float x, float y, float z)
        {
            GameObject gameObject = Object.Instantiate(NetworkManager.singleton.spawnPrefabs.FirstOrDefault((GameObject p) => p.gameObject.name == "Player"));
            CharacterClassManager component = gameObject.GetComponent<CharacterClassManager>();
            component.CurClass = role;
            component.RefreshPlyModel(role);
            gameObject.GetComponent<NicknameSync>().Network_myNickSync = $"{hub.GetNickname()}'s Pet";
            gameObject.GetComponent<QueryProcessor>().PlayerId = 9999;
            gameObject.GetComponent<QueryProcessor>().NetworkPlayerId = 9999;
            gameObject.transform.localScale = new Vector3(x, y, z);
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;
            return gameObject;
        }

        public static IEnumerator<float> RoundEndRestart()
        {
            yield return Timing.WaitForSeconds(7.5f);
            PlayerStats.StaticChangeLevel();
        }
    }
}
