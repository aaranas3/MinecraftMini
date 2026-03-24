using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace MinecraftMini
{
    class Weapon
    {

        //Implement fields and properties with validations in Weapon.cs.
        //Implement the default and greedy constructors.
        //Add computed properties(ValueOfAllItems and Rarity).
        //Add the method WeaponDetails() (and optionally the challenges if you choose to).
        //Finally, use methods in Program.cs(like your existing AddWeapon() method) to create and test Weapon objects from user input.


        // Weapon class requirements:

        // Fields and Properties:
        // DONE: string Type: cannot be null, empty, or whitespace (trim spaces)
        // Private field
        private string _type;

        // Public field
        public string Type
        {
            get { return _type; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Type cannot be null, empty or whitespace.");
                _type = value.Trim();
            }
        }

        // DONE: int Quality: can be negative or positive
        // Private field
        private int _quality;

        // Public field
        public int Quality
        {
            get { return _quality; }
            set
            {
                _quality = value;
            }
        }

        // DONE: double Cost: must be greater than or equal to 1
        // Private field
        private double _cost;

        // Public field
        public double Cost
        {
            get { return _cost; }
            set
            {
                if (value < 1)
                    throw new ArgumentException("Cost must be at least 1.");
                _cost = value;
            }
        }

        // DONE: int AmountInInventory: must be greater than or equal to 0
        // Private field
        private int _amountInInventory;

        // Public field
        public int AmountInInventory
        {
            get { return _amountInInventory; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Inventory amount cannot be negative.");
                }
                _amountInInventory = value;
            }
        }

        // Constructors:
        // DONE: Default constructor (no parameters)
        /// <summary>
        /// Creates a default Weapon values.
        /// </summary>
        public Weapon()
        {
            Type = "Wooden Sword";
            Quality = 1;
            Cost = 1;
            AmountInInventory = 1;
        }

        // DONE: Greedy constructor (parameters: type, quality, cost, amountInInventory)
        /// <summary>
        /// Creates a new Weapon with specified type, quality, cost, and inventory amount.
        /// </summary>
        /// <param name="type">The name or category of the weapon.</param>
        /// <param name="quality">The quality value of the weapon. Can be negative or positive.</param>
        /// <param name="cost">The cost per individual weapon. Must be 1 or greater.</param>
        /// <param name="amountInInventory">How many of this weapon are in stock. Must be 0 or greater.</param>
        public Weapon(string type, int quality, double cost, int amountInInventory)
        {
            Type = type;
            Quality = quality;
            Cost = cost;
            AmountInInventory = amountInInventory;
        }

        // Computed Properties:
        // DONE: double ValueOfAllItems (read-only): returns Cost * AmountInInventory
        public double ValueOfAllItems
        {
            get
            {
                return Cost * AmountInInventory;
            }
        }

        // DONE: string Rarity (read-only): returns rarity based on Quality:
        //            < 1       : Poor
        //            1  - 19   : Common
        //            20 - 49   : Rare
        //            50 - 99   : Epic
        //            100+      : Legendary
        public string Rarity
        {
            get
            {
                if (Quality < 1)
                {
                    return "Poor";
                }
                else if (Quality <= 19)
                {
                    return "Common";
                }
                else if (Quality <= 49)
                {
                    return "Rare";
                }
                else if (Quality <= 99)
                {
                    return "Epic";
                }
                else
                {
                    return "Legendary";
                }
            }
        }
        // Methods:
        // DONE: WeaponDetails(): returns formatted string with labeled details:
        //        (Type, Quality, Cost Per Item, Amount In Inventory, Rarity, Total Value)
        /// <summary>
        /// Returns a formatted string containing all important weapon details,
        /// including Type, Quality, Cost per Item, Amount in Inventory,
        /// Rarity, and Total Value of all items.
        /// </summary>
        /// <returns>A formatted string summarizing the weapon's details.</returns>
        public string WeaponDetails()
        {
            return $"{"Type:".PadRight(22)}{Type}\n" +
                   $"{"Quality:".PadRight(22)}{Quality}\n" +
                   $"{"Cost per item:".PadRight(22)}{Cost:F2}\n" +
                   $"{"Amount in Inventory:".PadRight(22)}{AmountInInventory}\n" +
                   $"{"Rarity:".PadRight(22)}{Rarity}\n" +
                   $"{"Total Value:".PadRight(22)}{ValueOfAllItems:F2}";
        }

        /*************** Optional Challenges (not for marks): ***************/
        // TODO: (optional) - ToCSVString(): returns CSV string (Type,Quality,Cost,AmountInInventory)
        // TODO: (optional) - Override ToString(): formatted output of Type, Quality, Cost, AmountInInventory

    }
}
