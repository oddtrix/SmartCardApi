namespace SmartCardApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            hostBuilder(args).Build().Run();    
        }

        public static IHostBuilder hostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults
                (webBuilder =>
                {
                    webBuilder.UseStartup(nameof(SmartCardApi));
                });
        }
    }
}