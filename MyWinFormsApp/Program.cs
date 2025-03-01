namespace MyWinFormsApp
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new WatchForm());


            //ConsoleAppInstance.Default.Run(args, true);


        }
    }
}
