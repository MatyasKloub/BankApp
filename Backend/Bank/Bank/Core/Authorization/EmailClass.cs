using Newtonsoft.Json;

namespace Bank.Core.Authorization
{
    public class EmailClass
    {
        public string? email { get; set; }
        public string? password { get; set; }

        public void setClass()
        {
            string filePath = "./email.txt";
            string[] lines = File.ReadAllLines(filePath);

            this.email = lines[0];
            this.password = lines[1];
        }
    }
}
