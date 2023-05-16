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

                User usertemp = ReturnUserDataObj(user.Email);
                CreateUcet(usertemp.Id, "CZK", "10000.00");
                CreateUcet(usertemp.Id, "USD", "10000.00");
                CreateUcet(usertemp.Id, "AUD", "10000.00");
                CreateUcet(usertemp.Id, "CAD", "10000.00");
                
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
                    uz.Id = us.Id;

                    string json = JsonConvert.SerializeObject(uz);

                    return json;
                }
                else return null;
            }
           
        }
        public static User ReturnUserDataObj(string email)
        {
            using (var context = new MyDbContext())
            {
                var us = context.Users.FirstOrDefault(u => u.Email == email);

                if (us != null)
                {
                    return us;

                }
                else return null;
            }
        }
        public static bool CreateUcet(int? uid, string mena, string val)
        {
            using (var db = new MyDbContext())
            {
                try
                {
                    db.Ucty.Add(new Ucet(uid, mena, val));
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }                
            }
        }
        public static string GetUcty(string email)
        {
            using (var context = new MyDbContext())
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
        public static List<Ucet> GetUctyObj(string email)
        {
            using (var context = new MyDbContext())
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
                return new Kurz(ex.Message, 2.01f);
                Console.WriteLine($"An error occurred in getKurz(): {ex.Message}");
            }
        }

        private static bool isPaymentPossible(string zkratka, int value, string email)
        {
            List<Ucet>ucty = DbAction.GetUctyObj(email);
            if (ucty == null)
            {
                return false;
            }
            Ucet ucet = new Ucet();
            foreach (var item in ucty)
            {
                if (item.Mena == zkratka)
                {
                    
                    if (float.Parse(item.Count) >= value)
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

            float hodnota_korun = float.Parse(ucet.Count);

            if (hodnota_korun >= value*kurz.Hodnota)
            {
                return true;
            }
            return false;

        }
        public static bool doPayment(string zkratka, int value, string email, string typ)
        {
            Kurz kurz = GetKurz(zkratka);
            if (typ == "prichozi")
            {
                List<Ucet> ucty = GetUctyObj(email);

                foreach (var item in ucty)
                {
                    if (item.Mena == zkratka)
                    {
                        using (var context = new MyDbContext())
                        {
                            var ucet = context.Ucty.FirstOrDefault(u => u.Id == item.Id);

                            if (ucet != null)
                            {
                                float? finalHodnota = float.Parse(ucet.Count) + (value);

                                ucet.Count = finalHodnota.ToString();
                                
                                context.SaveChanges();

                                createPlatba(email, value, zkratka, typ);
                                return true;
                            }
                            else return false;
                        }
                    }   
                }
                using (var context = new MyDbContext())
                {
                    foreach (var item in ucty)
                    {
                        if (item.Mena == "CZK")
                        {
                            var ucet = context.Ucty.FirstOrDefault(u => u.Id == item.Id);
                            float? finalHodnota = float.Parse(ucet.Count) + (value * kurz.Hodnota);
                            ucet.Count = finalHodnota.ToString();
                            
                            
                            context.SaveChanges();
                            createPlatba(email, value, zkratka, typ);
                            return true;
                        }
                    }
                }

                return true;
            }
            if (isPaymentPossible(zkratka, value, email))
            {
                List<Ucet> ucty = DbAction.GetUctyObj(email);
                if (ucty == null)
                {
                    return false;
                }
                Ucet ucet = new Ucet();
                foreach (var item in ucty)
                {
                    if (item.Mena == zkratka)
                    {
                        if (float.Parse(item.Count) >= value)
                        {
                            using (var context = new MyDbContext())
                            {
                                try
                                {
                                    // Retrieve the user entity with ID 8
                                    var uc = context.Ucty.FirstOrDefault(u => u.UserId == ReturnUserDataObj(email).Id && u.Mena == zkratka);

                                    if (uc != null)
                                    {
                                        // Update the email property
                                        float hodnota = float.Parse(item.Count);
                                        float finalVal = hodnota - value;
                                        float finalfinalVal = (float)Math.Round(finalVal, 2);
                                        uc.Count = finalfinalVal.ToString();
                                        
                                        // Save the changes to the database
                                        context.SaveChanges();
                                        createPlatba(email, value, zkratka, typ);
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
                float hodnota_korun = float.Parse(ucet.Count);
                float? hodnota_v_korunach = value * kurz.Hodnota;
                using (var context = new MyDbContext())
                {
                    try
                    {
                        // Retrieve the user entity with ID 8
                        var uc = context.Ucty.FirstOrDefault(u => u.UserId == ReturnUserDataObj(email).Id && u.Mena == "CZK");

                        if (uc != null)
                        {
                            float fin = hodnota_korun - hodnota_v_korunach ?? 0.00f;
                            float zaokrouhleno = (float)Math.Round(fin, 2);
                            uc.Count = zaokrouhleno.ToString();

                            createPlatba(email, value, zkratka, typ);
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
                return false;
            }
            return false;
        }
        public static bool createPlatba(string email, int value, string zkratka, string typ)
        {
            string add = "+";

            if (typ == "odchozi")
            {
                add = "-";
            }

            User usertemp = ReturnUserDataObj(email);
            using (var db = new MyDbContext())
            {
                Platba platba = new Platba((int)usertemp.Id, zkratka, add + value.ToString());
                db.Platba.Add(platba);
                db.SaveChanges();
            }

            return true;
        }
        public static string getPlatby(string email)
        {
            User user = ReturnUserDataObj(email);
            string returnal = "Provedené platby na účtu:<br>";
            using (var context = new MyDbContext())
            {
                var platby = context.Platba.Where(u => u.from == user.Id);
                if (platby != null)
                {
                    foreach (var item in platby)
                    {
                        returnal += "<p>ID platby: " + item.Id + ", platba:  " + item.Value + item.Currency + "</p>";
                    }
                }
                return returnal;
            }
        }
    }
}
