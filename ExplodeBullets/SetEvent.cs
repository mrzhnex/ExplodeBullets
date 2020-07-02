using EXILED;
using EXILED.Extensions;
using Grenades;
using Mirror;
using System.Linq;
using UnityEngine;

namespace ExplodeBullets
{
    public class SetEvent
    {
        public void OnShoot(ref ShootEvent ev)
        {
            if (ev.Shooter.inventory.curItem == ItemType.GunE11SR && ev.Shooter.gameObject.GetComponent<ExplodeMaster>())
            {
                GameObject gameobj = ev.Shooter.gameObject;
                if (Physics.Raycast(gameobj.transform.position, gameobj.GetComponent<Scp049PlayerScript>().plyCam.transform.forward, out RaycastHit hit, 800f))
                {
                    if (hit.distance > Global.save_distance)
                    {
                        CustomThrowG(hit.point, gameobj);
                    }
                }
            }
        }

        private string GetUsage()
        {
            return "eb <id>/<nickname> <add>/<remove>";
        }

        public void OnRemoteAdminCommand(ref RACommandEvent ev)
        {
            string[] args = ev.Command.Split(' ');
            if (args.Length > 0 && args[0] != "eb")
            {
                return;
            }
            if (args.Length != 3)
            {
                ev.Sender.RAMessage("Out of args. Usage: " + GetUsage());
                return;
            }

            ReferenceHub playerHub = Player.GetPlayer(args[1]);
            if (playerHub == null)
            {
                ev.Sender.RAMessage("Player not found", false);
                return;
            }
            switch (args[2])
            {
                case "add":
                    if (playerHub.gameObject.GetComponent<ExplodeMaster>() == null)
                    {
                        playerHub.gameObject.AddComponent<ExplodeMaster>();
                        ev.Sender.RAMessage("Add ExplodeMaster to " + playerHub.nicknameSync.Network_myNickSync);
                        return;
                    }
                    else
                    {
                        ev.Sender.RAMessage("Is already ExplodeMaster " + playerHub.nicknameSync.Network_myNickSync);
                        return;
                    }
                case "remove":
                    if (playerHub.gameObject.GetComponent<ExplodeMaster>() == null)
                    {
                        ev.Sender.RAMessage("Is not ExplodeMaster " + playerHub.nicknameSync.Network_myNickSync);
                        return;
                    }
                    else
                    {
                        Object.Destroy(playerHub.gameObject.GetComponent<ExplodeMaster>());
                        ev.Sender.RAMessage("Remove ExplodeMaster from" + playerHub.nicknameSync.Network_myNickSync);
                        return;
                    }
            }

        }

        public void OnPlayerSpawn(PlayerSpawnEvent ev)
        {
            if (ev.Player.gameObject.GetComponent<ExplodeMaster>())
            {
                Object.Destroy(ev.Player.gameObject.GetComponent<ExplodeMaster>());
            }
        }

        public void CustomThrowG(Vector3 position, GameObject gameObject)
        {
            if (gameObject.GetComponent<GrenadeManager>().availableGrenades.FirstOrDefault() == default)
            {
                return;
            }
            Grenade grenade = Object.Instantiate(gameObject.GetComponent<GrenadeManager>().availableGrenades.FirstOrDefault().grenadeInstance).GetComponent<Grenade>();
            grenade.gameObject.transform.position = position;
            NetworkServer.Spawn(grenade.gameObject);
            gameObject.AddComponent<HurtDelayComponent>();
            gameObject.GetComponents<HurtDelayComponent>().Last().Position = position;
        }
    }
}