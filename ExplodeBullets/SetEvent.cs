using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Grenades;
using Mirror;
using System.Linq;
using UnityEngine;

namespace ExplodeBullets
{
    public class SetEvent
    {
        internal void OnRoundStarted()
        {
            Global.FemurBreaker = GameObject.FindWithTag("FemurBreaker");
        }

        internal void OnSendingRemoteAdminCommand(SendingRemoteAdminCommandEventArgs ev)
        {
            if (ev.Name != "eb")
            {
                return;
            }
            ev.IsAllowed = false;
            if (ev.Arguments.Count != 2)
            {
                ev.Sender.RemoteAdminMessage("Out of args. Usage: " + GetUsage());
                return;
            }

            Player player = Player.Get(ev.Arguments[0]);
            if (player == null)
            {
                ev.Sender.RemoteAdminMessage("Player not found", false);
                return;
            }
            switch (ev.Arguments[1])
            {
                case "add":
                    if (player.GameObject.GetComponent<ExplodeMaster>() == null)
                    {
                        player.GameObject.AddComponent<ExplodeMaster>();
                        ev.Sender.RemoteAdminMessage("Add ExplodeMaster to " + player.Nickname);
                    }
                    else
                    {
                        ev.Sender.RemoteAdminMessage("Is already ExplodeMaster " + player.Nickname);
                    }
                    return;
                case "remove":
                    if (player.GameObject.GetComponent<ExplodeMaster>() == null)
                    {
                        ev.Sender.RemoteAdminMessage("Is not ExplodeMaster " + player.Nickname);
                    }
                    else
                    {
                        Object.Destroy(player.GameObject.GetComponent<ExplodeMaster>());
                        ev.Sender.RemoteAdminMessage("Remove ExplodeMaster from" + player.Nickname);
                    }
                    return;
                default:
                    ev.Sender.RemoteAdminMessage("Out of args. Usage: " + GetUsage());
                    return;
            }
        }

        internal void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player.GameObject.GetComponent<ExplodeMaster>())
            {
                Object.Destroy(ev.Player.GameObject.GetComponent<ExplodeMaster>());
            }
        }

        internal void OnShot(ShotEventArgs ev)
        {
            if (ev.Shooter.Inventory.curItem == ItemType.GunE11SR && ev.Shooter.GameObject.GetComponent<ExplodeMaster>())
            {
                GameObject gameobj = ev.Shooter.GameObject;
                if (Physics.Raycast(gameobj.transform.position, ev.Shooter.PlayerCamera.forward, out RaycastHit hit, 800f))
                {
                    if (hit.distance > Global.SaveDistance)
                    {
                        CustomThrowG(hit.point, ev.Shooter);
                    }
                }
            }
        }

        private string GetUsage()
        {
            return "eb <id>/<nickname> <add>/<remove>";
        }

        public void CustomThrowG(Vector3 position, Player player)
        {
            if (player.GameObject.GetComponent<GrenadeManager>().availableGrenades.FirstOrDefault() == default)
            {
                return;
            }
            Grenade grenade = Object.Instantiate(player.GameObject.GetComponent<GrenadeManager>().availableGrenades.FirstOrDefault().grenadeInstance).GetComponent<Grenade>();
            grenade.gameObject.transform.position = position;
            NetworkServer.Spawn(grenade.gameObject);
            
            if (Global.FemurBreaker != null)
            {
                HurtDelayComponent hurtDelayComponent = Global.FemurBreaker.AddComponent<HurtDelayComponent>();
                hurtDelayComponent.Position = position;
                hurtDelayComponent.Player = player;
            }
        }
    }
}