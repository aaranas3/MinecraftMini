namespace MinecraftMini
{
    internal class Program
    {
        const string DATA_FILE = "../../../weapons.csv";
        static void Main(string[] args)
        {
            RunProgram();
        }

        /************ Core Method ************/

        static void RunProgram()
        {
            List<Weapon> weapons = ReadFromFile();

            Console.WriteLine($"\nLoaded {weapons.Count} weapons from file.\n");
            char userChoice;

            do
            {
                DisplayMainMenu();
                userChoice = Char.ToLower(GetUserChar("Please enter your preferred choice: "));
                switch (userChoice)
                {
                    case 'a':
                        // DONE: AddWeapon(weapons);
                        AddWeapon(weapons);
                        break;
                    case 'v':
                        // DONE: DisplayWeapons(weapons);
                        DisplayWeapons(weapons);
                        break;
                    case 'e':
                        // DONE: EditWeapon(weapons);
                        EditWeapon(weapons);
                        break;
                    case 's':
                        // DONE: SearchWeapon(weapons);
                        SearchWeapon(weapons);
                        break;
                    case 'r':
                        // DONE: RemoveWeapon(weapons);
                        RemoveWeapon(weapons);
                        break;
                        // DONE: Sorting
                    case 'o':
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nSorting is not available in this version.");
                        Console.WriteLine("Weapons are listed in the order they were added.");
                        Console.WriteLine("This ensures inventory reflects restocking history.");
                        Console.ResetColor();
                        break;
                    case 'q':
                        // DONE: WriteToFile(weapons);
                        WriteToFile(weapons);
                        Console.WriteLine("Saving and exiting...");
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input. Please try again.");
                        Console.ResetColor();
                        break;
                }
            } while (userChoice != 'q');
        }

        /// <summary>
        /// Reads weapon data from a CSV file and returns a list of Weapon objects.
        /// </summary>
        /// <returns>A list of weapons loaded from the CSV file.</returns>
        static List<Weapon> ReadFromFile()
        {
            List<Weapon> weaponsFromFile = new List<Weapon>();

            if (File.Exists(DATA_FILE))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(DATA_FILE))
                    {
                        string weaponData;

                        while ((weaponData = reader.ReadLine()) != null)
                        {
                            if (string.IsNullOrWhiteSpace(weaponData))
                                continue;

                            string[] parts = weaponData.Split(',');

                            if (parts.Length == 4)
                            {
                                string type = parts[0].Trim();
                                bool qualityParsed = int.TryParse(parts[1], out int quality);
                                bool costParsed = double.TryParse(parts[2], out double cost);
                                bool amountParsed = int.TryParse(parts[3], out int amountInInventory);

                                if (qualityParsed && costParsed && amountParsed)
                                {
                                    try
                                    {
                                        Weapon weapon = new Weapon(type, quality, cost, amountInInventory);
                                        weaponsFromFile.Add(weapon);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"There was a problem creating a weapon from the following data: {ex.Message}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Could not extract values from: {weaponData}");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Skipping line – expected 4 values but found something else: {weaponData}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading file: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"No file found at {DATA_FILE}, starting with empty inventory.");
            }

            return weaponsFromFile;
        }


        // DONE: AddWeapon(List<Weapon>): prompts for new weapon and adds it
        /// <summary>
        /// Prompts the user to enter a new weapon's details and adds it to the list.
        /// Prevents duplicate weapon types using a basic foreach loop.
        /// Validates all input and retries if invalid values are entered.
        /// </summary>
        /// <param name="weapons">The current list of weapons to which a new one will be added.</param>
        static void AddWeapon(List<Weapon> weapons)
        {
            Console.WriteLine("\n--- Add New Weapon ---");
            string type;
            while (true)
            {
                type = GetUserString("Enter weapon type: ").Trim();

                bool isDuplicate = false;
                foreach (Weapon weapon in weapons)
                {
                    if (weapon.Type.ToLower() == type.ToLower())
                    {
                        isDuplicate = true;
                        break;
                    }
                }

                if (isDuplicate)
                {
                    Console.WriteLine("That weapon already exists. Try a different name.\n");
                }
                else
                {
                    break;
                }
            }

            int quality = PromptWithRetry("Enter weapon quality (can be negative): ", GetUserInt);
            double cost = PromptWithRetry("Enter weapon cost (minimum 1): ", GetUserDouble);
            int amount = GetUserInt("Enter amount in inventory (minimum 0): ", 0, int.MaxValue);

            while (true)
            {
                try
                {
                    if (ConfirmAction("Are you sure you want to save this weapon?"))
                    {
                        Weapon newWeapon = new Weapon(type, quality, cost, amount);
                        weapons.Add(newWeapon);
                        Console.WriteLine("Weapon added successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Weapon creation cancelled.");
                    }
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to create weapon: {ex.Message}");
                    if (ex.Message.Contains("Cost"))
                    {
                        cost = PromptWithRetry("Enter weapon cost (minimum 1): ", GetUserDouble);
                    }
                    else if (ex.Message.Contains("Inventory"))
                    {
                        amount = PromptWithRetry("Enter amount in inventory (minimum 0): ", GetUserInt);
                    }
                    else
                    {
                        // backup: re-ask both to make sure
                        cost = PromptWithRetry("Enter weapon cost (minimum 1): ", GetUserDouble);
                        amount = PromptWithRetry("Enter amount in inventory (minimum 0): ", GetUserInt);
                    }
                }
            }
        }



        /// <summary>
        /// Saves all weapons in the list to a CSV file.
        /// </summary>
        /// <param name="weapons">List of weapons to save.</param>
        static void WriteToFile(List<Weapon> weapons)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(DATA_FILE))
                {
                    foreach (Weapon weapon in weapons)
                    {
                        string csvLine = $"{weapon.Type},{weapon.Quality},{weapon.Cost},{weapon.AmountInInventory}";
                        writer.WriteLine(csvLine);
                    }
                }

                Console.WriteLine("Inventory saved to file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
        }


        /// <summary>
        /// Displays all weapons in the inventory in a formatted table with index numbers,
        /// including Type, Quality, Cost, Inventory, Rarity, and Total Value.
        /// </summary>
        /// <param name="weapons">List of Weapon objects to display.</param>
        static void DisplayWeapons(List<Weapon> weapons)
        {
            if (IsInventoryEmpty(weapons)) return;

            Console.WriteLine("\n--- Weapon Inventory ---\n");

            // Header
            Console.WriteLine(
                $"{"Item No.",-10}" +
                $"{"Type",-20}" +
                $"{"Quality",10}" +
                $"{"Cost",12}" +
                $"{"In Stock",12}" +
                $"{"Rarity",12}" +
                $"{"Total Value",15}");

            Console.WriteLine(new string('-', 94));

            for (int i = 0; i < weapons.Count; i++)
            {
                Weapon weapon = weapons[i];
                Console.WriteLine(
                    $"{i.ToString("D3"),-10}" +
                    $"{weapon.Type,-20}" +
                    $"{weapon.Quality,10}" +
                    $"{weapon.Cost,12:F2}" +
                    $"{weapon.AmountInInventory,12}" +
                    $"{weapon.Rarity,12}" +
                    $"{weapon.ValueOfAllItems,15:F2}");
            }

            Console.WriteLine(); // extra spacing after table
        }


        // DONE: SearchWeapon(List<Weapon>): finds and displays a weapon by Type
        /// <summary>
        /// Searches for a weapon by type and displays its details if found.
        /// </summary>
        /// <param name="weapons">List of weapons to search in.</param>
        static void SearchWeapon(List<Weapon> weapons)
        {
            bool found = false;

            do
            {
                Console.Write("\nEnter weapon type to search (or type -1 to cancel): ");
                string searchType = Console.ReadLine().Trim();

                if (CheckCancelInput(searchType))
                    return;

                foreach (Weapon weapon in weapons)
                {
                    if (weapon.Type.ToLower() == searchType.ToLower())
                    {
                        Console.WriteLine("\n--- Weapon Found ---\n");
                        Console.WriteLine(weapon.WeaponDetails());
                        Console.WriteLine();
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    Console.WriteLine("Weapon not found in inventory.");
                }
            } while (!found);
        }

        // DONE: EditWeapon(List<Weapon>): updates an existing weapon's data
        /// <summary>
        /// Edits the quality, cost, and amount in inventory of a weapon selected by index.
        /// </summary>
        /// <param name="weapons">List of weapons to edit from.</param>
        static void EditWeapon(List<Weapon> weapons)
        {
            if (IsInventoryEmpty(weapons)) return;

            // Show the table first (optional but helpful)
            DisplayWeapons(weapons);

            int index = GetUserInt("\nEnter the Item No. of the weapon to edit (e.g. 0, 1, 2): ");

            if (index < 0 || index >= weapons.Count)
            {
                Console.WriteLine("Invalid item number. Please try again.");
                return;
            }

            Weapon weaponToEdit = weapons[index];

            try
            {
                int newQuality = GetUserInt("Enter new quality: ");
                double newCost = GetUserDouble("Enter new cost: ");
                int newAmount = GetUserInt("Enter new amount in inventory: ");

                weaponToEdit.Quality = newQuality;
                weaponToEdit.Cost = newCost;
                weaponToEdit.AmountInInventory = newAmount;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Weapon updated successfully!");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error updating weapon: {ex.Message}");
                Console.ResetColor();
            }
        }

        // DONE: RemoveWeapon(List<Weapon>): removes a weapon by Type
        /// <summary>
        /// Removes a weapon from the inventory by item number.
        /// </summary>
        /// <param name="weapons">List of weapons to remove from.</param>
        static void RemoveWeapon(List<Weapon> weapons)
        {
            if (IsInventoryEmpty(weapons)) return;

            DisplayWeapons(weapons);

            int indexToRemove = GetUserInt("\nEnter the Item No. of the weapon to remove: ");

            if (indexToRemove < 0 || indexToRemove >= weapons.Count)
            {
                Console.WriteLine("Invalid item number. Please try again.");
                return;
            }

            // show confirmation before deleting
            Weapon weaponToRemove = weapons[indexToRemove];
            Console.Write($"\nAre you sure you want to remove '{weaponToRemove.Type}'? (y/n): ");
            string confirmInput = Console.ReadLine().Trim().ToLower();

            if (confirmInput == "y")
            {
                weapons.RemoveAt(indexToRemove);
                Console.WriteLine("Weapon removed successfully.");
            }
            else
            {
                Console.WriteLine("Operation cancelled.");
            }
        }


        /************ Display Method ************/
        /// <summary>
        /// Displays the main menu options for managing the weapon inventory.
        /// </summary>
        /// <remarks>
        /// This menu allows the user to choose actions such as adding, viewing,
        /// editing, searching, or removing weapons, as well as quitting the program.
        /// </remarks>
        static void DisplayMainMenu()
        {
            Console.WriteLine("MENU OPTIONS:\n" +
                "\t[a]dd new weapon\n" +
                "\t[v]iew weapons\n" +
                "\t[e]dit weapon\n" +
                "\t[s]earch weapon\n" +
                "\t[r]emove weapon\n" +
                "\t[o]sort (not available)\n" +
                "\t[q]uit\n");
        }

        /************ Helper Method ************/

        /// <summary>
        /// Asks the user to confirm an action with a yes/no prompt.
        /// </summary>
        /// <param name="question">The question to display (e.g. "Do you want to save this weapon?")</param>
        /// <returns>True if the user confirms with 'y'; false if 'n'.</returns>
        static bool ConfirmAction(string question)
        {
            while (true)
            {
                Console.Write($"{question} (y/n): ");
                string response = Console.ReadLine().Trim().ToLower();

                if (response == "y")
                    return true;
                else if (response == "n")
                    return false;
                else
                    Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
            }
        }


        /// <summary>
        /// Checks if the user entered "-1" to cancel the current operation.
        /// Displays a cancellation message and returns true if cancelled.
        /// </summary>
        /// <param name="input">The user input to check.</param>
        /// <returns>True if the input is "-1"; otherwise, false.</returns>
        static bool CheckCancelInput(string input)
        {
            if (input.Trim() == "-1")
            {
                Console.WriteLine("Operation cancelled.");
                return true;
            }

            return false;
        }


        /// <summary>
        /// Prompts the user for input using a given function, retrying on exception.
        /// </summary>
        /// <typeparam name="T">The return type of the input (e.g., int, double).</typeparam>
        /// <param name="prompt">The message to show to the user.</param>
        /// <param name="inputFunc">The function that reads and validates input.</param>
        /// <returns>The successfully validated input of type T.</returns>
        static T PromptWithRetry<T>(string prompt, Func<string, T> inputFunc)
        {
            while (true)
            {
                try
                {
                    return inputFunc(prompt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Invalid input: {ex.Message}\n");
                }
            }
        }

        /// <summary>
        /// Checks if the weapon list is empty and shows a message if so.
        /// </summary>
        /// <param name="weapons">The list of weapons to check.</param>
        /// <returns>True if the list is empty, otherwise false.</returns>
        static bool IsInventoryEmpty(List<Weapon> weapons)
        {
            if (weapons.Count == 0)
            {
                Console.WriteLine("Inventory is empty.");
                return true;
            }
            return false;
        }

        /// <summary>
        /// A reusable method which prompts the user for input then returns their inputted string.
        /// </summary>
        /// <param name="question">A message to display to the user.</param>
        /// <returns>The user-inputted text.</returns>
        static string GetUserString(string question)
        {
            string userResponse;
            // ask the question
            Console.Write(question);
            // get the answer
            userResponse = Console.ReadLine();
            // return the answer
            return userResponse;
        }

        /// <summary>
        /// A reusable method which prompts the user for input then returns their inputted double.
        /// </summary>
        /// <param name="question">A message to display to the user.</param>
        /// <returns>A user-entered double.</returns>
        static double GetUserDouble(string question)
        {
            while (true)
            {
                Console.Write(question);
                string input = Console.ReadLine();

                if (double.TryParse(input, out double value))
                {
                    if (value >= 1)
                    {
                        return value;
                    }
                    else
                    {
                        Console.WriteLine("Cost must be at least 1.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            }
        }

        /// <summary>
        /// A reusable method which prompts the user for input then returns their inputted int.
        /// </summary>
        /// <param name="question">A message to display to the user.</param>
        /// <returns>A user-entered int.</returns>
        static int GetUserInt(string question)
        {
            while (true)
            {
                // ask the user a question
                Console.Write(question);

                // read in their answer
                // try to parse it as a int
                try
                {
                    return int.Parse(Console.ReadLine());
                    // if that works, return it
                    // this is the only way to exit the method
                }
                catch  // otherwise, loop back & try again
                {
                    Console.WriteLine("Please enter a valid number. Try again.");
                }
            } // end of loop
        } // end of method

        /// <summary>
        /// Prompts the user to enter an integer within a specific range and retries until valid.
        /// </summary>
        /// <param name="question">The question to ask the user.</param>
        /// <param name="min">Minimum acceptable value (inclusive).</param>
        /// <param name="max">Maximum acceptable value (inclusive).</param>
        /// <returns>A valid integer within the specified range.</returns>
        static int GetUserInt(string question, int min, int max)
        {
            while (true)
            {
                Console.Write(question);
                string input = Console.ReadLine();

                if (int.TryParse(input, out int value))
                {
                    if (value >= min && value <= max)
                    {
                        return value;
                    }
                    else
                    {
                        Console.WriteLine($"Please enter a number between {min} and {max}.");
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a valid number.");
                }
            }
        }


        /// <summary>
        /// A reusable method which prompts the user for input then returns their inputted char.
        /// </summary>
        /// <param name="question">A message to display to the user.</param>
        /// <returns>A user-entered char.</returns>
        static char GetUserChar(string question)
        {
            while (true)
            {
                // ask the user a question
                Console.Write(question);

                // read in their answer
                // try to parse it as a char
                try
                {
                    return char.Parse(Console.ReadLine());
                    // if that works, return it
                    // this is the only way to exit the method
                }
                catch  // otherwise, loop back & try again
                {
                    Console.WriteLine("Please enter a valid character. Try again.");
                }
            } // end of loop
        } // end of method
    }
}
