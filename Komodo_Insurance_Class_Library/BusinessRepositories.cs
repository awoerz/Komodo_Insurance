using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komodo_Insurance_Class_Library
{
    abstract public class BusinessRepository
    {
        //Properties
        private List<BusinessObjects> _objectRepository = new List<BusinessObjects>();

        //Getter
        public List<BusinessObjects> GetBusinessObjects()
        {
            return _objectRepository;
        }

        //Public Methods

        //Create
        public void AddObjectToRepository(BusinessObjects passedObject)
        {
            _objectRepository.Add(passedObject);
        }

        //Read
        public void ViewRepositoryObjects()
        {
            foreach (var obj in _objectRepository)
            {
                Console.WriteLine($"ID: {obj.ID}, Name: {obj.Name}");
            }
        }

        public bool IsEmpty()
        {
            if (_objectRepository.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Update
        public bool UpdateRepositoryObjectById(int originalId, BusinessObjects newBusinessObject)
        {
            BusinessObjects originalBusinessObject = GetBusinessObjectsById(originalId);
            if (originalBusinessObject != null)
            {
                int originalBusinessObjectIndex = _objectRepository.IndexOf(originalBusinessObject);
                _objectRepository[originalBusinessObjectIndex] = newBusinessObject;
                return true;
            }
            else
            {
                return false;
            }
        }

        //Delete
        public bool RemoveRepositoryObjectById(int id)
        {
            BusinessObjects content = GetBusinessObjectsById(id);
            if (content == null)
            {
                return false;
            }

            int initialCount = _objectRepository.Count;
            _objectRepository.Remove(content);
            if (initialCount > _objectRepository.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Private Helper Methods
        public BusinessObjects GetBusinessObjectsById(int id)
        {
            foreach (var obj in _objectRepository)
            {
                if (obj.ID == id)
                {
                    return obj;
                }
            }
            return null;
        }
    }
    public class DeveloperRepository : BusinessRepository
    {
        //Getter - Change Business Objects To Developer Objects and Return as List.
        public List<Developer> GetDevelopersInRepository()
        {
            List<Developer> devListToReturn = new List<Developer>();
            List<BusinessObjects> businessObjects = GetBusinessObjects();
            foreach (var businessObject in businessObjects)
            {
                devListToReturn.Add((Developer)businessObject);
            }
            return devListToReturn;
        }

        public List<Developer> GetDevelopersInRepositoryWithoutPluralsightAccess()
        {
            List<Developer> currentDevsInRepo = GetDevelopersInRepository();
            List<Developer> devsWithoutPluralsightAccess = new List<Developer>();

            foreach (Developer dev in currentDevsInRepo)
            {
                if (dev.HasPluralsightAccess == false)
                {
                    devsWithoutPluralsightAccess.Add(dev);
                }
            }
            return devsWithoutPluralsightAccess;
        }

        public void printDevelopersInRepo()
        {
            List<Developer> devsInRepo = GetDevelopersInRepository();
            foreach (Developer dev in devsInRepo)
            {
                Console.WriteLine($"ID: {dev.ID}, Name: {dev.Name}, Has Pluralsight Acces: {dev.HasPluralsightAccess}");
            }
        }
    }
    public class DevelopmentTeamRepository : BusinessRepository
    {
        //Getter - Change Business Objects to DevelopmentTeam Objects and Return as List.
        public List<DevelopmentTeam> GetDevelopmentTeamsInRepository()
        {
            List<DevelopmentTeam> DevelopmentTeamToReturn = new List<DevelopmentTeam>();
            List<BusinessObjects> businessObjects = GetBusinessObjects();
            foreach (var businessObject in businessObjects)
            {
                DevelopmentTeamToReturn.Add((DevelopmentTeam)businessObject);
            }
            return DevelopmentTeamToReturn;
        }

        public DevelopmentTeam GetDevelopmentTeamById(int id)
        {
            List<BusinessObjects> businessObjects = GetBusinessObjects();
            foreach (var businessObject in businessObjects)
            {
                if (businessObject.ID == id)
                {
                    return (DevelopmentTeam)businessObject;
                }
            }
            return null;
        }
    }
}
