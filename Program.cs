
using System;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
using System.Threading;

namespace Adventure3;

class Program
{
    //instantiate rooms
    static Room bridge = new Room("Bridge", "The control panels blink in a rhythmic pattern. You notice a computer with a thin slot in it. You're on the bridge of your ship.");
    static Room dockingBay = new Room("Docking Bay", "You're in the docking bay. There's a shuttle here.");
    static Room storageRoom = new Room("Storage Room", "Crates and boxes fill this storage room. It's dimly lit.");

    //Instantiate Player        
    static Player player = new Player(bridge);

    static public int Initialize()
    {
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
        //Process imageViewerProcess = Process.Start(new ProcessStartInfo(imagePath) { UseShellExecute = true });


        while (true)
        {
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
                //Note: The process doesn't seem to close/closes intermittently. Look into why.

                // string pName = imageViewerProcess.ProcessName;
                //imageViewerProcess.CloseMainWindow();
                //Console.WriteLine($"The process {imageViewerProcess.ProcessName} has been killed");

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

            player.Action(input, player);
        }
    }//End of Main
}//End of Program class