namespace SpaceLogistic.Core.Model.Celestials
{
    using System.Collections.Generic;
    using System.Linq;

    public static class CelestialExtensions
    {
        public static IEnumerable<CelestialSystem> GetSelfAndAncestors(this CelestialSystem celestialSystem)
        {
            return Enumerable.Repeat(celestialSystem, 1).Concat(celestialSystem.GetAncestors());
        }

        public static IEnumerable<CelestialSystem> GetSelfAndDescendants(this CelestialSystem celestialSystem)
        {
            return Enumerable.Repeat(celestialSystem, 1).Concat(celestialSystem.GetDescendants());
        }

        public static IEnumerable<CelestialSystem> GetAncestors(this CelestialSystem celestialSystem)
        {
            if (celestialSystem.Parent == null)
            {
                return Enumerable.Empty<CelestialSystem>();
            }

            return Enumerable.Repeat(celestialSystem.Parent, 1).Concat(celestialSystem.Parent.GetAncestors());
        }

        public static IEnumerable<CelestialSystem> GetDescendants(this CelestialSystem celestialSystem)
        {
            return celestialSystem.Children.Concat(celestialSystem.Children.SelectMany(c => c.GetDescendants()));
        }

        public static CelestialSystem GetDescendantWithName(this CelestialSystem celestialSystem, string name)
        {
            return celestialSystem.GetDescendants().Single(c => c.Name == name);
        }
    }
}
