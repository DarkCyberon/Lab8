using System;
using System.Collections.Generic;

namespace Lab8_N1 {
    class Subtitle {
        public int StartingTime, FinishingTime;
        public string Text;
        public static int MaxTime;
        public string Position { get; set; } = "Bottom";
        public ConsoleColor Color { get; set; } = Colors.GetValueOrDefault("White");

        public static readonly Dictionary<string, ConsoleColor> Colors = new() {
            { "Blue", ConsoleColor.Blue },
            { "White", ConsoleColor.White },
            { "Green", ConsoleColor.Green },
            { "Yellow", ConsoleColor.Yellow },
            { "Red", ConsoleColor.Red }
        };
    }
}
