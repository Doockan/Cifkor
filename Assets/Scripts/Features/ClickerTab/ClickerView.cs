using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Features.ClickerTab
{
    public class ClickerView : MonoBehaviour
    {
        [SerializeField] private UIDocument _uIDocument;

        public VisualElement Root => _uIDocument.rootVisualElement.Q<VisualElement>("ClickerTab");
        public Button ClickerButton => Root.Q<Button>("ClickerButton");
        public Label CurrencyLabel => Root.Q<Label>("CurrencyLabel");
        public Label EnergyLabel => Root.Q<Label>("EnergyLabel");
        public VisualElement VfxContainer => Root.Q<VisualElement>("ClickerVfxContainer");

        public void PlayCurrencyFlyVfx()
        {
            var flyLabel = new Label("+1");
            flyLabel.AddToClassList("currency-fly");
            VfxContainer.Add(flyLabel);

            flyLabel.schedule.Execute(() => { flyLabel.AddToClassList("fly"); })
                .StartingIn(50);

            flyLabel.schedule.Execute(() => { VfxContainer.Remove(flyLabel); })
                .StartingIn(800);
        }

        public void PlayParticleVfx(int count = 10)
        {
            var rnd = new System.Random();
            float radius = 50f;
            var vfxContainer = VfxContainer;

            for (int i = 0; i < count; i++)
            {
                var particle = new VisualElement();
                particle.AddToClassList("clicker-particle");

                float angle = rnd.Next(0, 360) * Mathf.Deg2Rad;
                float x = Mathf.Cos(angle) * radius;
                float y = Mathf.Sin(angle) * radius;

                float centerX = vfxContainer.resolvedStyle.width / 2f;
                float centerY = vfxContainer.resolvedStyle.height / 2f;

                particle.style.left = centerX + x;
                particle.style.top = centerY + y;

                var colors = new[] { "#ffd700", "#ffe066", "#fffacd", "#fff" };
                string color = colors[rnd.Next(colors.Length)];
                particle.style.backgroundColor =
                    new StyleColor(ColorUtility.TryParseHtmlString(color, out var c) ? c : Color.white);

                vfxContainer.Add(particle);

                float flyAngle = angle + rnd.Next(-40, 40) * Mathf.Deg2Rad;
                float flyDistance = rnd.Next(80, 100);
                float dx = Mathf.Cos(flyAngle) * flyDistance;
                float dy = Mathf.Sin(flyAngle) * flyDistance;

                particle.schedule.Execute(() =>
                {
                    particle.style.left = centerX + x + dx;
                    particle.style.top = centerY + y + dy;
                    particle.AddToClassList("fly");
                }).StartingIn(50);

                particle.schedule.Execute(() => { vfxContainer.Remove(particle); }).StartingIn(850);
            }
        }
    }
}