using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;

using Example;
using static Example.exampleType;
using static Example.examCollections;

using FileSystem;
using ToolSystem;

namespace StudySystem
{
    public class Manager_Surface
    {
        protected static string Answer = "Dummy Answer";
        protected static byte Turn = 1;
        protected static byte Right = 0;
        protected static byte Wrong = 0;

        protected virtual void Notice_Exam(int Problems, string ExamType) { }
        protected virtual void Notice_Exam(int Problems) { }
        protected virtual void Reset_MemberValue() { }

        public virtual void Check_Workbook() { }
        public virtual void Check_Reviewbook() { }
        public virtual void Exam_Word(exampleType type = WORD) { }
        public virtual void Exam_Iverb(exampleType type = IVERB) { }
        public virtual void Exam_Proverb(exampleType type = PROVERB) { }
        public virtual void Exam_All() { }
    }

#if DEBUG
    public class Debugger
    {
        public static void Mining_Solutions(string[] Solutions)
        {
            Console.Write("정답 : ");
            foreach (var Solution in Solutions)
                Console.Write("[{0}] ", Solution);

            Console.Write("\n");
            return;
        }

        public static void All_Exam_Wrong(ref byte Wrong, List<example> Workbook, ref List<example> WrongNote)
        {
            foreach(var exam in Workbook)
                ScoreManager.Add_WrongNote(ref Wrong, exam, ref WrongNote);

            Wrong = (byte)Workbook.Count;
        }

        public static void All_Exam_Pass(ref byte Right, List<example> Workbook, ref List<example> WrongNote)
        {
            foreach (var exam in Workbook)
                ScoreManager.Erase_WrongNote(ref Right, exam, ref WrongNote);

            Right = (byte)Workbook.Count;
        }
    }
#endif

    public class ScoreManager
    {
        public static bool isRightAnswer(string[] Solutions, string Answer)
        {
            foreach(var Solution in Solutions)
            {
                if (Solution == Answer)
                    return true;
                    
            }

            return false;
        }

        public static void Add_WrongNote(ref byte Wrong, example exam, ref List<example> WrongNote)
        {
            ++Wrong;
            bool Trigger = false;

            foreach (var exam2 in WrongNote)
                if (exam == exam2)
                    Trigger = true;

            if (Trigger == false)
            {
                WrongNote.Add(exam);
                Console.Write("오답이므로 오답노트에 해당 문제가 추가됩니다!\n");
            }
        }

        public static void Erase_WrongNote(ref byte Right, example exam, ref List<example> WrongNote)
        {
            ++Right;
            WrongNote.RemoveAll(target => (exam == target));
        }
    }

    public class ExamManager : Manager_Surface
    {
        protected override void Notice_Exam(int Problems, string ExamType)
        {
            Console.Clear();

            Console.Write("[{0}문제 {1}선!]\n", ExamType, Problems);
            Console.Write("(Enter를 누르시면 시험을 시작합니다...)");

            Console.Write("\n\n\n<주의 사항>\n");
            Console.Write("★(입력은 모두 한 줄로 받으며, 띄어쓰기를 구분합니다.)\n");
            Console.Write("★(사용자가 정의한 txt파일을 통해 정답을 판단합니다.)\n");
            Console.Write("★(사용 중에 txt파일을 건드릴 경우, 오류가 날 수 있습니다.)");

            ToolManager.Press_Key(ConsoleKey.Enter);
        }

        protected override void Notice_Exam(int Problems)
        {
            Console.Clear();

            Console.Write("[문제 대난투 {0}선!]\n", Problems);
            Console.Write("(Enter를 누르시면 시험을 시작합니다...)");

            Console.Write("\n\n\n<주의 사항>\n");
            Console.Write("★(해당 모드에서는 틀려도 오답노트에 기록되지 않습니다.)\n");
            Console.Write("★(또한 문제를 맞아도 오답노트에서 지워지지 않습니다.)\n");
            Console.Write("★(입력은 모두 한 줄로 받으며, 띄어쓰기를 구분합니다.)\n");
            Console.Write("★(사용자가 정의한 txt파일을 통해 정답을 판단합니다.)\n");
            Console.Write("★(사용 중에 txt파일을 건드릴 경우, 오류가 날 수 있습니다.)");

            ToolManager.Press_Key(ConsoleKey.Enter);
        }

        protected override void Reset_MemberValue()
        {
            Answer = "Dummy Answer";
            Turn = 1;
            Right = 0;
            Wrong = 0;
        }

        public override void Check_Workbook()
        {
            int Count = 1;
            string filePath = Directory.GetCurrentDirectory() + @"\StudyFile\";
            if (!ExamGenerator.Exists_AllFile(filePath))
            {
                ToolManager.Press_Key(ConsoleKey.Enter);
                return;
            }

            List<example> examples = new List<example>();

            if (!ExamGenerator.Generate_Exam(ref examples, WORD))
                return;

            Console.Write("[단어 목록]\n");
            foreach (var example in examples)
            {
                if (example.solutions.Length == 1)
                    Console.Write("[{0}]. {1} : {2}\n", Count, example.question, example.solutions[0]);

                else
                    Console.Write("[{0}]. {1} : {2}...\n", Count, example.question, example.solutions[0]);

                ++Count;
            }

            if (!ExamGenerator.Generate_Exam(ref examples, IVERB))
                return;

            Console.Write("\n[불규칙동사 목록]\n");
            Count = 1;

            foreach (var example in examples)
            {
                if (example.solutions.Length == 1)
                    Console.Write("[{0}]. {1} : {2}\n", Count, example.question, example.solutions[0]);

                else
                    Console.Write("[{0}]. {1} : {2}...\n", Count, example.question, example.solutions[0]);

                ++Count;
            }

            if (!ExamGenerator.Generate_Exam(ref examples, PROVERB))
                return;

            Console.Write("\n[불규칙동사 목록]\n");
            Count = 1;

            foreach (var example in examples)
            {
                if (example.solutions.Length == 1)
                    Console.Write("[{0}]. {1} : {2}\n", Count, example.question, example.solutions[0]);

                else
                    Console.Write("[{0}]. {1} : {2}...\n", Count, example.question, example.solutions[0]);

                ++Count;
            }

            Console.Write("\n(Enter를 누르면 메뉴로 돌아갑니다...)");
            ToolManager.Press_Key(ConsoleKey.Enter);
        }

        public override void Exam_All()
        {
            if (!ExamGenerator.Generate_All_Exam(ref Workbook))
                return;
            
            Reset_MemberValue();
            Notice_Exam(Workbook.Count);

            foreach (var exam in Workbook)
            {
                switch (exam.type)
                {
                    case IVERB:
                        Console.Write("[{0}번 문제] <불규칙동사형> \"{1}\"의 현재형, 과거형, 과거분사를 모두 서술하세요!\n", Turn, exam.question);
#if DEBUG
                        Debugger.Mining_Solutions(exam.solutions);
#endif
                        break;
                    case WORD:
                        Console.Write("[{0}번 문제] <단어형> \"{1}\"의 의미를 서술하세요!\n", Turn, exam.question);
#if DEBUG
                        Debugger.Mining_Solutions(exam.solutions);
#endif
                        break;
                    case PROVERB:
                        Console.Write("[{0}번 문제] <속담형> \"{1}\"의 의미를 서술하세요!\n", Turn, exam.question);
#if DEBUG
                        Debugger.Mining_Solutions(exam.solutions);
#endif
                        break;
                    case DEFAULT:
                    default:
                        break;
                }

            WRITE_ANSWER:
                Answer = Console.ReadLine();

#if DEBUG
                switch (Answer)
                {
                    case "[All Wrong]":
                        Debugger.All_Exam_Wrong(ref Wrong, Workbook, ref WrongIverb);
                        goto TEST_END;
                    case "[All Right]":
                        Debugger.All_Exam_Pass(ref Right, Workbook, ref WrongIverb);
                        goto TEST_END;
                    case "[Escape]":
                        goto TEST_END;
                    default:
                        break;
                }

#else
                if (Answer == "")
                {
                    Console.Write("(공백 답안은 제출할 수 없습니다!)\n");
                    goto WRITE_ANSWER;
                }
#endif

                if (ScoreManager.isRightAnswer(exam.solutions, Answer))
                {
                    Console.WriteLine("[정답입니다!]\n< 맞은 문제 : [{0}] 틀린 문제 : [{1}] >", Right, Wrong);
                    switch (exam.type)
                    {
                        case IVERB:
                            ScoreManager.Erase_WrongNote(ref Right, exam, ref WrongIverb);
                            break;
                        case WORD:
                            ScoreManager.Erase_WrongNote(ref Right, exam, ref WrongWord);
                            break;
                        case PROVERB:
                            ScoreManager.Erase_WrongNote(ref Right, exam, ref WrongProverb);
                            break;
                        case DEFAULT:
                        default:
                            break;
                    }
                }

                else
                {
                    Console.WriteLine("[틀렸습니다...]\n< 맞은 문제 : [{0}] 틀린 문제 : [{1}] >", Right, Wrong);
                    switch (exam.type)
                    {
                        case IVERB:
                            ScoreManager.Add_WrongNote(ref Wrong, exam, ref WrongIverb);
                            break;
                        case WORD:
                            ScoreManager.Add_WrongNote(ref Wrong, exam, ref WrongWord);
                            break;
                        case PROVERB:
                            ScoreManager.Add_WrongNote(ref Wrong, exam, ref WrongProverb);
                            break;
                        case DEFAULT:
                        default:
                            break;
                    }

                }

            TEST_END:
                ++Turn;
                Thread.Sleep(800);
                Console.Clear();
            }

            Console.Write("[시험 종료] 수고하셨습니다!\n");
            Console.Write("맞은 문제 : [{0}] 틀린 문제 : [{1}]\n", Right, Wrong);
            Console.Write("(Enter를 누르시면 전 메뉴로 돌아갑니다...)");

            ToolManager.Press_Key(ConsoleKey.Enter);
        }

        public override void Exam_Iverb(exampleType type = IVERB)
        {
            if (!ExamGenerator.Generate_Exam(ref Workbook, type))
                return;

            ExamGenerator.Shuffle_Exam(ref Workbook);

            Reset_MemberValue();
            Notice_Exam(Workbook.Count, "단어");

            foreach (var exam in Workbook)
            {
                Console.Write("[{0}번 문제] \"{1}\"의 현재형, 과거형, 과거분사를 모두 서술하세요!\n", Turn, exam.question);
#if DEBUG
                        Debugger.Mining_Solutions(exam.solutions);
#endif

            WRITE_ANSWER:
                Answer = Console.ReadLine();

#if DEBUG
                switch (Answer)
                {
                    case "[All Wrong]":
                        Debugger.All_Exam_Wrong(ref Wrong, Workbook, ref WrongIverb);
                        goto TEST_END;
                    case "[All Right]":
                        Debugger.All_Exam_Pass(ref Right, Workbook, ref WrongIverb);
                        goto TEST_END;
                    case "[Escape]":
                        goto TEST_END;
                    default:
                        break;
                }

#else
                if (Answer == "")
                {
                    Console.Write("(공백 답안은 제출할 수 없습니다!)\n");
                    goto WRITE_ANSWER;
                }
#endif

                if (ScoreManager.isRightAnswer(exam.solutions, Answer))
                {
                    Console.WriteLine("[정답입니다!]\n< 맞은 문제 : [{0}] 틀린 문제 : [{1}] >", Right, Wrong);
                    ScoreManager.Erase_WrongNote(ref Right, exam, ref WrongIverb);
                }

                else
                {
                    Console.WriteLine("[틀렸습니다...]\n< 맞은 문제 : [{0}] 틀린 문제 : [{1}] >", Right, Wrong);
                    ScoreManager.Add_WrongNote(ref Wrong, exam, ref WrongIverb);
                }

                ++Turn;
                Thread.Sleep(800);
                Console.Clear();
            }

        TEST_END:
            Console.Write("[시험 종료] 수고하셨습니다!\n");
            Console.Write("맞은 문제 : [{0}] 틀린 문제 : [{1}]\n", Right, Wrong);
            Console.Write("(Enter를 누르시면 전 메뉴로 돌아갑니다...)");

            ToolManager.Press_Key(ConsoleKey.Enter);
        }

        public override void Exam_Proverb(exampleType type = PROVERB)
        {
            if (!ExamGenerator.Generate_Exam(ref Workbook, type))
                return;

            ExamGenerator.Shuffle_Exam(ref Workbook);

            Reset_MemberValue();
            Notice_Exam(Workbook.Count, "단어");

            foreach (var exam in Workbook)
            {
                Console.Write("[{0}번 문제] \"{1}\"의 의미를 서술하세요!\n", Turn, exam.question);
#if DEBUG
                        Debugger.Mining_Solutions(exam.solutions);
#endif

            WRITE_ANSWER:
                Answer = Console.ReadLine();

#if DEBUG
                switch (Answer)
                {
                    case "[All Wrong]":
                        Debugger.All_Exam_Wrong(ref Wrong, Workbook, ref WrongProverb);
                        goto TEST_END;
                    case "[All Right]":
                        Debugger.All_Exam_Pass(ref Right, Workbook, ref WrongProverb);
                        goto TEST_END;
                    case "[Escape]":
                        goto TEST_END;
                    default:
                        break;
                }

#else
                if (Answer == "")
                {
                    Console.Write("(공백 답안은 제출할 수 없습니다!)\n");
                    goto WRITE_ANSWER;
                }
#endif

                if (ScoreManager.isRightAnswer(exam.solutions, Answer))
                {
                    Console.WriteLine("[정답입니다!]\n< 맞은 문제 : [{0}] 틀린 문제 : [{1}] >", Right, Wrong);
                    ScoreManager.Erase_WrongNote(ref Right, exam, ref WrongProverb);
                }

                else
                {
                    Console.WriteLine("[틀렸습니다...]\n< 맞은 문제 : [{0}] 틀린 문제 : [{1}] >", Right, Wrong);
                    ScoreManager.Add_WrongNote(ref Wrong, exam, ref WrongProverb);
                }

                ++Turn;
                Thread.Sleep(800);
                Console.Clear();
            }

        TEST_END:
            Console.Write("[시험 종료] 수고하셨습니다!\n");
            Console.Write("맞은 문제 : [{0}] 틀린 문제 : [{1}]\n", Right, Wrong);
            Console.Write("(Enter를 누르시면 전 메뉴로 돌아갑니다...)");

            ToolManager.Press_Key(ConsoleKey.Enter);
        }

        public override void Exam_Word(exampleType type = WORD)
        {
            if (!ExamGenerator.Generate_Exam(ref Workbook, type))
                return;

            ExamGenerator.Shuffle_Exam(ref Workbook);

            Reset_MemberValue();
            Notice_Exam(Workbook.Count, "단어");

            foreach (var exam in Workbook)
            {
                Console.Write("[{0}번 문제] \"{1}\"의 의미를 서술하세요!\n", Turn, exam.question);
#if DEBUG
                        Debugger.Mining_Solutions(exam.solutions);
#endif

            WRITE_ANSWER:
                Answer = Console.ReadLine();

#if DEBUG
                switch (Answer)
                {
                    case "[All Wrong]":
                        Debugger.All_Exam_Wrong(ref Wrong, Workbook, ref WrongWord);
                        goto TEST_END;
                    case "[All Right]":
                        Debugger.All_Exam_Pass(ref Right, Workbook, ref WrongWord);
                        goto TEST_END;
                    case "[Escape]":
                        goto TEST_END;
                    default:
                        break;
                }

#else
                if (Answer == "")
                {
                    Console.Write("(공백 답안은 제출할 수 없습니다!)\n");
                    goto WRITE_ANSWER;
                }
#endif

                if (ScoreManager.isRightAnswer(exam.solutions, Answer))
                {
                    Console.WriteLine("[정답입니다!]\n< 맞은 문제 : [{0}] 틀린 문제 : [{1}] >", Right, Wrong);
                    ScoreManager.Erase_WrongNote(ref Right, exam, ref WrongWord);
                }

                else
                {
                    Console.WriteLine("[틀렸습니다...]\n< 맞은 문제 : [{0}] 틀린 문제 : [{1}] >", Right, Wrong);
                    ScoreManager.Add_WrongNote(ref Wrong, exam, ref WrongWord);
                }

                ++Turn;
                Thread.Sleep(800);
                Console.Clear();
            }

        TEST_END:
            Console.Write("[시험 종료] 수고하셨습니다!\n");
            Console.Write("맞은 문제 : [{0}] 틀린 문제 : [{1}]\n", Right, Wrong);
            Console.Write("(Enter를 누르시면 전 메뉴로 돌아갑니다...)");

            ToolManager.Press_Key(ConsoleKey.Enter);
        }
    }

    public class ReviewManager : Manager_Surface
    {
        protected override void Notice_Exam(int Problems, string ExamType)
        {
            Console.Clear();

            Console.Write("[{0} 복습 {1}선!]\n", ExamType, Problems);
            Console.Write("(Enter를 누르시면 시험을 시작합니다...)");

            Console.Write("\n\n\n<주의 사항>\n");
            Console.Write("★([복습 모드]는 정답의 여부가 오답노트에 영향을 끼치지 않습니다.)\n");
            Console.Write("★(입력은 모두 한 줄로 받으며, 띄어쓰기를 구분합니다.)\n");
            Console.Write("★(사용자가 정의한 txt파일을 통해 정답을 판단합니다.)\n");
            Console.Write("★(사용 중에 txt파일을 건드릴 경우, 오류가 날 수 있습니다.)");

            ToolManager.Press_Key(ConsoleKey.Enter);
        }

        protected override void Notice_Exam(int Problems)
        {
            Console.Clear();

            Console.Write("[오답 대난투 {0}선!]\n", Problems);
            Console.Write("(Enter를 누르시면 시험을 시작합니다...)");

            Console.Write("\n\n\n<주의 사항>\n");
            Console.Write("★([오답 대난투]는 정답의 여부가 오답노트에 영향을 끼치지 않습니다.)\n");
            Console.Write("★(입력은 모두 한 줄로 받으며, 띄어쓰기를 구분합니다.)\n");
            Console.Write("★(사용자가 정의한 txt파일을 통해 정답을 판단합니다.)\n");
            Console.Write("★(사용 중에 txt파일을 건드릴 경우, 오류가 날 수 있습니다.)");

            ToolManager.Press_Key(ConsoleKey.Enter);
        }

        protected override void Reset_MemberValue()
        {
            Answer = "Dummy Answer";
            Turn = 1;
            Right = 0;
            Wrong = 0;
        }

        public override void Check_Reviewbook()
        {
            Console.Clear();

            Console.Write("[틀린 단어]\n");
            Turn = 1;

            if (!(WrongWord.Count == 0))
                foreach (var Word in WrongWord)
                {
                    Console.Write("[{0}]. {1} : ", Turn, Word.question);

                    if (Word.solutions.Length == 1) { Console.Write("[{0}]\n", Word.solutions[0]); }
                    else { Console.Write("[{0}] ...\n", Word.solutions[0]); }
                    ++Turn;
                }
            else { Console.Write("(오답노트가 깨끗합니다.)\n"); }

            Console.Write("\n[틀린 불규칙동사]\n");
            Turn = 1;

            if (!(WrongIverb.Count == 0))
                foreach (var Iverb in WrongIverb)
                {
                    Console.Write("[{0}]. {1} : ", Turn, Iverb.question);

                    if (Iverb.solutions.Length == 1) { Console.Write("[{0}]\n", Iverb.solutions[0]); }
                    else { Console.Write("[{0}] ...\n", Iverb.solutions[0]); }
                    ++Turn;
                }
            else { Console.Write("(오답노트가 깨끗합니다.)\n"); }

            Console.Write("\n[틀린 속담]\n");
            Turn = 1;

            if (!(WrongProverb.Count == 0))
                foreach (var Proverb in WrongProverb)
                {
                    Console.Write("[{0}]. {1} : ", Turn, Proverb.question);

                    if (Proverb.solutions.Length == 1) { Console.Write("[{0}]\n", Proverb.solutions[0]); }
                    else { Console.Write("[{0}] ...\n", Proverb.solutions[0]); }
                    ++Turn;
                }
            else { Console.Write("(오답노트가 깨끗합니다.)\n"); }

            Console.Write("\n\n(Enter를 누르면 이전 메뉴로 나갑니다.)");
            ToolManager.Press_Key(ConsoleKey.Enter);
        }

        public override void Exam_All()
        {
            List<example> All_Review = new List<example>();
            ExamGenerator.Generate_All_Review(ref All_Review);

            Reset_MemberValue();
            Notice_Exam(All_Review.Count);

            foreach (var exam in All_Review)
            {
                switch (exam.type)
                {
                    case IVERB:
                        Console.Write("[{0}번 문제] <불규칙동사형> \"{1}\"의 현재형, 과거형, 과거분사를 모두 서술하세요!\n", Turn, exam.question);
#if DEBUG
                        Debugger.Mining_Solutions(exam.solutions);
#endif
                        break;
                    case WORD:
                        Console.Write("[{0}번 문제] <단어형> \"{1}\"의 의미를 서술하세요!\n", Turn, exam.question);
#if DEBUG
                        Debugger.Mining_Solutions(exam.solutions);
#endif
                        break;
                    case PROVERB:
                        Console.Write("[{0}번 문제] <속담형> \"{1}\"의 의미를 서술하세요!\n", Turn, exam.question);
#if DEBUG
                        Debugger.Mining_Solutions(exam.solutions);
#endif
                        break;
                    case DEFAULT:
                    default:
                        break;
                }


            WRITE_ANSWER:
                Answer = Console.ReadLine();

#if DEBUG
                switch (Answer)
                {
                    case "[All Wrong]":
                        Debugger.All_Exam_Wrong(ref Wrong, Workbook, ref WrongWord);
                        goto TEST_END;
                    case "[All Right]":
                        Debugger.All_Exam_Pass(ref Right, Workbook, ref WrongWord);
                        goto TEST_END;
                    case "[Escape]":
                        goto TEST_END;
                    default:
                        break;
                }

#else
                if (Answer == "")
                {
                    Console.Write("(공백 답안은 제출할 수 없습니다!)\n");
                    goto WRITE_ANSWER;
                }
#endif

                if (ScoreManager.isRightAnswer(exam.solutions, Answer))
                {
                    ++Right;
                    Console.WriteLine("[정답입니다!]\n< 맞은 문제 : [{0}] 틀린 문제 : [{1}] >", Right, Wrong);
                }

                else
                {
                    ++Wrong;
                    Console.WriteLine("[틀렸습니다...]\n< 맞은 문제 : [{0}] 틀린 문제 : [{1}] >", Right, Wrong);
                }
                ++Turn;
                Thread.Sleep(800);
                Console.Clear();
            }

        TEST_END:
            Console.Write("[복습 종료] 수고하셨습니다!\n");
            Console.Write("맞은 문제 : [{0}] 틀린 문제 : [{1}]\n", Right, Wrong);
            Console.Write("(Enter를 누르시면 전 메뉴로 돌아갑니다...)");

            ToolManager.Press_Key(ConsoleKey.Enter);
        }

        public override void Exam_Iverb(exampleType type = IVERB)
        {
            List<example> Reviewbook = new List<example>();
            Reviewbook = ExamGenerator.Shuffle_Exam(WrongIverb);

            Reset_MemberValue();

            Notice_Exam(Reviewbook.Count, "불규칙동사");

            foreach (var review in Reviewbook)
            {
                Console.Write("[{0}번 문제] \"{1}\"의 의미를 서술하세요!\n", Turn, review.question);


            WRITE_ANSWER:
                Answer = Console.ReadLine();

#if DEBUG
                switch (Answer)
                {
                    case "[All Wrong]":
                        Debugger.All_Exam_Wrong(ref Wrong, Workbook, ref WrongWord);
                        goto TEST_END;
                    case "[All Right]":
                        Debugger.All_Exam_Pass(ref Right, Workbook, ref WrongWord);
                        goto TEST_END;
                    case "[Escape]":
                        goto TEST_END;
                    default:
                        break;
                }

#else
                if (Answer == "")
                {
                    Console.Write("(공백 답안은 제출할 수 없습니다!)\n");
                    goto WRITE_ANSWER;
                }
#endif

                if (ScoreManager.isRightAnswer(review.solutions, Answer))
                {
                    ++Right;
                    Console.WriteLine("[정답입니다!]\n< 맞은 문제 : [{0}] 틀린 문제 : [{1}] >", Right, Wrong);
                }

                else
                {
                    ++Wrong;
                    Console.WriteLine("[틀렸습니다...]\n< 맞은 문제 : [{0}] 틀린 문제 : [{1}] >", Right, Wrong);
                }

                ++Turn;
                Thread.Sleep(800);
                Console.Clear();
            }

            TEST_END:
            Console.Write("[복습 종료] 수고하셨습니다!\n");
            Console.Write("맞은 문제 : [{0}] 틀린 문제 : [{1}]\n", Right, Wrong);
            Console.Write("(Enter를 누르시면 전 메뉴로 돌아갑니다...)");

            ToolManager.Press_Key(ConsoleKey.Enter);
        }

        public override void Exam_Proverb(exampleType type = PROVERB)
        {
            List<example> Reviewbook = new List<example>();
            Reviewbook = ExamGenerator.Shuffle_Exam(WrongProverb);

            Reset_MemberValue();

            Notice_Exam(Reviewbook.Count, "속담");

            foreach (var review in Reviewbook)
            {
                Console.Write("[{0}번 문제] \"{1}\"의 의미를 서술하세요!\n", Turn, review.question);


            WRITE_ANSWER:
                Answer = Console.ReadLine();

#if DEBUG
                switch (Answer)
                {
                    case "[All Wrong]":
                        Debugger.All_Exam_Wrong(ref Wrong, Workbook, ref WrongWord);
                        goto TEST_END;
                    case "[All Right]":
                        Debugger.All_Exam_Pass(ref Right, Workbook, ref WrongWord);
                        goto TEST_END;
                    case "[Escape]":
                        goto TEST_END;
                    default:
                        break;
                }

#else
                if (Answer == "")
                {
                    Console.Write("(공백 답안은 제출할 수 없습니다!)\n");
                    goto WRITE_ANSWER;
                }
#endif

                if (ScoreManager.isRightAnswer(review.solutions, Answer))
                {
                    ++Right;
                    Console.WriteLine("[정답입니다!]\n< 맞은 문제 : [{0}] 틀린 문제 : [{1}] >", Right, Wrong);
                }

                else
                {
                    ++Wrong;
                    Console.WriteLine("[틀렸습니다...]\n< 맞은 문제 : [{0}] 틀린 문제 : [{1}] >", Right, Wrong);
                }

                ++Turn;
                Thread.Sleep(800);
                Console.Clear();
            }

        TEST_END:
            Console.Write("[복습 종료] 수고하셨습니다!\n");
            Console.Write("맞은 문제 : [{0}] 틀린 문제 : [{1}]\n", Right, Wrong);
            Console.Write("(Enter를 누르시면 전 메뉴로 돌아갑니다...)");

            ToolManager.Press_Key(ConsoleKey.Enter);
        }

        public override void Exam_Word(exampleType type = WORD)
        {
            List<example> Reviewbook = new List<example>();
            Reviewbook = ExamGenerator.Shuffle_Exam(WrongWord);

            Reset_MemberValue();

            Notice_Exam(Reviewbook.Count, "단어");

            foreach (var review in Reviewbook)
            {
                Console.Write("[{0}번 문제] \"{1}\"의 의미를 서술하세요!\n", Turn, review.question);
#if DEBUG
                Debugger.Mining_Solutions(review.solutions);
#endif


            WRITE_ANSWER:
                Answer = Console.ReadLine();

#if DEBUG
                switch (Answer)
                {
                    case "[All Wrong]":
                        Debugger.All_Exam_Wrong(ref Wrong, Workbook, ref WrongWord);
                        goto TEST_END;
                    case "[All Right]":
                        Debugger.All_Exam_Pass(ref Right, Workbook, ref WrongWord);
                        goto TEST_END;
                    case "[Escape]":
                        goto TEST_END;
                    default:
                        break;
                }

#else
                if (Answer == "")
                {
                    Console.Write("(공백 답안은 제출할 수 없습니다!)\n");
                    goto WRITE_ANSWER;
                }
#endif

                if (ScoreManager.isRightAnswer(review.solutions, Answer))
                {
                    ++Right;
                    Console.WriteLine("[정답입니다!]\n< 맞은 문제 : [{0}] 틀린 문제 : [{1}] >", Right, Wrong);
                }

                else
                {
                    ++Wrong;
                    Console.WriteLine("[틀렸습니다...]\n< 맞은 문제 : [{0}] 틀린 문제 : [{1}] >", Right, Wrong);
                }

                ++Turn;
                Thread.Sleep(800);
                Console.Clear();
            }

        TEST_END:
            Console.Write("[복습 종료] 수고하셨습니다!\n");
            Console.Write("맞은 문제 : [{0}] 틀린 문제 : [{1}]\n", Right, Wrong);
            Console.Write("(Enter를 누르시면 전 메뉴로 돌아갑니다...)");

            ToolManager.Press_Key(ConsoleKey.Enter);
        }
    }
}
