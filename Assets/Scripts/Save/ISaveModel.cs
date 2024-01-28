namespace Save
{
    public interface ISaveModel
    {
        void SaveCurrentShipId(string index);
        void Save();
        void Deserialize();
    }
}