using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

using Example;
using static Example.examCollections;
using static Example.exampleType;

using ErrorSystem;
using static ErrorSystem.errorType;

using ToolSystem;

namespace FileSystem
{
    public class ExamGenerator
    {
        private static readonly string path_Word = @"\Word\";
        private static readonly string path_Iverb = @"\Iverb\";
        private static readonly string path_Proverb = @"\Proverb\";

        public static void Reset_Exam(ref List<example>exams)
        {
            if(exams != null)
            {
                exams.Clear();
                exams.Capacity = 0;
            }
        }

        public static bool Generate_Exam(ref List<example> exams, exampleType type)
        {
            Reset_Exam(ref exams);
            string filePath = Directory.GetCurrentDirectory() + @"\StudyFile\";
            string[] Lines_Q, Lines_S;

            switch (type)
            {
                case WORD:
                    filePath += path_Word;
                    break;
                case IVERB:
                    filePath += path_Iverb;
                    break;
                case PROVERB:
                    filePath += path_Proverb;
                    break;
                case DEFAULT:
                default:
                    ErrorManager.Oops_Massage(FAILQUE);
                    ToolManager.Press_Key(ConsoleKey.Enter);
                    return false;
            }

            if (Directory.Exists(filePath))
            {
                if (File.Exists(filePath + "Que.txt") && File.Exists(filePath + "Sol.txt"))
                {
                    Lines_Q = File.ReadAllLines(filePath + "Que.txt", Encoding.Default);
                    Lines_S = File.ReadAllLines(filePath + "Sol.txt", Encoding.Default);

                    if (Lines_Q.Length != Lines_S.Length)
                        ErrorManager.Oops_Massage(FAILWHILEREAD, filePath + "Que.txt 또는 Sol.txt");

                    int Size = (Lines_Q.Length + Lines_S.Length) / 2;

                    for (int i = 0; i < Size; ++i)
                        exams.Add(new example(Lines_Q[i], Lines_S[i].Split(";"), type));
                    return true;
                }

                else
                {
                    ErrorManager.Oops_Massage(NOTFILE, filePath + "Que.txt 또는 Sol.txt");
                    ToolManager.Press_Key(ConsoleKey.Enter);
                    return false;
                }
            }

            else
            {
                ErrorManager.Oops_Massage(NOTFILE, filePath);
                ToolManager.Press_Key(ConsoleKey.Enter);
                return false;
            }
        }

        public static bool Generate_All_Exam(ref List<example> exams)
        {
            Reset_Exam(ref exams);
            List<example> temp = new List<example>();

            if (Generate_Exam(ref temp, WORD))
                exams.AddRange(temp);
            else { return false; }

            if (Generate_Exam(ref temp, IVERB))
                exams.AddRange(temp);
            else { return false; }

            if (Generate_Exam(ref temp, PROVERB))
                exams.AddRange(temp);
            else { return false; }

            Shuffle_Exam(ref exams);

            return true;
        }

        public static void Generate_All_Review(ref List<example> Temp)
        {
            Temp.AddRange(WrongWord);
            Temp.AddRange(WrongIverb);
            Temp.AddRange(WrongProverb);

            Shuffle_Exam(ref Temp);
        }

        public static void Shuffle_Exam(ref List<example> exams)
        {
            Console.Write("[문제의 출제 순서에 대한 방식을 골라주세요.]\n");
            Console.Write("[1]. 섞는다(랜덤)  [2]. 순번대로(저장 순서)");

        CHOOSE_WAY:
            if (ToolManager.isPressKey(ConsoleKey.NumPad1, ConsoleKey.D1))
            {
                var Copy = exams;
                var rnd = new Random();
                exams = Copy.OrderBy(Copy => rnd.Next()).ToList();
            }

            else if (ToolManager.isPressKey(ConsoleKey.NumPad2, ConsoleKey.D2)) { return; }

            else goto CHOOSE_WAY;
        }

        public static List<example> Shuffle_Exam(List<example>exams)
        {
            Console.Write("[문제의 출제 순서에 대한 방식을 골라주세요.]\n");
            Console.Write("[1]. 섞는다(랜덤)  [2]. 순번대로(저장 순서)");

        CHOOSE_WAY:
            if (ToolManager.isPressKey(ConsoleKey.NumPad1, ConsoleKey.D1))
            {
                var rnd = new Random();
                exams.OrderBy(exam => rnd.Next());
            }

            else if (ToolManager.isPressKey(ConsoleKey.NumPad2, ConsoleKey.D2)) { }

            else goto CHOOSE_WAY;

            return exams;
        }
    }
}
