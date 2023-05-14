using Bank.Core.Database;
using Newtonsoft.Json;

namespace Bank.Core.Authorization
{
    public static class DbAction
    {
        public static bool CreateUser(User user)
        {
            if (!userExists(user.Email))
            {
                 using (var db = new MyDbContext())
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                }
                 return true;
            }
            return false;
        }

        private static bool userExists(string email)
        {
            using (var context = new MyDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Email == email);
                return user != null;
            }
        }

        public static bool UserExistsAndRight(User user)
        {
            using (var context = new MyDbContext())
            {
                var us = context.Users.FirstOrDefault(u => u.Email == user.Email);

                if (us != null)
                {
                    if (us.Password == user.Password)
                    {
                        return true;
                    }
                    else return false;
                }
                else return false; 
            }
        }
        
        public static string ReturnUserData(User user)
        {
            User uz = new User("non", "non", user.Email);

            using (var context = new MyDbContext())
            {
                var us = context.Users.FirstOrDefault(u => u.Email == user.Email);

                if (us != null)
                {
                    uz.Name = us.Name;


                    string json = JsonConvert.SerializeObject(uz);

                    return json;
                
                }
                else return null;
            }
           
        }
    }
}
