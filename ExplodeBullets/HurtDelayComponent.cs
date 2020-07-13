using EXILED.Extensions;
using UnityEngine;

namespace ExplodeBullets
{
    public class HurtDelayComponent : MonoBehaviour
    {
        private float Timer = 0.0f;
        public Vector3 Position;
        public ReferenceHub ReferenceHub;

        public void Update()
        {
            Timer += Time.deltaTime;

            if (Timer > 0.1f)
            {
                foreach (ReferenceHub referenceHub in Player.GetHubs())
                {
                    if (referenceHub.GetRole() == RoleType.Spectator || referenceHub.GetRole() == RoleType.Scp079)
                    {
                        continue;
                    }
                    if (Vector3.Distance(referenceHub.GetPosition(), Position) < Global.HurtDistance)
                    {
                        referenceHub.playerStats.HurtPlayer(new PlayerStats.HitInfo(GetDamage(Vector3.Distance(referenceHub.GetPosition(), Position)), ReferenceHub.nicknameSync.Network_myNickSync, DamageTypes.Grenade, ReferenceHub.GetPlayerId()), referenceHub.gameObject);
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