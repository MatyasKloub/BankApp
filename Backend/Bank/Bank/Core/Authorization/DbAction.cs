using Bank.Core.Database;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Bank.Core.Authorization
{
    public static class DbAction
    {
        private static DbContextOptions<MyDbContext> options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=mydatabase.db").Options;
        private static MyDbContext context = new MyDbContext(options);
        
        public static bool CreateUser(User user, DbContextOptions<MyDbContext> customOptions)
        {
            if (customOptions == null)
            {
                customOptions = options;
            }
            context.CreateDatabaseAndTables();
            if (!userExists(user.Email, customOptions))
            {
                using (var db = new MyDbContext(customOptions))
                {
                   db.Users.Add(user);
                   db.SaveChanges();
                }

                User usertemp = ReturnUserDataObj(user.Email, customOptions);
                CreateUcet(usertemp.Id, "CZK", "10000.00", customOptions);
                CreateUcet(usertemp.Id, "USD", "10000.00", customOptions);
                CreateUcet(usertemp.Id, "AUD", "10000.00", customOptions);
                CreateUcet(usertemp.Id, "CAD", "10000.00", customOptions);
                
                return true;
            }
            return false;
        }

        public static bool userExists(string email, DbContextOptions<MyDbContext> customOptions)
        {
            context.CreateDatabaseAndTables();
            using (var context = new MyDbContext(customOptions))
            {
                var user = context.Users.FirstOrDefault(u => u.Email == email);
                return user != null;
            }
        }

        public static bool UserExistsAndRight(User user, DbContextOptions<MyDbContext> customOptions)
        {
            context.CreateDatabaseAndTables();
            if (customOptions == null)
            {
                customOptions = options;
            }

            using (var context = new MyDbContext(customOptions))
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
        
        public static string ReturnUserData(User user, DbContextOptions<MyDbContext> customOptions)
        {
            User uz = new User("non", "non", user.Email);

            if (customOptions == null)
            {
                customOptions = options;
            }
            if (customOptions == null)
            {
                customOptions = options;
            }    
            using (var context = new MyDbContext(customOptions))
            {
                var us = context.Users.FirstOrDefault(u => u.Email == user.Email);

                if (us != null)
                {
                    uz.Name = us.Name;
                    uz.Id = us.Id;

                    string json = JsonConvert.SerializeObject(uz);

                    return json;
                }
                else return null;
            }
           
        }
        public static User ReturnUserDataObj(string email, DbContextOptions<MyDbContext> customOptions)
        {
            
            using (var context = new MyDbContext(customOptions))
            {
                var us = context.Users.FirstOrDefault(u => u.Email == email);

                if (us != null)
                {
                    return us;

                }
                else return null;
            }
        }
        public static bool CreateUcet(int? uid, string mena, string val, DbContextOptions<MyDbContext> customOptions)
        {
            if (customOptions == null)
            {
                customOptions = options;
            }
            using (var db = new MyDbContext(customOptions))
            {
                db.Ucty.Add(new Ucet(uid, mena, val));
                db.SaveChanges();
                return true;
            }
        }
        public static string GetUcty(string email, DbContextOptions<MyDbContext> customOptions)
        {
            if (customOptions == null)
            {
                customOptions = options;
            }
            using (var context = new MyDbContext(customOptions))
            {
                var user = context.Users.FirstOrDefault(u =>u.Email == email);
                if (user == null)
                {
                    return null;
                }
                var ucty = context.Ucty.Where(u => u.UserId == user.Id).ToList();

                if (ucty != null)
                {
                    string returnal = "";

                    foreach (var u in ucty)
                    {
                        returnal += "<h3>" + u.Mena + ": " + u.Count +"</h3>";
                    }


                    return returnal;

                }
                else return null;
            }
        }
        public static List<Ucet> GetUctyObj(string email, DbContextOptions<MyDbContext> customOptions)
        {
            if (customOptions == null)
            {
                customOptions=options;
            }
            using (var context = new MyDbContext(customOptions))
            {
                var user = context.Users.FirstOrDefault(u => u.Email == email);
                if (user == null)
                {
                    return null;
                }
                var ucty = context.Ucty.Where(u => u.UserId == user.Id).ToList();

                if (ucty != null)
                {
                    return ucty;
                }
                else return null;
            }
        }
        public static string GetMeny()
        {
            string returnal = "";
            try
            {
                string[] lines = System.IO.File.ReadAllLines("kurz.txt");

                // Skip the first two rows
                for (int i = 2; i < lines.Length; i++)
                {
                    string line = lines[i];
                    string[] columns = line.Split('|');

                    if (columns.Length >= 5)
                    {
                        string currency = columns[3];
                        returnal += "<option value=\"" + currency + "\">" + currency + "</option>";
                    }
                }
                returnal += "<option value=\"CZK\">CZK</option>";
                return returnal;
                Console.WriteLine("Currencies loaded successfully.");
            }
            catch (Exception ex)
            {
                return null;
                Console.WriteLine($"An error occurred in GetMeny(): {ex.Message}");
            }
        }

        public static Kurz GetKurz(string zkratka)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines("kurz.txt");
                Kurz kurz = new Kurz();
                float result;
                // Skip the first two rows
                for (int i = 2; i < lines.Length; i++)
                {
                    string line = lines[i];
                    string[] columns = line.Split('|');

                    if (columns.Length >= 5)
                    {
                        if (columns[3] == zkratka)
                        {
                            string currency = columns[4];
                            if (float.TryParse(currency.Replace(",", "."), out result))
                            {
                                if (columns[2] != "1")
                                {
                                    int kurs = int.Parse(columns[2]);

                                    result = result / kurs;
                                }
                                kurz.Hodnota = result;
                                kurz.Zkratka = columns[3];
                                return kurz;
                            }
                            else
                            {
                                Console.WriteLine("Nefungovalo to lool");
                                return null;
                            }
                        }
                    }
                }
                return null; // toto by nemělo nikdy nastat
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool isPaymentPossible(string zkratka, int value, string email, DbContextOptions<MyDbContext> customOptions)
        {
            if (customOptions == null)
            {
                customOptions = options;
            }

            List<Ucet>ucty = DbAction.GetUctyObj(email, customOptions);
            if (ucty == null)
            {
                return false;
            }
            Ucet ucet = new Ucet();
            foreach (var item in ucty)
            {
                if (item.Mena == zkratka)
                {
                    
                    if ((float.Parse(item.Count)*1.1f) >= value)
                    {
                        return true;
                    }    
               
                }
                if (item.Mena == "CZK")
                {
                    ucet.Count = item.Count;
                }
            }
            Kurz kurz = GetKurz(zkratka);

            float hodnota_korun = float.Parse(ucet.Count) * 1.1f;

            // CZK
            if (kurz == null && zkratka == "CZK")
            {
                if (hodnota_korun >= value)
                {
                    return true;
                }
            }
            else
            {
                if (hodnota_korun >= value*kurz.Hodnota)
                {
                    return true;
                }
            }
            return false;

        }
        
        public static bool doPayment(string zkratka, int value, string email, string typ, DbContextOptions<MyDbContext> customOptions)
        {
            if (customOptions == null)
            {
                customOptions = options;
            }
            Kurz kurz = GetKurz(zkratka);
            if (typ == "prichozi")
            {
                List<Ucet> ucty = GetUctyObj(email, customOptions);

                foreach (var item in ucty)
                {
                    if (item.Mena == zkratka)
                    {
                        using (var context = new MyDbContext(customOptions))
                        {
                            var ucet = context.Ucty.FirstOrDefault(u => u.Id == item.Id);

                            if (ucet != null)
                            {
                                float? finalHodnota = float.Parse(ucet.Count) + (value);

                                ucet.Count = finalHodnota.ToString();
                                
                                context.SaveChanges();

                                createPlatba(email, value, zkratka, typ, customOptions);
                                return true;
                            }
                            else return false;
                        }
                    }   
                }
                using (var context = new MyDbContext(customOptions))
                {
                    foreach (var item in ucty)
                    {
                        if (item.Mena == "CZK")
                        {
                            var ucet = context.Ucty.FirstOrDefault(u => u.Id == item.Id);
                            float? finalHodnota = float.Parse(ucet.Count) + (value * kurz.Hodnota);
                            ucet.Count = finalHodnota.ToString();
                            
                            
                            context.SaveChanges();
                            createPlatba(email, value, zkratka, typ, customOptions);
                            return true;
                        }
                    }
                }

                return true;
            }
            if (isPaymentPossible(zkratka, value, email, customOptions))
            {
                List<Ucet> ucty = DbAction.GetUctyObj(email, customOptions);
                if (ucty == null)
                {
                    return false;
                }
                Ucet ucet = new Ucet();
                foreach (var item in ucty)
                {
                    if (item.Mena == zkratka)
                    {
                        if (float.Parse(item.Count)*1.1f >= value)
                        {
                            using (var context = new MyDbContext(customOptions))
                            {
                                try
                                {
                                    
                                    var uc = context.Ucty.FirstOrDefault(u => u.UserId == ReturnUserDataObj(email, customOptions).Id && u.Mena == zkratka);

                                    if (uc != null)
                                    {
                                        // Update the email property
                                        float hodnota = float.Parse(item.Count);
                                        float finalVal = hodnota - value;
                                        float finalfinalVal = (float)Math.Round(finalVal, 2);
                                        uc.Count = finalfinalVal.ToString();
                                        
                                        // Save the changes to the database
                                        context.SaveChanges();
                                        createPlatba(email, value, zkratka, typ, customOptions);
                                        return true;
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("chyba při platbě, stejná měna: " + e.Message);
                                    return false;
                                }

                            }
                        }
                    }
                    if (item.Mena == "CZK")
                    {
                        ucet.Count = item.Count;
                    }
                }
                float hodnota_korun = float.Parse(ucet.Count)*1.1f;
                float? hodnota_v_korunach = value * kurz.Hodnota;
                using (var context = new MyDbContext(customOptions))
                {
                    try
                    {
                        var uc = context.Ucty.FirstOrDefault(u => u.UserId == ReturnUserDataObj(email, customOptions).Id && u.Mena == "CZK");

                        if (uc != null)
                        {
                            float fin = hodnota_korun - hodnota_v_korunach ?? 0.00f;
                            float zaokrouhleno = (float)Math.Round(fin, 2);
                            uc.Count = zaokrouhleno.ToString();

                            createPlatba(email, value, zkratka, typ, customOptions);
                            // Save the changes to the database
                            context.SaveChanges();
                            return true;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("chyba při platbě, převod v českých!: " + e.Message);
                        return false;
                    }

                }
            }
            return false;
        }
        public static bool createPlatba(string email, int value, string zkratka, string typ, DbContextOptions<MyDbContext> customOptions)
        {
            if (customOptions == null)
            {
                customOptions = options;
            }


            string add = "+";

            if (typ == "odchozi")
            {
                add = "-";
            }

            User usertemp = ReturnUserDataObj(email, customOptions);
            using (var db = new MyDbContext(customOptions))
            {
                Platba platba = new Platba((int)usertemp.Id, zkratka, add + value.ToString());
                db.Platba.Add(platba);
                db.SaveChanges();
                
            }
            return true;
        }
        public static string getPlatby(string email, DbContextOptions<MyDbContext> customOptions)
        {
            if (customOptions == null)
            {
                customOptions = options;
            }
            User user = ReturnUserDataObj(email, customOptions);
            string returnal = "Provedené platby na účtu:<br>";
            using (var context = new MyDbContext(customOptions))
            {
                try
                {
                    var platby = context.Platba.Where(u => u.from == user.Id);
                    foreach (var item in platby)
                    {
                        returnal += "<p>ID platby: " + item.Id + ", platba:  " + item.Value + item.Currency + "</p>";
                    }
                    return returnal;
                }
                catch (Exception)
                {
                    return null;
                    
                }  
            }
        }
    }
}
