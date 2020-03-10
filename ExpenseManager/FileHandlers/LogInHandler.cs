using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace ExpenseManager.FileHandlers
{
    class LogInHandler
    {
        readonly string path = @"usersData.json";
        public List<User> Users { get; set; } = new List<User>();

        public async Task LoadData()
        {
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("[]");
                }
            }
            using (StreamReader file = await Task.Run(() => File.OpenText(path)))
            {
                JsonSerializer serializer = new JsonSerializer();
                Users = await Task.Run(() => (List<User>)serializer.Deserialize(file, typeof(List<User>)));
            }
            //Console.WriteLine("Log in file loaded.");
        }

        public async Task SaveData()
        {
            using (StreamWriter file = await Task.Run(() => File.CreateText(path)))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, Users);
            }
            //Console.WriteLine("Log in file saved.");
        }

        //Validate username and password
        public User Validate(string name, string pass) {
            foreach (User user in Users)
            {
                if (user.Name == name) {
                    if (user.Password == pass) {
                        return user;
                    }
                    Console.WriteLine("ERROR: Invalid password.");
                    return null;
                }
            }
            Console.WriteLine("ERROR: Invalid user name.");
            return null;
        }

        //Add new user
        public bool Register(string name, string pass) {
            foreach (User user in Users)
            {
                if (user.Name == name)
                {
                    Console.WriteLine("ERROR: Name exist.");
                    return false;
                }
            }
            Users.Add(new User(name, pass));
            Console.WriteLine("INFO: New user registered.");
            return true;
        }
    }
}
