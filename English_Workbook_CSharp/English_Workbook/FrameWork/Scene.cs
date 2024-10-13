using System;

using static Main.Program_Info;
using static Scene.sceneCollections;
using static Example.examCollections;

using ToolSystem;
using StudySystem;

namespace Scene
{
    public class scene
    {
        protected byte Index = 0;
        protected ConsoleKeyInfo Act;
        protected string[] SceneInfo;
        protected string[] Menus;

        protected ExamManager examManager = new ExamManager();
        protected ReviewManager reviewManager = new ReviewManager();

        public scene() { }
        public virtual void MainMenu() { }
        public virtual void PrintInfo() { }
    }

    public class TitleScene : scene
    {
        public override void MainMenu()
        {
            while (true)
            {
                PrintInfo();

            PRESS_ENTER:
                ConsoleKeyInfo KeyInfo = Console.ReadKey(true);

                if (ToolManager.isPressKey(KeyInfo, ConsoleKey.Enter))
                    break;

                else { goto PRESS_ENTER; }
            }

            Scenes[1].MainMenu();
            Console.Clear();
            Console.Write("[프로그램을 종료합니다...]");
            return;
        }

        public override void PrintInfo()
        {
            Console.Clear();
            Console.Write("[ {0} < Ver {1} > ]\n", program_Name, version);
            Console.Write("(Enter를 누르시면 다음으로 넘어갑니다...)");
        }
    }

    public class MainScene : scene
    {
        public MainScene()
        {
            this.SceneInfo = new string[] { "=====================================[메인 메뉴]=====================================", 
                                            "★(테스트와 복습 중 원하는 공부를 할 수 있습니다.)",
                                            "====================================================================================="};

            this.Menus = new string[] { "[테스트 모드]", "[복습 모드]", "[나가기]" };
        }

        public override void PrintInfo()
        {
            Console.Clear();

            Index = 0;
            foreach (var Elem in SceneInfo)
            {
                Console.Write("{0}\n", Elem);
                ++Index;
            }

            Console.Write("\n");

            Index = 1;
            foreach (var Elem in Menus)
            {
                Console.Write("[{0}]. {1}\n", Index, Elem);
                ++Index;
            }
        }

        public override void MainMenu()
        {
            while (true)
            {
                PrintInfo();

            CHOOSE_WAY:
                Act = Console.ReadKey(true);

                switch (Act.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        Scenes[2].MainMenu();
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        Scenes[3].MainMenu();
                        break;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        return;
                    default:
                        goto CHOOSE_WAY;
                }
            }
        }
    }

    public class ExamScene : scene
    {
        public ExamScene()
        {
            this.SceneInfo = new string[] { "=====================================[테스트 모드]=====================================",
                                            "★(단어, 불규칙동사, 속담 중 자신이 원하는 테스트를 볼 수 있습니다.)",
                                            "★(또한, [문제 대난투]모드를 통해 모든 유형의 테스트를 볼 수도 있습니다.)",
                                            "======================================================================================"};

            this.Menus = new string[] { "[단어 테스트]", "[불규칙동사 테스트]", "[속담 테스트]", "[문제 대난투]", "[문제 목록 확인]", "[뒤로 가기]" };
        }

        public override  void MainMenu()
        {
            while (true)
            {
                PrintInfo();

            CHOOSE_WAY:
                Act = Console.ReadKey(true);

                switch (Act.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        Console.Clear();
                        examManager.Exam_Word();
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        Console.Clear();
                        examManager.Exam_Iverb();
                        break;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        Console.Clear();
                        examManager.Exam_Proverb();
                        break;
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        Console.Clear();
                        examManager.Exam_All();
                        break;
                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        Console.Clear();
                        examManager.Check_Workbook();
                        break;
                    case ConsoleKey.D6:
                    case ConsoleKey.NumPad6:
                        return;
                    default:
                        goto CHOOSE_WAY;
                }
            }
        }

        public override void PrintInfo()
        {
            Console.Clear();

            Index = 1;
            foreach (var Elem in SceneInfo)
            {
                Console.Write("{0}\n", Elem);
                ++Index;
            }

            Console.Write("\n");

            Index = 1;
            foreach (var Elem in Menus)
            {
                Console.Write("[{0}]. {1}\n", Index, Elem);
                ++Index;
            }
        }
    }

    public class ReviewScene : scene
    {
        public ReviewScene()
        {
            this.SceneInfo = new string[] { "=====================================[복습 모드]=====================================",
                                            "★([오답노트]를 통해 자신이 틀린 문제들을 확인하거나, 틀린 문제들을 다시 풀어볼 수 있습니다.)",
                                            "★(또한, [오답 대난투]모드를 통해 모든 유형의 오답들을 다시 한번 공부할 수도 있습니다.)",
                                            "===================================================================================="};

            this.Menus = new string[] { "[단어 복습]", "[불규칙동사 복습]", "[속담 복습]", "[오답 대난투]", "[오답노트]", "[뒤로 가기]" };
        }

        public override void MainMenu()
        {
            bool isEmptyNote = false;

            while (true)
            {
                PrintInfo();

                if (isEmptyNote == true) { Console.Write("(해당 오답노트가 비어 있습니다.)"); }

                CHOOSE_WAY:
                Act = Console.ReadKey(true);

                switch (Act.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        if (WrongWord.Count != 0)
                        {
                            isEmptyNote = false;
                            Console.Clear();
                            reviewManager.Exam_Word();
                        }
                        else { isEmptyNote = true; }
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        if (WrongIverb.Count != 0)
                        {
                            isEmptyNote = false;
                            Console.Clear();
                            reviewManager.Exam_Iverb();
                        }
                        else { isEmptyNote = true; }
                        break;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        if (WrongProverb.Count != 0)
                        {
                            isEmptyNote = false;
                            Console.Clear();
                            reviewManager.Exam_Proverb();
                        }
                        else { isEmptyNote = true; }
                        break;
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        if (WrongWord.Count != 0 || WrongIverb.Count != 0 || WrongProverb.Count != 0)
                        {
                            isEmptyNote = false;
                            reviewManager.Exam_All();
                        }
                        else { isEmptyNote = true; }
                        break;
                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        if (WrongWord.Count != 0 || WrongIverb.Count != 0 || WrongProverb.Count != 0)
                        {
                            isEmptyNote = false;
                            reviewManager.Check_Reviewbook();
                        }
                        else { isEmptyNote = true; }
                        break;
                    case ConsoleKey.D6:
                    case ConsoleKey.NumPad6:
                        return;
                    default:
                        goto CHOOSE_WAY;
                }
            }
        }

        public override void PrintInfo()
        {
            Console.Clear();

            Index = 1;
            foreach (var Elem in SceneInfo)
            {
                Console.Write("{0}\n", Elem);
                ++Index;
            }

            Console.Write("\n");

            Index = 1;
            foreach (var Elem in Menus)
            {
                Console.Write("[{0}]. {1}\n", Index, Elem);
                ++Index;
            }
        }
    }

    public class sceneCollections
    {
        public static scene[] Scenes = new scene[]
        {
            new TitleScene(), new MainScene(), new ExamScene(), new ReviewScene()
        };
    }
}
