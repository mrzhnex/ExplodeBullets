using Exiled.API.Features;
using UnityEngine;

namespace ExplodeBullets
{
    public class HurtDelayComponent : MonoBehaviour
    {
        private float Timer = 0.0f;
        public Vector3 Position;
        public Player Player;

        public void Update()
        {
            Timer += Time.deltaTime;

            if (Timer > 0.1f)
            {
                foreach (Player player in Player.List)
                {
                    if (player.Role == RoleType.Spectator || player.Role == RoleType.Scp079)
                    {
                        continue;
                    }
                    if (Vector3.Distance(player.Position, Position) < Global.HurtDistance)
                    {
                        player.ReferenceHub.playerStats.HurtPlayer(new PlayerStats.HitInfo(GetDamage(Vector3.Distance(player.Position, Position)), Player.Nickname, DamageTypes.Grenade, Player.Id), player.GameObject);
                    }
                }
                foreach (Door door in Map.Doors)
                {
                    if (door.DoorName.ToLower().Contains("gate") || door.DoorName.ToLower().Contains("079") || door.DoorName.ToLower().Contains("914") || door.DoorName.ToLower().Contains("372"))
                    {
                        continue;
                    }
                    if (door.Networkdestroyed)
                    {
                        continue;
                    }
                    if (Vector3.Distance(Position, door.gameObject.transform.position) > Global.HurtDistance)
                    {
                        continue;
                    }
                    door.DestroyDoor(true);
                }
                Destroy(this);
            }
        }

        private float GetDamage(float distance)
        {
            return (Global.HurtDistance - distance) * 25 + Random.Range(0, 10);
        }
    }
}