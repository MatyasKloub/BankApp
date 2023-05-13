using Bank.Core.Database;

namespace Bank.Core.Authorization
{
    public static class DbAction
    {
        public static bool CreateUser(User user)
        {





            return false;
        }

        private static bool userExists(string name)
        {
            using (var context = new MyDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Name == name);
                return user != null;
            }
        }

    }
}
