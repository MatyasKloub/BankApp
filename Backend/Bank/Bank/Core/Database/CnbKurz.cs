namespace Bank.Core.Database
{
    public static class CnbKurz
    {
        public static async Task<bool> newKurz()
        {
            string url = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";
            string filePath = "kurz.txt";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string content = await response.Content.ReadAsStringAsync();

                    // Save the content to a file
                    await File.WriteAllTextAsync(filePath, content);

                    Console.WriteLine("File saved successfully.");
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return false;
                }
            }
        }
    }
}
