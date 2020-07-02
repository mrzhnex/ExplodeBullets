using EXILED;

namespace ExplodeBullets
{
    public class MainSetting : Plugin
    {
        public override string getName => "ExplodeBullets";
        private SetEvent SetEvent;
        public override void OnEnable()
        {
            SetEvent = new SetEvent();
            Events.ShootEvent += SetEvent.OnShoot;
            Events.PlayerSpawnEvent += SetEvent.OnPlayerSpawn;
            Events.RemoteAdminCommandEvent += SetEvent.OnRemoteAdminCommand;
            Log.Info(getName + " on");
        }

        public override void OnDisable()
        {
            Events.ShootEvent -= SetEvent.OnShoot;
            Events.PlayerSpawnEvent -= SetEvent.OnPlayerSpawn;
            Events.RemoteAdminCommandEvent -= SetEvent.OnRemoteAdminCommand;
            Log.Info(getName + " off");
        }

        public override void OnReload() { }
    }
}