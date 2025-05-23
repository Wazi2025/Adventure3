namespace Adventure3.classes;

public class Room
{
    //Name of the room
    public string Name { get; set; }

    //Room description
    public string Description { get; set; }

    //List of items (if any) in room    
    public Dictionary<string, Room> Items { get; set; }

    public Dictionary<string, string> Puzzles { get; set; }

    //List of number of exits in room
    public List<string> NumberOfExits { get; set; }

    public Dictionary<string, Room> Exits { get; set; }

    public Room(string name, string description)
    {
        Name = name;
        Description = description;

        Exits = new Dictionary<string, Room>();
        Items = new Dictionary<string, Room>();
        NumberOfExits = new List<string>();
        Puzzles = new Dictionary<string, string>();

        InitializePuzzles();
    }

    // Adds an exit from this room to another
    public void AddExit(string playerAction, Room targetRoom)
    {
        Exits[playerAction.ToLower()] = targetRoom;

        NumberOfExits.Add(playerAction);
    }

    void InitializePuzzles()
    {
        //Note: Prolly will have to create some sort of "puzzle" class instead
        Puzzles.Add("blaster", "Docking Bay");
        Puzzles.Add("keycard", "Bridge");
    }
}//End of class Room