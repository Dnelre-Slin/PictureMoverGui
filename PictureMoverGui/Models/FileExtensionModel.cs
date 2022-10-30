namespace PictureMoverGui.Models
{
    public class FileExtensionModel
    {
        public string Name { get; }
        public int Count { get; }
        public bool Active { get; }

        public FileExtensionModel(string name, int count, bool active)
        {
            Name = name;
            Count = count;
            Active = active;
        }
    }
}
