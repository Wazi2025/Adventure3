public class Room
{
    public int allItems = 0;
    //Name of the room
    public string Name { get; set; }

    //Room description
    public string Description { get; set; }

    //List of items (if any) in room
    //public List<string> Items { get; set; }
    public Dictionary<string, Room> Items { get; set; }

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
    }

    // Adds an exit from this room to another
    public void AddExit(string playerAction, Room targetRoom)
    {
        Exits[playerAction.ToLower()] = targetRoom;

        NumberOfExits.Add(playerAction);
    }
}//End of class Room