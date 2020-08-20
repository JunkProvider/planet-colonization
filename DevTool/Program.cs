using System;

namespace DevTool
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;

    using JPV.RocketScience;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using SpaceLogistic.Core.Services.WorldGeneration;
    using SpaceLogistic.Core.Services.WorldGeneration.Import;

    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;;

            /*WikiTableToJson.Convert(
                "G:\\Projects\\my\\DotNet\\SpaceLogistic\\DevTool\\Files\\WikiTable.txt",
                "G:\\Projects\\my\\DotNet\\SpaceLogistic\\DevTool\\Files\\WikiTable.json",
                new Dictionary<int, string>()
                    {
                        { 2, "name" },
                        { 6, "diameter" },
                        { 7, "mass" },
                        { 8, "orbit" }
                    });*/

            /*var sun = new StarSystemImporter().Import(@"G:\Stuff\Data\solar-system.json");
            sun = new WorldGenerator(new WorldSettings()).Modify(sun);

            Out.WriteLine($"Lowest distance between bodies: {(sun.GetMinDistanceBetweenBodies() / 1e6):0.00} Gm");
            Out.WriteLine();

            sun.Print();
            
            Console.ReadLine();*/
        }
    }
}
