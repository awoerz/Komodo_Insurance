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
        public bool AddSingleDeveloper(Developer dev)
        {
            if (dev != null)
            {
                DevTeam.Add(dev);
                return true;
            }
            else
            {
                return true;
            }
        }

        public bool AddDevelopers(List<Developer> DevelopersToAddToDevTeam)
        {
            if(DevelopersToAddToDevTeam.Count > 0)
            {
                foreach (Developer dev in DevelopersToAddToDevTeam)
                {
                    DevTeam.Add(dev);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RemoveDeveloperById(int id)
        {
            var devToRemove = GetDevFromTeamById(id);
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

    }
}
