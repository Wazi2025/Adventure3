public class Player
{
    public List<string> Inventory { get; set; }
    public Room CurrentRoom { get; set; }
    public const string look = "look";
    public const string get = "get";
    public const string use = "use";
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

    public void Action(string playerAction, Player player)
    {

        if (CurrentRoom.Exits.ContainsKey(playerAction))
        {
            CurrentRoom = CurrentRoom.Exits[playerAction];
            Console.WriteLine($"You move {playerAction} and enter the {CurrentRoom.Name}.\n");
            Console.WriteLine(CurrentRoom.Description);

            //Call methods
            IterateItems(player);
            DisplayExits(CurrentRoom);

        }
        else if (playerAction == look)
        {
            Console.WriteLine($"{CurrentRoom.Name}\n");
            Console.WriteLine(CurrentRoom.Description);

            //Call methods
            IterateItems(player);
            DisplayExits(CurrentRoom);
        }
        else if (playerAction == inv)
        {
            //Call method
            ShowInventory(player);
        }
        else if (playerAction.Contains(get))
        {
            //Call method
            GetItem(playerAction, player);
        }
        else if (playerAction.Contains(use))
        {
            //Call method
            UseItem(playerAction, player);
        }
        else if (playerAction == help)
        {
            //Call method
            Help();
        }
        else
        {
            Console.WriteLine("You can't go that way.");
        }
    }

    static public void Help()
    {
        //show what kind of commands/actions the user can perform        
        Console.WriteLine("List of possible actions to perform: ");
        Console.WriteLine($"{look}, {inv}, {get}, {use}, {north}, {south}, {east}, {west} \n");
    }
    static public void DisplayExits(Room CurrentRoom)
    {
        //check number of exits for CurrentRoom (the room the player is in)       
        string exitsList = "";
        const string exitText = "There is an exit:";
        const string exitTextMany = "There are exits:";
        bool manyExits = true;

        for (int i = 0; i < CurrentRoom.NumberOfExits.Count; i++)
        {
            if (CurrentRoom.NumberOfExits.Count <= 1)
            {
                manyExits = false;
                exitsList = "\n" + CurrentRoom.NumberOfExits[i];
            }
            else
            {
                manyExits = true;
                exitsList += "\n" + CurrentRoom.NumberOfExits[i];
            }
        }
        if (manyExits)
            Console.WriteLine($"{exitTextMany} {exitsList}\n");
        else
            Console.WriteLine($"{exitText} {exitsList}\n");

    }//End DisplayExits

    static public void IterateItems(Player player)
    {
        const string youSeeText = "You see: ";

        //Check if there are items in the room
        if (player.CurrentRoom.Items.Count > 0)
            Console.WriteLine(youSeeText);

        //iterate items in room
        foreach (var item in player.CurrentRoom.Items)
            Console.WriteLine(item.Key);

        //Add some space after item iteration
        Console.WriteLine();
    }//End of IterateItems

    static public void ShowInventory(Player player)
    {
        const string ItemsText = "You have: ";
        const string noItemsText = "You have no items";

        //Show player's inventory   
        //Check if player has any items
        if (player.Inventory.Count > 0)
            Console.WriteLine(ItemsText);
        else
            Console.WriteLine(noItemsText);

        for (int i = 0; i < player.Inventory.Count(); i++)
        {
            Console.WriteLine($"- {player.Inventory[i]}");
        }
        Console.WriteLine();
    }//End of ShowInventory

    static public void UseItem(string playerAction, Player player)
    {
        //Note: this is just a test. Will need to implement (somehow) some sort of Infocom type parser.
        #region test
        //int itemIndex;
        // string item = "keycard";
        // string room = "Bridge";

        // string[] arrayWords = playerAction.Split(" ");
        // for (int i = 0; i < arrayWords.Length; i++)
        // {
        //     string tempItem = "";
        //     if (i >= 1)
        //     {
        //         tempItem = arrayWords[i];

        //         if (!player.Inventory.Contains(arrayWords[i]))
        //         {
        //             Console.WriteLine($"You don't have {tempItem}!");
        //             return;
        //         }

        //     }
        // }

        // if (playerAction.IndexOf(" ")! > -1)
        //     itemIndex = playerAction.IndexOf(" ");
        // else
        // {
        //     //inform user if input is missing a string (or item in this case). e.g. 'use card'/use keycard
        //     Console.WriteLine("What do you want to use?");
        //     return;
        // }

        // if (player.Inventory.Contains(item) && player.CurrentRoom.Name == room)
        // {
        //     Console.WriteLine($"You insert the {item} into the computer slot.");
        // }
        // else if (!player.Inventory.Contains(item))
        //     Console.WriteLine($"You don't have a {item}!");
        // else if (player.CurrentRoom.Name != room && player.Inventory.Contains(item))
        //     Console.WriteLine($"You are not in {room}!");
        #endregion

        string[] tempItem = playerAction.Split(" ");
        //Check SAL's suggestion:
        foreach (var item in player.CurrentRoom.Puzzles)
        {
            if (player.CurrentRoom.Name.Equals("Bridge"))
            {
                for (int i = 0; i <= player.Inventory.Count; i++)
                {
                    bool hasItem = player.Inventory.Contains(tempItem[1]);
                    bool correctRoom = player.CurrentRoom.Name.Equals(item.Value);

                    if (!hasItem)
                    {
                        Console.WriteLine($"You are not in posession of {tempItem[1]}");
                        return;
                    }
                    else if (item.Key != tempItem[1] && hasItem)
                    {
                        Console.WriteLine($"That does not seem to work. Yet.");
                        return;
                    }
                    else if (item.Key == tempItem[1] && hasItem && !correctRoom)
                    {
                        Console.WriteLine($"Unable to use {tempItem[1]} here. Perhaps another room?");
                        return;
                    }
                    else if (item.Key == tempItem[1] && hasItem && correctRoom)
                    {
                        Console.WriteLine($"You insert {item.Key} into the computer slot");
                        Console.WriteLine("A hologram of a beautiful woman coalesce in front of you. 'Hello, I am SAL. How may I be of service?'");
                        return;
                    }
                }
            }

        }//End of Puzzles loop

    }//End method
    static public void GetItem(string playerAction, Player player)
    {
        bool itemFound = false;
        bool missingItem = false;
        string tempItem = "";

        //Player can pickup any item in CurrentRoom's itemlist

        int itemIndex = 0;

        //Use String.Split method to split verb/items based on blank space (" ")
        string[] arrayWords = playerAction.Split(" ");

        //Split player input based on using blank space(" ") as a marker
        //Note: This means that currently this routine only accepts 1 blank space, i.e. "old newspaper" will fail
        //
        //Check if blank space (" ") can actually be found in the input string
        //or else we would get an ArgumentOutOfRangeException when assigning itemIndex
        // if (playerAction.IndexOf(" ")! > -1)
        //     itemIndex = playerAction.IndexOf(" ");
        // else
        // {
        //     //inform user if input is missing a string (or item in this case). e.g. 'get card'/get keycard
        //     Console.WriteLine("What do you want to get?");
        //     missingItem = true;
        //     break;
        // }

        //remove any whitespace
        tempItem = playerAction.Substring(itemIndex).Trim();

        //Extract the word from index 0 to before the blank space, in this case it's 'get'.
        //We don't currently use it but might later on
        //string get = playerAction.Substring(0, itemIndex);

        // foreach (var item in player.CurrentRoom.Items)
        //     Console.WriteLine(item.Key);

        //if player input word is the same as an item in CurrentRoom
        //we add this item to inventory and remove it from CurrentRoom's itemlist
        string removeRoomItem = "";

        foreach (var item in player.CurrentRoom.Items)
        {
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

            if (arrayWords[1].ToLower() == item.Key)
            {
                removeRoomItem = item.Key;
                player.Inventory.Add(item.Key);

                //Remove item from CurrentRoom's itemlist                
                player.CurrentRoom.Items.Remove(removeRoomItem);

                Console.WriteLine($"You pick up {removeRoomItem}.");
                itemFound = true;
                break;
            }
        }

        //Inform user if item is not in CurrentRoom's itemlist
        if (!itemFound && missingItem == false)
            Console.WriteLine($"There is no '{arrayWords[1]}' to pick up!\n");
    }//End of GetItem
}//End of class Player