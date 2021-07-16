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

        public void Run()
        {
            SeedContent();
            PrintMenu();
        }

        private void PrintMenu()
        {
            bool keepGoing = true;
            while (keepGoing)
            {
                Console.Clear();
                Console.WriteLine("MAIN MENU\n" +
                    "Welcome. What would you like to do?\n" +
                    "1. Go to developer menu.\n" +
                    "2. Go to developer team menu.\n" +
                    "3. Exit the application.\n");

                string userChoice = Console.ReadLine();

                Console.Clear();
                switch (userChoice)
                {
                    case "1":
                        DeveloperMenu();
                        break;
                    case "2":
                        PrintDeveloperTeamMenu();
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
                    "Please select which action you would like to perform: \n" +
                    "1. Add A New Developer\n" +
                    "2. View All Developers\n" +
                    "3. View A Specific Developer\n" +
                    "4. Update A Developer\n" +
                    "5. Remove A Developer\n" +
                    "6. View All Developers That Require Pluralsight Acces\n" +
                    "7. Go back to main menu\n");

                string userChoice = Console.ReadLine();

                Console.Clear();
                switch (userChoice)
                {
                    //Add a new developer
                    case "1":
                        Developer DevToAdd = new Developer();
                        Console.WriteLine("Please provide an ID for the developer: ");
                        DevToAdd.ID = int.Parse(Console.ReadLine());

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
                        DevRepo.printDevelopersInRepo();
                        PressAnyKeyToContinue();
                        break;
                    //View a specific developer
                    case "3":
                        DevRepo.printDevelopersInRepo();
                        Console.WriteLine("Please provide the ID of the user you'd like to see more specifically.");
                        int userSelectedId = int.Parse(Console.ReadLine());
                        Developer specifiedDeveloper = (Developer)DevRepo.GetBusinessObjectsById(userSelectedId);
                        Console.Clear();
                        Console.WriteLine($"ID: {specifiedDeveloper.ID}, Name: {specifiedDeveloper.Name}, Has Pluralsight Access: {specifiedDeveloper.HasPluralsightAccess}");

                        if (DevsOnTeam.ContainsKey(userSelectedId))
                        {
                            DevelopmentTeam devsDevTeam = GetDevelopersDevTeamByDeveloperId(userSelectedId);
                            Console.WriteLine($"Assigned Development Team: {devsDevTeam.Name} - ID: {devsDevTeam.ID}");
                        }
                        else
                        {
                            Console.WriteLine("Developer is not assigned to a Development Team");
                        }

                        PressAnyKeyToContinue();
                        break;
                    //Update a developer
                    case "4":
                        DevRepo.printDevelopersInRepo();

                        //Get The Developer
                        Console.WriteLine("Please provide the ID of the user you'd like to update.");
                        int developertoUpdateId = int.Parse(Console.ReadLine());
                        Developer originalDeveloper = (Developer)DevRepo.GetBusinessObjectsById(developertoUpdateId);

                        Developer newDeveloper = new Developer(originalDeveloper.ID, originalDeveloper.Name, originalDeveloper.HasPluralsightAccess);

                        Console.WriteLine("Would you like to update the user's ID? Y/N");

                        string userReply = Console.ReadLine().ToUpper();
                        if (userReply == "Y")
                        {
                            Console.WriteLine("What is the user's new ID?");

                            int newID = int.Parse(Console.ReadLine());

                            if (newID.GetType() == typeof(int))
                            {
                                newDeveloper.ID = newID;
                                UpdateUserIdInDictionary(originalDeveloper.ID, newID);
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
                        DevRepo.UpdateRepositoryObjectById(developertoUpdateId, newDeveloper);
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
                        DevRepo.printDevelopersInRepo();
                        Console.WriteLine("Please select a developer that you would like to remove by their ID\n" +
                            "!!!WARNING: removing a developer will remove them from their respective development team!!!");

                        int devToRemoveId = int.Parse(Console.ReadLine());
                        Developer devToRemove = (Developer)DevRepo.GetBusinessObjectsById(devToRemoveId);

                        if (devToRemove != null)
                        {
                            if (IsDevOnADevTeam(devToRemove))
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
                        PressAnyKeyToContinue();
                        break;
                    default:
                        Console.WriteLine("Option invalid. Returning to the developer menu.");
                        PressAnyKeyToContinue();
                        break;
                }
            }

        }

        private void PrintDeveloperTeamMenu()
        {
            Console.Clear();
            Console.WriteLine("\n" +
                "1. Add A Development Team\n" +
                "2. View All Development Teams\n" +
                "3. View A Specific Development Team And It's Developers\n" +
                "4. Update A Development Team Name\n" +
                "5. Add Developers To A Development Team\n" +
                "6. Remove Developers From A Development Team\n" +
                "7. Add Multiple Developers To A Development Team\n" +
                "8. Go Back To Main Menu.\n");

            string userChoice = Console.ReadLine();
            Console.Clear();
            switch (userChoice)
            {
                case "1":
                    break;
                case "2":
                    break;
                case "3":
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
                    PressAnyKeyToContinue();
                    break;
                default:
                    Console.WriteLine("Option invalid. Returning to the main menu.");
                    PressAnyKeyToContinue();
                    break;
            }
        }

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

        private bool IsDevOnADevTeam(Developer passDeveloper)
        {
            return DevsOnTeam.ContainsKey(passDeveloper.ID);
        }

        private DevelopmentTeam GetDevelopersDevTeamByDeveloperId(int id)
        {
            DevelopmentTeam devsDevelopmentTeam = DevTeamRepo.GetDevelopmentTeamById(DevsOnTeam[id]);
            return devsDevelopmentTeam;
        }

        private void UpdateUserIdInDictionary(int ID, int newID)
        {
            int devsDevTeam = DevsOnTeam[ID];
            DevsOnTeam.Add(newID, devsDevTeam);
            DevsOnTeam.Remove(ID);
        }
    }
}

