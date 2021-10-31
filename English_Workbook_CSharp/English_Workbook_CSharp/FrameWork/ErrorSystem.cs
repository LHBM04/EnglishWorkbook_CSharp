using System;

namespace ErrorSystem
{
    public enum errorType
    {
        DUMMY = 0, NOTFILE = 1, FAILOPEN = 2,
        FAILREAD = 3, FAILWHILEREAD = 4, FAILCLOSE = 5, 
        FAILQUE = 6, EMPTYFILE = 7,
    }

    public class ErrorManager
    { 
        public static readonly string header = "[오류 발생!] ";

        private static readonly string[] errorCollections = new string[]
        {
            header + "Dummy Error",
            header + "파일이 존재하지 않습니다!",
            header + "파일이 열리지 않습니다!",
            header + "파일을 읽어오지 못했습니다!",
            header + "파일을 읽어오는 도중 실패했습니다!",
            header + "파일이 닫히지 않습니다!",
            header + "문제가 존재할 수 없는 형식입니다!",
            header + "파일이 비어있습니다!",
        };

        private static void Error_Pathfinder(string filePath)
        {
            Console.WriteLine("(자세한 오류는 오류지점을 참조해주세요.)");
            Console.WriteLine("<오류지점 : {0}>", filePath);
        }

        public static void Oops_Massage(errorType type)
        {
            Console.WriteLine("{0}\n", errorCollections[(int)type]);
        }

        public static void Oops_Massage(errorType type, string filePath)
        {
            Console.WriteLine("{0}\n", errorCollections[(int)type]);
            Error_Pathfinder(filePath);
        }
    }
}
