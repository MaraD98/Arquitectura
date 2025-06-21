namespace API
{
    public class Program
    {
        protected Program()
        {
        }
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(config => { config.UseStartup<Startup>(); });
    }
}