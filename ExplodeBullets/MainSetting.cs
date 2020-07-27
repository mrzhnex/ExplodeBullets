using Exiled.API.Features;

namespace ExplodeBullets
{
    public class MainSetting : Plugin<Config>
    {
        public override string Name => nameof(ExplodeBullets);
        public SetEvent SetEvent { get; set; }
        public override void OnEnabled()
        {
            SetEvent = new SetEvent();
            Exiled.Events.Handlers.Player.Shooting += SetEvent.OnShooting;
            Exiled.Events.Handlers.Player.ChangingRole += SetEvent.OnChangingRole;
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += SetEvent.OnSendingRemoteAdminCommand;
            Exiled.Events.Handlers.Server.RoundStarted += SetEvent.OnRoundStarted;
            Log.Info(Name + " on");
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Shooting -= SetEvent.OnShooting;
            Exiled.Events.Handlers.Player.ChangingRole -= SetEvent.OnChangingRole;
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= SetEvent.OnSendingRemoteAdminCommand;
            Exiled.Events.Handlers.Server.RoundStarted -= SetEvent.OnRoundStarted;
            Log.Info(Name + " off");
        }
    }
}