namespace SpaceLogistic.WpfView.ViewModel
{
    using System.Collections.Generic;

    using SpaceLogistic.Core.Model.ShipRoutes;
    using SpaceLogistic.WpfView.View.SelectionBox;

    public sealed class RefuelBehaviorOption : IOption
    {
        public static IReadOnlyCollection<RefuelBehaviorOption> All { get; } = new[]
            {
                new RefuelBehaviorOption(RefuelBehavior.MinRequired, "Auto Refuel"),
                new RefuelBehaviorOption(RefuelBehavior.Full, "Full Refuel"),
                new RefuelBehaviorOption(RefuelBehavior.NoRefuel, "No Refuel"),
            };

        public RefuelBehaviorOption(RefuelBehavior value, string name)
        {
            this.Name = name;
            this.Value = value;
        }

        public string Name { get; }

        public RefuelBehavior Value { get; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
