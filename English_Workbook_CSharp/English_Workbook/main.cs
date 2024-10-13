using static Scene.sceneCollections;

namespace Main
{
    public class Program_Info
    {
        private const string Program_Name = "English_Workbook";
        private const string Version = "BETA 1.0";

        public static string program_Name { get { return Program_Name; } }
        public static string version { get { return Version; } }
    }

    public class main
    {
        static void Main(string[] args)
        {
            Scenes[0].MainMenu();
        }
    }
}
