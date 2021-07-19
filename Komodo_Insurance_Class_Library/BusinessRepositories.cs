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
        private List<BusinessObjects> _ObjectRepository = new List<BusinessObjects>();

        //Create
        public void AddObjectToRepository(BusinessObjects passedObject)
        {
            _ObjectRepository.Add(passedObject);
        }

        //Read
        public List<BusinessObjects> GetBusinessObjects()
        {
            return _ObjectRepository;
        }

        public BusinessObjects GetBusinessObjectsById(int id)
        {
            foreach (var obj in _ObjectRepository)
            {
                if (obj.ID == id)
                {
                    return obj;
                }
            }
            return null;
        }

        public bool RepositoryContainsObject(int id)
        {
            return _ObjectRepository.Contains(GetBusinessObjectsById(id));
        }

        //Update
        public bool UpdateRepositoryObjectById(int originalId, BusinessObjects newBusinessObject)
        {
            BusinessObjects originalBusinessObject = GetBusinessObjectsById(originalId);
            if (originalBusinessObject != null)
            {
                int originalBusinessObjectIndex = _ObjectRepository.IndexOf(originalBusinessObject);
                _ObjectRepository[originalBusinessObjectIndex] = newBusinessObject;
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

            int initialCount = _ObjectRepository.Count;
            _ObjectRepository.Remove(content);
            if (initialCount > _ObjectRepository.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
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
    }
    
    public class DevelopmentTeamRepository : BusinessRepository
    {
        //Getter - Change Business Objects to DevelopmentTeam Objects and Return as List.
        public List<DevelopmentTeam> GetDevelopmentTeamsInRepository()
        {
            List<DevelopmentTeam> DevelopmentTeamsToReturn = new List<DevelopmentTeam>();
            List<BusinessObjects> businessObjects = GetBusinessObjects();
            foreach (var businessObject in businessObjects)
            {
                DevelopmentTeamsToReturn.Add((DevelopmentTeam)businessObject);
            }
            return DevelopmentTeamsToReturn;
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
