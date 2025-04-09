using System.IO.Pipelines;
using System.Runtime.CompilerServices;

namespace Adventure3;

class Program
{
    public class Room
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string RoomExit { get; set; }
        public string RoomExit2 { get; set; }
        public List<string> Items { get; set; }
        public List<string> NumberOfExits { get; set; }

        public Dictionary<string, Room> Exits { get; set; }

        public Room(string name, string description, string roomExit, string roomExit2)
        {
            Name = name;
            Description = description;
            RoomExit = roomExit;
            RoomExit2 = roomExit2;

            Exits = new Dictionary<string, Room>();
            Items = new List<string>();
            NumberOfExits = new List<string>();
        }

        // Adds an exit from this room to another
        public string AddExit(string direction, Room targetRoom)
        {
            Exits[direction.ToLower()] = targetRoom;

            NumberOfExits.Add(direction);

            //Note: don't need a return value, change method return to void            
            return "";
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

            if (CurrentRoom.Exits.ContainsKey(direction.ToLower()))
            {
                CurrentRoom = CurrentRoom.Exits[direction.ToLower()];
                Console.WriteLine($"You move {direction} and enter the {CurrentRoom.Name}.\n");
                Console.WriteLine(CurrentRoom.Description);

                //Call methods
                IterateItems();
                DisplayExits(CurrentRoom);

            }
            else if (direction == "look")
            {
                Console.WriteLine($"{CurrentRoom.Name}\n");
                Console.WriteLine(CurrentRoom.Description);

                //Call methods
                IterateItems();
                DisplayExits(CurrentRoom);
            }
            else if (direction == "inv")
            {
                //Show player's inventory
                //Note: should be in its own method
                Console.WriteLine("You have: ");
                for (int i = 0; i < player.Inventory.Count(); i++)
                {
                    Console.WriteLine($"- {player.Inventory[i]}");
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("You can't go that way.");
            }

        }
    }//End of class Player

    //instantiate rooms
    static Room bridge = new Room("Bridge", "The control panels blink in a rhythmic pattern. You're on the bridge of your ship.", "", "");
    static Room dockingBay = new Room("Docking Bay", "You're in the docking bay. There's a shuttle here.", "", "");
    static Room storageRoom = new Room("Storage Room", "Crates and boxes fill this storage room. It's dimly lit.", "", "");

    //Instantiate Player        
    static Player player = new Player(bridge);

    static public void Initialize()
    {
        const string north = "north";
        const string south = "south";
        const string east = "east";
        const string west = "west";

        // Connecting rooms 
        dockingBay.RoomExit = dockingBay.AddExit(south, bridge);
        storageRoom.RoomExit = storageRoom.AddExit(north, bridge);

        //Bridge room has two exits
        bridge.RoomExit = bridge.AddExit(south, storageRoom);
        bridge.RoomExit2 = bridge.AddExit(north, dockingBay);

        //Add items to rooms
        bridge.Items.Add("a keycard");
        bridge.Items.Add("an old newspaper");
        dockingBay.Items.Add("a DL-44 Blaster");
        storageRoom.Items.Add("a broom");
        storageRoom.Items.Add("a bucket");

        //Add items to player inventory
        player.Inventory.Add("some pocket lint");
        player.Inventory.Add("a perfectly ordinary ballpen");
    }

    static public void DisplayExits(Room CurrentRoom)
    {
        //check number of exits        
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
        Console.WriteLine();
    }

    static void Main(string[] args)
    {
        bool gameStarted = false;

        //Call method
        Initialize();

        while (true)
        {
            //describe starting room instead of just "Which way?"
            //the first time. After that Player will supply room descriptions
            if (!gameStarted)
            {
                Console.WriteLine("Hello there, stranger. Why not have a 'look' around?");

                gameStarted = true;
            }

            Console.WriteLine("What now?");
            string input = Console.ReadLine().Trim().ToLower();


            if (input == "quit" || input == "exit") break;

            player.Move(input);
        }

    }

}