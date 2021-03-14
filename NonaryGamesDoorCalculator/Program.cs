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
            var doors = new[] { 3, 7, 8 };
            for (var k = 1; k <= doors.Length; ++k)
            {
                IterateCombinations(doors, k, (k, doors) =>
                {
                    CheckDoorCombinations(people.ToArray(), doors, (possibility) =>
                    {
                        possibilities.Add(possibility);
                    }, door: 0);
                });
            }

            foreach (var possibility in possibilities.OrderByDescending(p => p.NumPeople))
            {
                Console.WriteLine($"Remaining people: {people.Length - possibility.NumPeople}    {possibility}");
            }
        }

        static void CheckDoorCombinations(Person[] remainingPeople, int[] doors, Action<Possibility> handlePossibility, int door = 0, Stack<DoorInfo> doorInfos = null)
        {
            if (door >= doors.Length)
            {
                handlePossibility(new Possibility(doorInfos.Reverse().ToArray()));
                return;
            }

            doorInfos ??= new Stack<DoorInfo>();
            for (var numPeople = 3; numPeople <= 5 && numPeople <= remainingPeople.Length; ++numPeople)
            {
                IterateCombinations(remainingPeople, k: numPeople, (k, people) =>
                {
                    if (DigitalRoot(people) == doors[door])
                    {
                        doorInfos.Push(new DoorInfo { DigitalRoot = doors[door], People = people.ToArray() });
                        CheckDoorCombinations(remainingPeople.Except(people).ToArray(), doors, handlePossibility, door + 1, doorInfos);
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
