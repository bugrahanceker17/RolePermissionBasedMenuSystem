namespace RoleBasedMenuSystem.Core.Entities;

public abstract class EnumBase : IComparable
{
    public string Name { get; set; }
    public int Id { get; set; }
    public Guid? UniqueId { get; set; }
    public string? Code { get; set; }

    protected EnumBase(int id, string name)
    {
        Id = id;
        Name = name;
    }

    protected EnumBase(int id, string name, Guid unique)
    {
        Id = id;
        Name = name;
        UniqueId = unique;
    } 
    
    protected EnumBase(int id, string name, string code)
    {
        Id = id;
        Name = name;
        Code = code;
    } 
    
    protected EnumBase(string name, string code)
    {
        Name = name;
        Code = code;
    }
        
    public int CompareTo(object obj) => Id.CompareTo(((EnumBase)obj).Id);
}