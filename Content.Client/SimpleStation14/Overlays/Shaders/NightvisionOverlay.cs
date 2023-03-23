using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Enums;
using Robust.Shared.Prototypes;
using Content.Shared.SimpleStation14.Overlays;

namespace Content.Client.SimpleStation14.Overlays
{
    public sealed class NightvisionOverlay : Overlay
    {
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
        [Dependency] private readonly IPlayerManager _playerManager = default!;
        [Dependency] IEntityManager _entityManager = default!;


        public override bool RequestScreenTexture => true;
        public override OverlaySpace Space => OverlaySpace.WorldSpace;
        private readonly ShaderInstance _shader;

        public NightvisionOverlay()
        {
            IoCManager.InjectDependencies(this);
            _shader = _prototypeManager.Index<ShaderPrototype>("Nightvision").InstanceUnique();
        }

        protected override void Draw(in OverlayDrawArgs args)
        {
            if (ScreenTexture == null) return;
            if (_playerManager.LocalPlayer?.ControlledEntity is not { Valid: true } player) return;
            if (!_entityManager.TryGetComponent<NightvisionComponent>(player, out var component)) return;

            _shader?.SetParameter("SCREEN_TEXTURE", ScreenTexture);

            _shader?.SetParameter("tint", component.Tint);
            _shader?.SetParameter("luminance_threshold", component.Strength);
            _shader?.SetParameter("noise_amount", component.Noise);


            var worldHandle = args.WorldHandle;
            var viewport = args.WorldBounds;
            worldHandle.SetTransform(Matrix3.Identity);
            worldHandle.UseShader(_shader);
            worldHandle.DrawRect(viewport, Color.White);
            worldHandle.UseShader(null);
        }
    }
}
