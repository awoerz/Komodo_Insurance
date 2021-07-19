using Komodo_Insurance_Class_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komodo_Inusrance_Console
{
    class UI
    {
        private DeveloperRepository DevRepo = new DeveloperRepository();
        private DevelopmentTeamRepository DevTeamRepo = new DevelopmentTeamRepository();
        //Dictionary is used as a connection point for DevRepo and DevTeamRepo, they key should be a developer ID and the value should be a DevTeam ID.
        private Dictionary<int, int> DevsOnTeam = new Dictionary<int, int>();

        //UI Run Method
        public void Run()
        {
            SeedContent();
            PrintMenu();
        }

        //Menu Methods
        private void PrintMenu()
        {
            bool keepGoing = true;
            while (keepGoing)
            {
                Console.Clear();
                Console.WriteLine("MAIN MENU\n" +
                    "Welcome. What would you like to do?\n" +
                    "1. Go to developer menu.\n" +
                    "2. Go to development team menu.\n" +
                    "3. Exit the application.\n");

                string userChoice = Console.ReadLine();

                Console.Clear();
                switch (userChoice)
                {
                    case "1":
                        DeveloperMenu();
                        break;
                    case "2":
                        DevelopmentTeamMenu();
                        break;
                    case "3":
                        Console.WriteLine("Thank you for using the application\n" +
                            "Press any key to exit ...");
                        Console.ReadKey();
                        keepGoing = false;
                        break;
                    default:
                        Console.WriteLine("You did not select a valid option. Please select an option by the number of the option.");
                        break;
                }
            }
        }

        private void DeveloperMenu()
        {
            bool continueDevMenu = true;
            while (continueDevMenu)
            {
                Console.Clear();
                Console.WriteLine("DEVELOPER MENU\n" +
                    "Please select which action you would like to perform: \n\n" +
                    "1. Add a new developer\n" +
                    "2. View all developers\n" +
                    "3. View a specific developer\n" +
                    "4. Update a developer\n" +
                    "5. Remove a developer\n" +
                    "6. View all developers that require pluralsight acces\n" +
                    "7. Go back to main menu\n");

                string userChoice = Console.ReadLine();

                Console.Clear();
                switch (userChoice)
                {
                    //Add a new developer
                    case "1":
                        Developer DevToAdd = new Developer();

                        Console.WriteLine("Please provide a integer value for the developer's new ID.");
                        int newDevID = GetUserInputAsInt();
                        DevToAdd.ID = newDevID;

                        Console.WriteLine("Please provide the developer's name:");
                        DevToAdd.Name = Console.ReadLine();


                        Console.WriteLine("Does the developer need access to pluralsight? Y/N");
                        string userResponse = Console.ReadLine().ToUpper();
                        
                        if (userResponse == "Y")
                        {
                            DevToAdd.HasPluralsightAccess = true;
                        }
                        else
                        {
                            DevToAdd.HasPluralsightAccess = false;
                        }

                        DevRepo.AddObjectToRepository(DevToAdd);
                        Console.WriteLine("The user has been added.");

                        PressAnyKeyToContinue();
                        break;

                    //View all developers
                    case "2":
                        PrintDevelopersInRepo();
                        
                        PressAnyKeyToContinue();
                        break;

                    //View a specific developer
                    case "3":
                        Console.WriteLine("Please provide the ID of the developer you'd like to see more specifically.\n");
                        PrintDevelopersInRepo();
                        int userSelectedId = GetUserInputAsInt();

                        if (!DevRepo.RepositoryContainsObject(userSelectedId))
                        {
                            Console.WriteLine("Please try again. Entry does not contain a valid user number.");
                        }
                        else
                        {

                            Developer specifiedDeveloper = (Developer)DevRepo.GetBusinessObjectsById(userSelectedId);
                            Console.Clear();
                            Console.WriteLine($"ID: {specifiedDeveloper.ID}, Name: {specifiedDeveloper.Name}, Has Pluralsight Access: {specifiedDeveloper.HasPluralsightAccess}");

                            if (DevsOnTeam.ContainsKey(userSelectedId))
                            {
                                DevelopmentTeam devsDevTeam = DevTeamRepo.GetDevelopmentTeamById(userSelectedId);
                                Console.WriteLine($"Assigned Development Team: {devsDevTeam.Name} - ID: {devsDevTeam.ID}");
                            }
                            else
                            {
                                Console.WriteLine("Developer is not assigned to a Development Team");
                            }
                        }

                        PressAnyKeyToContinue();
                        break;

                    //Update a developer
                    case "4":
                        Console.WriteLine("Please provide the ID of the user you'd like to update.\n");
                        PrintDevelopersInRepo();

                        int developerToUpdateId = GetUserInputAsInt();

                        if (!DevRepo.RemoveRepositoryObjectById(developerToUpdateId))
                        {
                            Console.WriteLine("A developer ");
                        }

                        Developer originalDeveloper = (Developer)DevRepo.GetBusinessObjectsById(developerToUpdateId);

                        Developer newDeveloper = new Developer(originalDeveloper.ID, originalDeveloper.Name, originalDeveloper.HasPluralsightAccess);

                        Console.WriteLine("Would you like to update the user's ID? Y/N");
                        string userReply = Console.ReadLine().ToUpper();
                        if (userReply == "Y")
                        {
                            Console.WriteLine("What is the user's new ID?");

                            int newID = GetUserInputAsInt();
                            if(newID == 0)
                            {
                                Console.WriteLine("Sorry but the provided value was not a value that can be used as an ID.");
                            }
                            else
                            {
                                newDeveloper.ID = newID;
                                //Cheks if developer is on a dev team and updates the ID in the dictionary if so.
                                if (DevsOnTeam.ContainsKey(developerToUpdateId))
                                {
                                    UpdateUserIdInDictionary(developerToUpdateId, newID);
                                }
                            }
                        }

                        Console.WriteLine("Would you like to update the user's name? Y/N");
                        string userReplyTwo = Console.ReadLine().ToUpper();
                        if (userReplyTwo == "Y")
                        {
                            Console.WriteLine("What is the user's new name?");
                            newDeveloper.Name = Console.ReadLine();
                        }

                        Console.WriteLine($"The user's current pluralsight access is set to {originalDeveloper.HasPluralsightAccess}\n" +
                            $"Would you like to update user's pluralsight access? Y/N");
                        string userReplyThree = Console.ReadLine().ToUpper();
                        if (userReplyThree == "Y")
                        {
                            newDeveloper.HasPluralsightAccess = newDeveloper.HasPluralsightAccess ? false : true;
                            Console.WriteLine($"Update Pluralsight access to {newDeveloper.HasPluralsightAccess}");
                        }

                        //Update Developer with DevRepo method
                        DevRepo.UpdateRepositoryObjectById(developerToUpdateId, newDeveloper);
                        if (userReply == "Y" || userReplyTwo == "Y" || userReplyThree == "Y")
                        {
                            Console.WriteLine("Your selected user has been updated.");
                        }
                        else
                        {
                            Console.WriteLine("Why did you click this option then?");
                        }
                        
                        PressAnyKeyToContinue();
                        break;

                    //Remove a developer
                    case "5":
                        Console.WriteLine("Please select a developer that you would like to remove by their ID\n" +
                            "!!!WARNING: removing a developer will remove them from their respective development team!!!\n");
                        PrintDevelopersInRepo();

                        int devToRemoveId = int.Parse(Console.ReadLine());
                        Developer devToRemove = (Developer)DevRepo.GetBusinessObjectsById(DevsOnTeam[devToRemoveId]);

                        if (devToRemove != null)
                        {
                            if (DevsOnTeam.ContainsKey(devToRemoveId)) 
                            {
                                int devTeamId = DevsOnTeam[devToRemove.ID]; //Get the devTeams ID based on Devs Id
                                DevelopmentTeam devsDevelopmentTeam = DevTeamRepo.GetDevelopmentTeamById(devTeamId); //Get the entire team from repo as copy
                                devsDevelopmentTeam.RemoveDeveloperById(devToRemoveId); //remove the developer
                                bool devTeamUpdate = DevTeamRepo.UpdateRepositoryObjectById(devTeamId, devsDevelopmentTeam); //update the repository with devteam minus the dev.
                                if (devTeamUpdate)
                                {
                                    Console.WriteLine($"Developer: {devToRemove.Name} has been removed from Dev Team: {devsDevelopmentTeam.Name}.");
                                }
                            }
                            bool isDevRemoved = DevRepo.RemoveRepositoryObjectById(devToRemoveId);
                            if (isDevRemoved)
                            {
                                Console.WriteLine("The developer has been removed from the developer repo.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("The developer you selected could not be found. Please try again.");
                        }
                        
                        PressAnyKeyToContinue();
                        break;

                    //Show all devs with pluralsight access.
                    case "6":
                        List<Developer> devsWithoutPluralsight = DevRepo.GetDevelopersInRepositoryWithoutPluralsightAccess();
                        if (devsWithoutPluralsight.Count != 0)
                        {
                            foreach (Developer dev in devsWithoutPluralsight)
                            {
                                Console.WriteLine($"ID: {dev.ID}, Name: {dev.Name}, Has Pluralsight Access: {dev.HasPluralsightAccess}.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("All developers in developer repo have pluralsight access.");
                        }
                        
                        PressAnyKeyToContinue();
                        break;

                    //Return to menu
                    case "7":
                        continueDevMenu = false;
                        break;
                    default:
                        Console.WriteLine("Option invalid. Returning to the developer menu.");
                        
                        PressAnyKeyToContinue();
                        break;
                }
            }
        }

        private void DevelopmentTeamMenu()
        {
            bool continueDevTeamMenu = true;

            while (continueDevTeamMenu)
            {
                Console.Clear();
                Console.WriteLine("DEVELOPMENT TEAM MENU\n" +
                    "1. Add a development team\n" +
                    "2. View all development teams\n" +
                    "3. View a specific development team and it's developers\n" +
                    "4. Update a development team name\n" +
                    "5. Add developers to a development team\n" +
                    "6. Remove developers from a development team\n" +
                    "7. Add multiple developers to a development team\n" +
                    "8. Go back to main menu.\n");

                string userChoice = Console.ReadLine();
                Console.Clear();
                switch (userChoice)
                {
                    //Add a dev team
                    case "1":
                        DevelopmentTeam devTeamToAdd = new DevelopmentTeam();
                        DevTeamRepo.AddObjectToRepository(devTeamToAdd);

                        Console.WriteLine("Please provide an ID for the development team:");
                        devTeamToAdd.ID = GetUserInputAsInt();

                        Console.WriteLine("What would you like to name this development team?");
                        devTeamToAdd.Name = Console.ReadLine();

                        Console.WriteLine("Would you like to add any developers to the dev team? Y/N");
                        string userReply = Console.ReadLine().ToUpper();
                        if (userReply == "Y")
                        {
                            AddMultipleDevelopersToTeam(devTeamToAdd.ID);
                        }
                        Console.WriteLine("Your Dev team has been added.");
                        
                        PressAnyKeyToContinue();
                        break;
                    case "2":
                        PrintDevelopmentTeamsInRepo();
                        
                        PressAnyKeyToContinue();
                        break;
                    case "3":
                        Console.WriteLine("Please select a development team from the following list to add to view the members of.");
                        PrintDevelopmentTeamsInRepo();

                        int userSelection = GetUserInputAsInt();
                        
                        if (userSelection == 0 || !DevRepo.RepositoryContainsObject(userSelection))
                        {
                            Console.WriteLine("Please try again. Entry does not contain a valid user number.");
                        }
                        else
                        {
                            Console.Clear();
                            DevelopmentTeam userSelectedDevTeam = DevTeamRepo.GetDevelopmentTeamById(userSelection);
                            List<Developer> devTeamDevList = userSelectedDevTeam.DevTeam;
                            if (devTeamDevList.Count > 0)
                            {
                                Console.WriteLine($"ID: {userSelectedDevTeam.ID}, Name: {userSelectedDevTeam.Name}\n" +
                                    $"Dev Team Members:");
                                foreach (Developer dev in devTeamDevList)
                                {
                                    Console.WriteLine($"ID: {dev.ID}, Name: {dev.Name}, Has Pluralsight Access: {dev.HasPluralsightAccess}");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Sorry, but {userSelectedDevTeam.Name} has no developers assigned.");
                            }
                        }
                        
                        PressAnyKeyToContinue();
                        break;
                    case "4":
                        break;
                    case "5":
                        break;
                    case "6":
                        break;
                    case "7":
                        break;
                    case "8":
                        continueDevTeamMenu = false;
                        break;
                    default:
                        Console.WriteLine("Option invalid. Returning to the development team menu.");
                        PressAnyKeyToContinue();
                        break;
                }
            }
        }

        //Helper Methods
        private void PressAnyKeyToContinue()
        {
            Console.WriteLine("\n" +
                "Press any key to continue ...");
            Console.ReadKey();
        }

        private void SeedContent()
        {
            //Create some developers
            Developer Adam = new Developer(1, "Adam", true);
            Developer Amanda = new Developer(2, "Amanda", true);
            Developer Jordan = new Developer(3, "Jordan", true);
            Developer Daniel = new Developer(4, "Daniel", false);
            Developer Wendy = new Developer(5, "Wendy", false);
            Developer Josh = new Developer(6, "Josh", false);

            //Add Developers to Repo
            DevRepo.AddObjectToRepository(Adam);
            DevRepo.AddObjectToRepository(Amanda);
            DevRepo.AddObjectToRepository(Jordan);
            DevRepo.AddObjectToRepository(Daniel);
            DevRepo.AddObjectToRepository(Wendy);
            DevRepo.AddObjectToRepository(Josh);

            //Create Lists of Developers to assisnt in instantiating Dev Teams
            List<Developer> alphaList = new List<Developer>();
            List<Developer> betaList = new List<Developer>();
            List<Developer> gammaList = new List<Developer>();
            alphaList.Add(Adam);
            alphaList.Add(Amanda);
            betaList.Add(Jordan);
            betaList.Add(Daniel);

            //Create some dev teams
            DevelopmentTeam devTeamAlpha = new DevelopmentTeam(alphaList, 1, "Dev Team Alpha");
            DevelopmentTeam devTeamBeta = new DevelopmentTeam(betaList, 2, "Dev Team Beta");
            DevelopmentTeam devTeamGamma = new DevelopmentTeam(gammaList, 3, "Dev Team Gamma");

            //Update Devs Dictionary
            DevsOnTeam.Add(1, 1);
            DevsOnTeam.Add(2, 1);
            DevsOnTeam.Add(3, 2);
            DevsOnTeam.Add(4, 2);

            //Add dev teams to repo.
            DevTeamRepo.AddObjectToRepository(devTeamAlpha);
            DevTeamRepo.AddObjectToRepository(devTeamBeta);
            DevTeamRepo.AddObjectToRepository(devTeamGamma);
        }

        private void AddMultipleDevelopersToTeam(int TeamID)
        {
            bool keepAdding = true;
            string passedTeamName = DevTeamRepo.GetDevelopmentTeamById(TeamID).Name;
            DevelopmentTeam devTeamToGetDevs = new DevelopmentTeam(new List<Developer>(), TeamID, passedTeamName);
            List<Developer> devsAddingToTeam = new List<Developer>();
            List<int> validDevIds = new List<int>();
            while (keepAdding)
            {
                //Check if there are developers that are able to be added.
                validDevIds.Clear();
                foreach (Developer dev in DevRepo.GetDevelopersInRepository())
                {
                    //If the dev is not on a dev team
                    if (!DevsOnTeam.ContainsKey(dev.ID))
                    {
                        validDevIds.Add(dev.ID);
                    }
                }
                if (validDevIds.Count == 0)
                {
                    Console.WriteLine("Sorry but there are no more developers that not already assigned to a development team at this time.");
                    keepAdding = false;
                }
                else
                {
                    Console.WriteLine("The following developers are available. Please select the ID of the developer you'd like to add.\n" +
                        "If you entered this menu by mistake and wish to exit, enter the letter E.");
                    foreach (int devId in validDevIds)
                    {
                        Developer currentdev = (Developer)DevRepo.GetBusinessObjectsById(devId);
                        Console.WriteLine($"ID: {currentdev.ID}, Name: {currentdev.Name}, Access to pluralsight: {currentdev.HasPluralsightAccess}");
                    }
                    var userInput = Console.ReadLine().ToUpper();
                    //Exit if user screwed up.
                    if (userInput == "E")
                    {
                        keepAdding = false;
                    }
                    else
                    {
                        int selectedDevsId = int.Parse(userInput);
                        if (validDevIds.Contains(selectedDevsId))
                        {
                            Developer devToAdd = (Developer)DevRepo.GetBusinessObjectsById(int.Parse(userInput));
                            DevsOnTeam.Add(devToAdd.ID, TeamID);
                            devsAddingToTeam.Add(devToAdd);
                        }
                        else
                        {
                            Console.WriteLine("Your selection was invalid. Please try again.");
                        }
                    }
                    Console.WriteLine("Would you like to add an additional developer? Y/N");
                    userInput = Console.ReadLine().ToUpper();
                    if (userInput == "N")
                    {
                        keepAdding = false;
                    }
                    else if (userInput != "Y")
                    {
                        Console.WriteLine("Selection is invalid. Leaving developer addition");
                        keepAdding = false;
                    }
                }
            }
            if (devsAddingToTeam != null)
            {
                devTeamToGetDevs.AddDevelopers(devsAddingToTeam);
            }
        }

        private void PrintDevelopersInRepo()
        {
            List<Developer> devsInRepo = DevRepo.GetDevelopersInRepository();
            foreach(Developer dev in devsInRepo)
            {
                Console.WriteLine($"ID: {dev.ID}, Name: {dev.Name}, Has PluralsightAccess {dev.HasPluralsightAccess}");
            }
        }

        private void PrintDevelopmentTeamsInRepo()
        {
            List<DevelopmentTeam> devTeamsInRepo = DevTeamRepo.GetDevelopmentTeamsInRepository();
            foreach (DevelopmentTeam devTeam in devTeamsInRepo)
            {
                Console.WriteLine($"ID: {devTeam.ID}, Name: {devTeam.Name}");
            }
        }

        //Dictionary Helper Methods
        private void UpdateUserIdInDictionary(int ID, int newID)
        { 
            int devsDevTeam = DevsOnTeam[ID];
            DevsOnTeam.Add(newID, devsDevTeam);
            DevsOnTeam.Remove(ID);
        }

        private int GetUserInputAsInt()
        {
            int returnValue = 0;
            while(returnValue == 0)
            {
                string userInput = Console.ReadLine();
                try
                {
                    returnValue = int.Parse(userInput);
                }
                catch
                {
                    returnValue = 0;
                }
                if(returnValue == 0)
                {
                    Console.Clear();
                    Console.WriteLine("Please provide a valid integer for the response.");
                }
            }
            return returnValue;
        }
    }
}

