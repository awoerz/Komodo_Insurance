using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komodo_Insurance_Class_Library
{
    public abstract class BusinessObjects
    {
        //Properties
        public int ID { get; set; }
        public string Name { get; set; }

        //Constructors
        public BusinessObjects() { }
        public BusinessObjects(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }

    public class Developer : BusinessObjects
    {
        //Properties
        public bool HasPluralsightAccess { get; set; }

        //Constructors
        public Developer() { }

        public Developer(int id, string name, bool pluralsightAccess)
            : base(id, name)
        {
            HasPluralsightAccess = pluralsightAccess;
        }
    }

    public class DevelopmentTeam : BusinessObjects
    {
        //Properties
        public List<Developer> DevTeam { get; set; }

        //Constructors
        public DevelopmentTeam() { }

        public DevelopmentTeam(List<Developer> devList, int id, string name)
            : base(id, name)
        {
            DevTeam = devList;
        }

        //Public Methods
        public bool AddDevelopers(List<Developer> DeveloperRepositoryDevs)
        {
            //Initializing devs that are not already on the dev team list:
            var devsAbleToAdd = new List<Developer>();

            //If DevTeam is not null (Meaning already instantiated) then make sure those devs are on list to add again.
            if (DevTeam != null)
            {
                devsAbleToAdd = DeveloperRepositoryDevs.Except(DevTeam).ToList();
            }
            else
            //else instantiate the Dev Team and add all of the developers in developer repo to list of developers we can add to a dev team.
            {
                DevTeam = new List<Developer>();
                devsAbleToAdd = DeveloperRepositoryDevs;
            }

            Console.WriteLine("Please enter the ID of the Developer that you would like to add to the Development Team");
            foreach (var dev in devsAbleToAdd)
            {
                Console.WriteLine($"ID: {dev.ID}, Name: {dev.Name}");
            }
            Console.WriteLine("");
            string userInputAsString = Console.ReadLine();
            var devToAdd = GetDeveloperFromListById(DeveloperRepositoryDevs, int.Parse(userInputAsString));
            if (devToAdd != null)
            {
                DevTeam.Add(devToAdd);
                return true;
            }

            return false;
        }

        public bool RemoveDeveloperById(int id)
        {
            List<Developer> AnotherDevTeam = new List<Developer>();

            Console.WriteLine("Please enter the ID of the Developer that you would like to remove to the Development Team");
            foreach (var dev in DevTeam)
            {
                Console.WriteLine($"ID: {dev.ID}, Name: {dev.Name}");
            }
            Console.WriteLine("");
            string userInputAsString = Console.ReadLine();
            var devToRemove = GetDevFromTeamById(int.Parse(userInputAsString));
            if (devToRemove != null)
            {
                DevTeam.Remove(devToRemove);
                return true;
            }

            return false;
        }

        //Private Helper Methods:
        private Developer GetDevFromTeamById(int id)
        {
            foreach (var dev in DevTeam)
            {
                if (dev.ID == id)
                {
                    return dev;
                }
            }
            return null;
        }

        private Developer GetDeveloperFromListById(List<Developer> devList, int id)
        {
            foreach (var dev in devList)
            {
                if (dev.ID == id)
                {
                    return dev;
                }
            }
            return null;
        }
    }
}
