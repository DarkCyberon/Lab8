using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;

namespace Lab8_N1 {
    class Actions {
        static void Main() {
            string[] lines = File.ReadAllLines("text.txt");
            List<Subtitle> subtitles = MakeTheSubtitlesList(lines);
            Action(subtitles);
        }

        static void Action(List<Subtitle> subtitles) {
            int width, height;

            Console.Write("Введите желаемую ширину рамки (Если текст не поместится в рамку, он не выведется на экран): ");
            while (!int.TryParse(Console.ReadLine(), out width)) {
                Console.Write("Некорректно введённое число, попробуйте ещё раз: ");
            }
            Console.Clear();

            Console.Write("Введите желаемую длину рамки: ");
            while (!int.TryParse(Console.ReadLine(), out height)) {
                Console.Write("Некорректно введённое число, попробуйте ещё раз: ");
            }
            Console.Clear();

            Console.CursorVisible = false;
            DrawTheFrame(width, height);

            for (int second = 0; second < Subtitle.MaxTime; second++)  {
                foreach (var subtitle in subtitles) {
                    if (subtitle.StartTime == second) WriteTheSubtitle(subtitle, width, height);
                    else if (subtitle.FinishTime == second) RemoveTheSubtitle(subtitle, width, height);
                }

                Thread.Sleep(1000);
            }
        }

        static void WriteSubtitleThere(Subtitle subtitle, int width, int height) {
            if (!SetTheCursor(subtitle, width, height)) return;
            SetTheColor(subtitle);
            Console.WriteLine(subtitle.Text);
        }

        static void RemoveSubtitleThere(Subtitle subtitle, int width, int height) {
            if (!SetTheCursor(subtitle, width, height)) return;
            for (int i = 0; i < subtitle.Text.Length; i++) {
                Console.Write(" ");
            }
        }

        static bool SetTheCursor(Subtitle subtitle, int width, int height) {
            if (width - 1 < subtitle.Text.Length) return false;

            switch (subtitle.Position)  {
                case "Top":
                    Console.SetCursorPosition((width - subtitle.Text.Length) / 2, 1);
                    break;
                case "Bottom":
                    Console.SetCursorPosition((width - subtitle.Text.Length) / 2, height - 2);
                    break;
                case "Right":
                    Console.SetCursorPosition(width - 1 - subtitle.Text.Length, height / 2);
                    break;
                case "Left":
                    Console.SetCursorPosition(1, height / 2);
                    break;
            }

            return true;
        }

        static void SetTheColor(Subtitle subtitle) {
            Console.ForegroundColor = subtitle.Color;
        }

        static void DrawTheFrame(int width, int height) {
            Console.Write("┌");
            for (int i = 0; i < width - 2; i++) Console.Write("─");
            Console.WriteLine("┐");

            for (int i = 0; i < height - 2; i++) {
                Console.SetCursorPosition(0, i + 1);
                Console.Write("|");
                Console.SetCursorPosition(width - 1, i + 1);
                Console.WriteLine("|");
            }

            Console.Write("└");
            for (int i = 0; i < width - 2; i++) Console.Write("─");
            Console.WriteLine("┘");
        }

        static List<Subtitle> CollectSubtitlesToList(string[] lines) {
            var subtitles = new List<Subtitle>();
            var timeOfSubtitles = new List<string>();

            foreach (var line in lines) {
                var subtitle = new Subtitle();
                string[] elements = line.Split(":");
                int timeLength = elements[0].Length + elements[1].Length + 4;
                string phrase = line[(timeLength + 1)..];
                timeOfSutitles.Add(line.Substring(0, timeLength));

                if (phrase[0] == '[') {
                    int index = phrase.IndexOf(']');
                    string[] elementsOfExpression = phrase[1..index].Split(", ");
                    subtitle.Text = phrase[(index + 2)..];
                    subtitle.Position = elementsOfExpression[0];
                    subtitle.Color = Subtitle.Colors.GetValueOrDefault(elementsOfExpression[1]);
                }
                else subtitle.Text = phrase;

                subtitles.Add(subtitle);
            }

            return AddTheTime(subtitles, timeOfSutitles);
        }

        static List<Subtitle> AddTheTime(List<Subtitle> subtitles, List<string> timeOfSubtitles) {
            for (int i = 0; i < subtitles.Count; i++) {
                string[] elements = timeOfSubtitles[i].Split(" - ");
                string[] startParameters = elements[0].Split(":");
                string[] endParameters = elements[1].Split(":");

                subtitles[i].StartTime = int.Parse(startParameters[0]) * 60 + int.Parse(startParameters[1]);
                subtitles[i].FinishTime = int.Parse(endParameters[0]) * 60 + int.Parse(endParameters[1]);

                if (subtitles[i].FinishTime > Subtitle.MaxTime) Subtitle.MaxTime = subtitles[i].FinishTime;
            }

            return subtitles;
        }
    }
}

