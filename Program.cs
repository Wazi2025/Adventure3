using System.IO.Pipelines;
using System.Runtime.CompilerServices;

namespace Adventure3;

class Program
{
    public class Room
    {
        //Name of the room
        public string Name { get; set; }

        //Room description
        public string Description { get; set; }

        //List of items (if any) in room
        public List<string> Items { get; set; }

        //List of numer of exits in room
        public List<string> NumberOfExits { get; set; }

        public Dictionary<string, Room> Exits { get; set; }

        public Room(string name, string description)
        {
            Name = name;
            Description = description;

            Exits = new Dictionary<string, Room>();
            Items = new List<string>();
            NumberOfExits = new List<string>();
        }

        // Adds an exit from this room to another
        public void AddExit(string direction, Room targetRoom)
        {
            Exits[direction.ToLower()] = targetRoom;

            NumberOfExits.Add(direction);
        }
    }//End of class Room


    public class Player
    {
        public List<string> Inventory { get; set; }
        public Room CurrentRoom { get; set; }

        public Player(Room startRoom)
        {
            Inventory = new List<string>();
            CurrentRoom = startRoom;
        }

        public void Move(string direction)
        {

            if (CurrentRoom.Exits.ContainsKey(direction))
            {
                CurrentRoom = CurrentRoom.Exits[direction];
                Console.WriteLine($"You move {direction} and enter the {CurrentRoom.Name}.\n");
                Console.WriteLine(CurrentRoom.Description);

                //Call methods
                IterateItems();
                DisplayExits(CurrentRoom);

            }
            else if (direction == "look")
            {
                //Verbs prolly shouldn't be part of Move method or simply rename it to something like PlayerAction
                Console.WriteLine($"{CurrentRoom.Name}\n");
                Console.WriteLine(CurrentRoom.Description);

                //Call methods
                IterateItems();
                DisplayExits(CurrentRoom);
            }
            else if (direction == "inv")
            {
                //Call method
                ShowInventory();
            }
            else if (direction.Contains("get"))
            {
                //Call method
                GetItem(direction);
            }
            else
            {
                Console.WriteLine("You can't go that way.");
            }
        }
    }//End of class Player

    //instantiate rooms
    static Room bridge = new Room("Bridge", "The control panels blink in a rhythmic pattern. You're on the bridge of your ship.");
    static Room dockingBay = new Room("Docking Bay", "You're in the docking bay. There's a shuttle here.");
    static Room storageRoom = new Room("Storage Room", "Crates and boxes fill this storage room. It's dimly lit.");

    //Instantiate Player        
    static Player player = new Player(bridge);

    static public void Initialize()
    {
        //Less chance of accidentally writing directions wrong         
        const string north = "north";
        const string south = "south";
        const string east = "east";
        const string west = "west";

        // Connecting rooms 
        dockingBay.AddExit(south, bridge);
        storageRoom.AddExit(north, bridge);

        //Bridge room has two exits
        bridge.AddExit(south, storageRoom);
        bridge.AddExit(north, dockingBay);

        //Add items to rooms
        bridge.Items.Add("keycard");
        bridge.Items.Add("newspaper");
        dockingBay.Items.Add("Blaster");
        storageRoom.Items.Add("broom");
        storageRoom.Items.Add("bucket");

        //Add items to player inventory
        player.Inventory.Add("some pocket lint");
        player.Inventory.Add("a perfectly ordinary ballpen");
    }

    static public void DisplayExits(Room CurrentRoom)
    {
        //check number of exits for CurrentRoom (the room the player is in)       
        string ExitsList = "";

        for (int i = 0; i < CurrentRoom.NumberOfExits.Count; i++)
        {
            if (CurrentRoom.NumberOfExits.Count <= 1)
            {
                ExitsList = "\n" + CurrentRoom.NumberOfExits[i];
            }
            else
            {
                ExitsList += "\n" + CurrentRoom.NumberOfExits[i];
            }
        }
        Console.WriteLine($"There are exit(s): {ExitsList}");
    }

    static public void IterateItems()
    {
        Console.WriteLine("You see: ");

        //iterate items in room
        for (int i = 0; i < player.CurrentRoom.Items.Count(); i++)
        {
            Console.WriteLine(player.CurrentRoom.Items[i]);
        }

        //Add some space after item iteration
        Console.WriteLine();
    }

    static public void ShowInventory()
    {
        //Show player's inventory        
        Console.WriteLine("You have: ");
        for (int i = 0; i < player.Inventory.Count(); i++)
        {
            Console.WriteLine($"- {player.Inventory[i]}");
        }
        Console.WriteLine();
    }

    static public void GetItem(string direction)
    {
        //Player can pickup any item in CurrentRoom's itemlist
        for (int i = 0; i < player.CurrentRoom.Items.Count; i++)
        {
            //Split player input based using blank space as a marker
            //Note: This means that currently this routine only accepts 1 blank space, i.e. "old newspaper" will fail
            //Preserve item name capitalization for pickup/inventory

            // string tempItem = direction;
            // string ItemAsIs = tempItem;
            int itemIndex = direction.IndexOf(" ");
            string tempItem = direction.Substring(itemIndex + 1);

            //Extract the word from index 0 to before the blank space, in this case it's 'get'.
            //We don't currently use it but might later on
            string get = direction.Substring(0, itemIndex);

            //if player input word is the same as an item in CurrentRoom
            //we add this item to inventory and remove it from CurrentRoom's itemlist
            if (tempItem.ToLower() == player.CurrentRoom.Items[i].ToLower())
            {
                player.Inventory.Add(player.CurrentRoom.Items[i]);
                player.CurrentRoom.Items[i] = "";
                Console.WriteLine($"You pick up {tempItem}.");
            }
            //Note: implement some way of searching through CurrentRoom's itemlist
            //and inform user if item doesn't exist
            // else if (!player.CurrentRoom.Items.Contains(tempItem))
            // {
            //     Console.WriteLine($"{tempItem} is not an item!");
            // }

        }
    }
    static void Main(string[] args)
    {
        bool gameStarted = false;

        //Call method
        Initialize();

        while (true)
        {
            //Prolly add some more descriptive text here, lol!            
            if (!gameStarted)
            {
                Console.WriteLine("Hello there, stranger. Why not have a 'look' around?");
                gameStarted = true;
            }

            Console.WriteLine("What now?");
            //string input = Console.ReadLine().Trim().ToLower();
            string input = Console.ReadLine().ToLower();

            //Break loop if user inputs 'quit' or 'exit'
            if (input == "quit" || input == "exit") break;

            player.Move(input);
        }
    }//End of Main
}//End of Program class