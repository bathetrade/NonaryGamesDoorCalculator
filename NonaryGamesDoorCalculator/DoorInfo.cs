using System;

namespace NonaryGamesDoorCalculator
{
    public class DoorInfo
    {
        public int DigitalRoot { get; set; }
        public Person[] People { get; set; }
        public int Degree => People.Length;

        public override string ToString()
        {
            return $"{{Door: {DigitalRoot} [{String.Join<Person>(", ", People)}]}}";
        }
    }
}
