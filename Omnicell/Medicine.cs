namespace Omnicell
{
    public class Medicine
    {
        public string Id { get; }
        public string Name { get; }

        public Medicine(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
