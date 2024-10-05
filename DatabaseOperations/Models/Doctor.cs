namespace DatabaseOperations.Models
{
    public class Doctor
    {
        public Doctor(string name)
        {
            Name=name;
        }

        public int Id { get; set; }
        public string Name { get; set; }    
    }
}
