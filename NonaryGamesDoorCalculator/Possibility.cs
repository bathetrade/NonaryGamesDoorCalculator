using System.Linq;

namespace NonaryGamesDoorCalculator
{
    public class Possibility
    {
        public Possibility(DoorInfo[] doorInfos)
        {
            DoorInfos = doorInfos;
        }

        public int NumPeople => DoorInfos.SelectMany(di => di.People).Count();

        public DoorInfo[] DoorInfos { get; set; }

        public override string ToString()
        {
            return string.Join<DoorInfo>(", ", DoorInfos);
        }
    }
}
