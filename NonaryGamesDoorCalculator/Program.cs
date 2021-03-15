using System;
using System.Collections.Generic;
using System.Linq;

namespace NonaryGamesDoorCalculator
{
    class Program
    {
        static void Main()
        {
            var people = new[] {
                //new Person("Zero", 0),
                new Person("Ace", 1),
                //new Person("Snake", 2),
                new Person("Santa", 3),
                new Person("Clover", 4),
                new Person("Junpei", 5),
                new Person("June", 6),
                new Person("Seven", 7),
                new Person("Lotus", 8),
                //new Person("Ninth Man", 9)
            };

            var possibilities = new List<Possibility>();
            var doors = new[] { 1, 2, 6 };
            for (var k = 1; k <= doors.Length; ++k)
            {
                IterateCombinations(doors, k, (k, doors) =>
                {
                    CheckDoorCombinations(people.ToArray(), doors, 3..5, (possibility) =>
                    {
                        possibilities.Add(possibility);
                    });
                });
            }

            foreach (var possibility in possibilities.OrderByDescending(p => p.NumPeople))
            {
                Console.WriteLine($"Remaining people: {people.Length - possibility.NumPeople}  {possibility}");
                var remainingPeople = people.Except(possibility.DoorInfos.SelectMany(di => di.People));
                Console.WriteLine($"\t{string.Join<Person>(", ", remainingPeople)} ({DigitalRoot(remainingPeople)})");
            }
        }

        static void CheckDoorCombinations(Person[] remainingPeople, int[] doors, Range range, Action<Possibility> handlePossibility, int door = 0, Stack<DoorInfo> doorInfos = null)
        {
            if (door >= doors.Length)
            {
                handlePossibility(new Possibility(doorInfos.Reverse().ToArray()));
                return;
            }

            doorInfos ??= new Stack<DoorInfo>();
            for (var numPeople = range.Start.Value; numPeople <= range.End.Value && numPeople <= remainingPeople.Length; ++numPeople)
            {
                IterateCombinations(remainingPeople, k: numPeople, (k, people) =>
                {
                    if (DigitalRoot(people) == doors[door])
                    {
                        doorInfos.Push(new DoorInfo { DigitalRoot = doors[door], People = people.ToArray() });
                        CheckDoorCombinations(remainingPeople.Except(people).ToArray(), doors, range, handlePossibility, door + 1, doorInfos);
                        doorInfos.Pop();
                    }
                });
            }
        }

        static int DigitalRoot(IEnumerable<Person> people)
        {
            return DigitalRoot(people.Sum(p => p.Number));
        }

        static int DigitalRoot(int x)
        {
            if (x < 10)
            {
                return x;
            }

            var sum = 0;
            while (x > 0)
            {
                sum += x % 10;
                x /= 10;
            }
            return DigitalRoot(sum);
        }

        static void IterateCombinations<T>(T[] items, int k, Action<int, T[]> handleCombination)
        {
            var current = new T[k];
            IterateCombinations(items, k, ref current, handleCombination);
        }

        static void IterateCombinations<T>(T[] items, int k, ref T[] current, Action<int, T[]> handleCombination, int i = 0, int depth = 0)
        {
            if (depth == k)
            {
                handleCombination(k, current);
            }
            else
            {
                while (i <= items.Length - (k - depth))
                {
                    current[depth] = items[i];
                    IterateCombinations(items, k, ref current, handleCombination, ++i, depth + 1);
                }
            }
        }
    }
}
