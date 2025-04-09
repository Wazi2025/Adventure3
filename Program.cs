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

        public Dictionary<string, Room> Exits { get; set; }

        public Room(string name, string description, string roomExit, string roomExit2)
        {
            Name = name;
            Description = description;
            RoomExit = roomExit;
            RoomExit2 = roomExit2;

            Exits = new Dictionary<string, Room>();
            Items = new List<string>();
        }

        // Adds an exit from this room to another
        public string AddExit(string direction, Room targetRoom)
        {
            Exits[direction.ToLower()] = targetRoom;
            string temp = $"There are exit(s) {direction}";
            return temp;
        }

    }//End of class Room


    public class Player
    {
        public Room CurrentRoom { get; set; }

        public Player(Room startRoom)
        {
            CurrentRoom = startRoom;
        }

        public void Move(string direction)
        {
            if (CurrentRoom.Exits.ContainsKey(direction.ToLower()))
            {
                CurrentRoom = CurrentRoom.Exits[direction.ToLower()];
                Console.WriteLine($"You move {direction} and enter {CurrentRoom.Name}.\n");
                Console.WriteLine(CurrentRoom.Description);

                IterateItems();
                // //iterate items in room
                // for (int i = 0; i < CurrentRoom.Items.Count(); i++)
                // {
                //     Console.WriteLine($"You see:  {CurrentRoom.Items[i]}");
                // }

                Console.WriteLine(CurrentRoom.RoomExit);
                Console.WriteLine(CurrentRoom.RoomExit2);
            }
            else if (direction == "look")
            {
                Console.WriteLine(CurrentRoom.Description);

                IterateItems();

                Console.WriteLine(CurrentRoom.RoomExit);
                Console.WriteLine(CurrentRoom.RoomExit2);
            }
            else
            {
                Console.WriteLine("You can't go that way.");

            }

        }
    }//End of class Player

    //instantiate rooms
    static Room bridge = new Room("the Bridge", "The control panels blink in a rhythmic pattern. You're on the bridge of your ship.", "", "");
    static Room dockingBay = new Room("the Docking Bay", "You're in the docking bay. There's a shuttle here.", "", "");
    static Room storageRoom = new Room("a Storage Room", "Crates and boxes fill this storage room. It's dimly lit.", "", "");

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

        //add items to rooms
        bridge.Items.Add("a keycard");
        bridge.Items.Add("an old newspaper");
        dockingBay.Items.Add("a DL-44 Blaster");
        storageRoom.Items.Add("a broom");
        storageRoom.Items.Add("a bucket");
    }

    static public void IterateItems()
    {
        //iterate items in room
        for (int i = 0; i < player.CurrentRoom.Items.Count(); i++)
        {
            Console.WriteLine($"You see: {player.CurrentRoom.Items[i]}");
        }
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
                Console.WriteLine($"{player.CurrentRoom.Name}\n");
                Console.WriteLine(player.CurrentRoom.Description);

                IterateItems();

                Console.WriteLine(player.CurrentRoom.RoomExit);
                Console.WriteLine(player.CurrentRoom.RoomExit2);
                gameStarted = true;
            }
            Console.WriteLine("What now?");
            string input = Console.ReadLine().Trim().ToLower();


            if (input == "quit" || input == "exit") break;

            player.Move(input);
        }

    }

}