// using System.ComponentModel.Design;
// using System.IO.Pipelines;
// using System.Runtime.CompilerServices;
using System;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
using System.Threading;

namespace Adventure3;

class Program
{
    public class Room
    {
        public int allItems = 0;
        //Name of the room
        public string Name { get; set; }

        //Room description
        public string Description { get; set; }

        //List of items (if any) in room
        public List<string> Items { get; set; }

        //List of number of exits in room
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
        public void AddExit(string playerAction, Room targetRoom)
        {
            Exits[playerAction.ToLower()] = targetRoom;

            NumberOfExits.Add(playerAction);
        }
    }//End of class Room


    public class Player
    {
        public List<string> Inventory { get; set; }
        public Room CurrentRoom { get; set; }
        public const string look = "look";
        public const string get = "get";
        public const string inv = "i";
        public const string help = "help";
        public const string north = "north";
        public const string south = "south";
        public const string east = "east";
        public const string west = "west";
        public const string exit = "exit";
        public const string quit = "quit";

        public Player(Room startRoom)
        {
            Inventory = new List<string>();
            CurrentRoom = startRoom;
        }

        public void Action(string playerAction)
        {

            if (CurrentRoom.Exits.ContainsKey(playerAction))
            {
                CurrentRoom = CurrentRoom.Exits[playerAction];
                Console.WriteLine($"You move {playerAction} and enter the {CurrentRoom.Name}.\n");
                Console.WriteLine(CurrentRoom.Description);

                //Call methods
                IterateItems();
                DisplayExits(CurrentRoom);

            }
            else if (playerAction == look)
            {
                Console.WriteLine($"{CurrentRoom.Name}\n");
                Console.WriteLine(CurrentRoom.Description);

                //Call methods
                IterateItems();
                DisplayExits(CurrentRoom);
            }
            else if (playerAction == inv)
            {
                //Call method
                ShowInventory();
            }
            else if (playerAction.Contains(get))
            {
                //Call method
                GetItem(playerAction);
            }
            else if (playerAction == help)
            {
                //show what kind of commands/actions the user can perform
                //Note: move this to it's own method
                Console.WriteLine("List of possible actions to perform: ");
                Console.WriteLine($"{look}, {inv}, {get}, {north}, {south}, {east}, {west} \n");
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

    static public int Initialize()
    {
        //Less chance of accidentally writing playerActions wrong         
        //const string north = "north";
        // const string south = "south";
        // const string east = "east";
        // const string west = "west";

        // Connecting rooms 
        dockingBay.AddExit(Player.south, bridge);
        storageRoom.AddExit(Player.north, bridge);

        //Bridge room has two exits
        bridge.AddExit(Player.south, storageRoom);
        bridge.AddExit(Player.north, dockingBay);

        //Add items to rooms
        bridge.Items.Add("keycard");
        bridge.Items.Add("newspaper");
        dockingBay.Items.Add("Blaster");
        storageRoom.Items.Add("broom");
        storageRoom.Items.Add("bucket");

        //Add items to player inventory
        // player.Inventory.Add("some pocket lint");
        // player.Inventory.Add("a perfectly ordinary babel fish");

        //return all items in all rooms so we can use it in Main in the ending message
        return bridge.Items.Count + dockingBay.Items.Count + storageRoom.Items.Count;
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
        Console.WriteLine($"There are exit(s): {ExitsList}\n");
    }

    static public void IterateItems()
    {
        //Check if there are items in the room
        if (player.CurrentRoom.Items.Count > 0)
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
        //Check if player has any items
        if (player.Inventory.Count > 0)
            Console.WriteLine("You have: ");
        else
            Console.WriteLine("You have no items");

        for (int i = 0; i < player.Inventory.Count(); i++)
        {
            Console.WriteLine($"- {player.Inventory[i]}");
        }
        Console.WriteLine();
    }

    static public void GetItem(string playerAction)
    {
        bool itemFound = false;
        bool missingItem = false;
        string tempItem = "";

        //Player can pickup any item in CurrentRoom's itemlist
        for (int i = 0; i < player.CurrentRoom.Items.Count; i++)
        {
            int itemIndex = 0;
            //Split player input based on using blank space(" ") as a marker
            //Note: This means that currently this routine only accepts 1 blank space, i.e. "old newspaper" will fail
            //
            //Check if blank space (" ") can actually be found in the input string
            //or else we would get an ArgumentOutOfRangeException when assigning itemIndex
            if (playerAction.IndexOf(" ")! > -1)
                itemIndex = playerAction.IndexOf(" ");
            else
            {
                //inform user if input is missing a string (or item in this case). e.g. 'get card'/get keycard
                Console.WriteLine("What do you want to get?");
                missingItem = true;
                break;
            }

            //remove any whitespace
            tempItem = playerAction.Substring(itemIndex).Trim();

            //Extract the word from index 0 to before the blank space, in this case it's 'get'.
            //We don't currently use it but might later on
            string get = playerAction.Substring(0, itemIndex);

            //if player input word is the same as an item in CurrentRoom
            //we add this item to inventory and remove it from CurrentRoom's itemlist
            if (tempItem.ToLower() == player.CurrentRoom.Items[i].ToLower())
            {
                player.Inventory.Add(player.CurrentRoom.Items[i]);

                //Remove item from CurrentRoom's itemlist
                player.CurrentRoom.Items.RemoveAt(i);

                Console.WriteLine($"You pick up {tempItem}.\n");
                itemFound = true;
                break;
            }
        }

        //Inform user if item is not in CurrentRoom's itemlist
        if (!itemFound && missingItem == false)
            Console.WriteLine($"There is no '{tempItem}' to pick up!\n");
    }
    static void Main(string[] args)
    {
        string imagePath = @"C:\Users\Instruktor\Desktop\CSharp\Adventure3\gfx\hhg.png";
        bool gameStarted = false;
        int items;
        string winner = @"                                                                                                                                                                            
                                                                                                                                                                               
      ___    ___ ________  ___  ___          ________  ________  _______           ________  ________  ___  ___       ___       ___  ________  ________   _________  ___       
     |\  \  /  /|\   __  \|\  \|\  \        |\   __  \|\   __  \|\  ___ \         |\   __  \|\   __  \|\  \|\  \     |\  \     |\  \|\   __  \|\   ___  \|\___   ___\\  \      
     \ \  \/  / | \  \|\  \ \  \\\  \       \ \  \|\  \ \  \|\  \ \   __/|        \ \  \|\ /\ \  \|\  \ \  \ \  \    \ \  \    \ \  \ \  \|\  \ \  \\ \  \|___ \  \_\ \  \     
      \ \    / / \ \  \\\  \ \  \\\  \       \ \   __  \ \   _  _\ \  \_|/__       \ \   __  \ \   _  _\ \  \ \  \    \ \  \    \ \  \ \   __  \ \  \\ \  \   \ \  \ \ \  \    
       \/  /  /   \ \  \\\  \ \  \\\  \       \ \  \ \  \ \  \\  \\ \  \_|\ \       \ \  \|\  \ \  \\  \\ \  \ \  \____\ \  \____\ \  \ \  \ \  \ \  \\ \  \   \ \  \ \ \__\   
     __/  / /      \ \_______\ \_______\       \ \__\ \__\ \__\\ _\\ \_______\       \ \_______\ \__\\ _\\ \__\ \_______\ \_______\ \__\ \__\ \__\ \__\\ \__\   \ \__\ \|__|   
    |\___/ /        \|_______|\|_______|        \|__|\|__|\|__|\|__|\|_______|        \|_______|\|__|\|__|\|__|\|_______|\|_______|\|__|\|__|\|__|\|__| \|__|    \|__|     ___ 
    \|___|/                                                                                                                                                               |\__\
                                                                                                                                                                          \|__|
                                                                                                                                                                               ";

        //Call method
        //items is all items in all rooms combined from Initialize method
        items = Initialize();

        // Start the process (open image with the default viewer)
        Process imageViewerProcess = Process.Start(new ProcessStartInfo(imagePath) { UseShellExecute = true });


        while (true)
        {

            //Process.Start(new ProcessStartInfo(imagePath) { UseShellExecute = true });

            //Prolly add some more descriptive text here, lol!            
            if (!gameStarted)
            {
                Console.WriteLine("Hello there, stranger. Type 'help' for possible commands");
                gameStarted = true;
            }

            Console.WriteLine("What now?");
            string input = Console.ReadLine().Trim().ToLower();

            //Break loop if user inputs 'quit' or 'exit'
            if (input == Player.exit || input == Player.quit)
            {
                //if (!imageViewerProcess.HasExited)
                //Note: The process doesn't seem to close. Look into why.
                imageViewerProcess.Kill();
                break;
            }

            if (player.Inventory.Count == items)
            //if (player.Inventory.Count == 1) //testing
            {
                string winner2 = $"Congratulations! You managed to collect {player.Inventory.Count} of {items} items. An amazing performance!\n";
                //Console.WriteLine($"Congratulations! You managed to collect {player.Inventory.Count} of {items} items. An amazing performance!\n");
                Console.WriteLine(winner);

                //Add some "animation" ;-)
                foreach (char c in winner2)
                {
                    Console.Write(c);
                    Thread.Sleep(20);
                }
                //break;

                //keep CLI open until user presses any key
                //Note: this crashes in VS Code but works fine in CLI
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
                break;
            }

            player.Action(input);
        }
    }//End of Main
}//End of Program class