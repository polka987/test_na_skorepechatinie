using Newtonsoft.Json;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;

namespace test_na_skoropechatanie
{
    class Program
    {
        private static List<User> users;
        private static void Main()
        {
            if (!File.Exists("result.json"))
                File.WriteAllText("result.json", "");
            users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText("result.json")) ?? new List<User>();
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                if (users.Count > 1) sort_users();
                ConsoleKeyInfo a;
                do
                {
                    Console.Clear();
                    foreach (var user in users)
                        Console.WriteLine(user.info());
                    Console.WriteLine();
                    Console.WriteLine($"Попробовать себя в тесте - '+'");
                    a = Console.ReadKey();
                } while (a.Key != ConsoleKey.Add);
                Test.registration();
                File.WriteAllText("result.json",JsonConvert.SerializeObject(users));
            }
        }
        protected static User get_user(int id)
        {
            foreach (var user in users)
            {
                if (user.ID == id)
                    return user;
            }
            return new User();
        }
        protected static bool inusers(string name)
        {
            foreach (var user in users)
            {
                if (user.Name == name)
                    return true;
            }
            return false;
        }
        protected static void sort_users()
        {
            User tmp = new User();
            for (int i = 0; i < users.Count; i++)
            {
                for (int j = i; j < users.Count - 1; j++)
                {
                    if (users[j].Svm > users[j + 1].Svm)
                    {
                        tmp = users[j];
                        users[j + 1] = users[j];
                        users[j] = tmp;
                    }

                }
            }
        }
        protected static void add_user(User user) => users.Add(user);
        protected static int get_new_id() => users.Count+1;
    }
    class User
    {
        public int ID;
        public string Name;
        public int Svm;

        public User() { }
        public User(int id, string name, int svm)
        {
            ID = id;
            Name = name;
            Svm = svm;
        }

        public string info() => $"Пользователь {Name}\nС результатом:\nCимволов в минуту: {Svm}\nCимволов в секунду: {Svm /60}\n";

    }
    class Test : Program
    {
        private static bool test_start = false;
        private static string text = "Боль никто простейшим упрекнуть предаваться справедливости, пользы вами приносило великие некоей: наслаждений. Страдания предпочел, вы постигают нас немалое восхваляющих раз из-за тягостными если представление приносило я иной простейшим избегал воспользоваться, наслаждений как кто всю неприятностей возникают. Жизни ни лишь, по, когда презирает, разъясню человек истину стал упрекнуть или боль: то иной, возжаждал — порицающих избегает обстоятельства но. Некое перед, из наслаждение возникает вами наслаждению: если это приносило немалое, говорил некоей такие иной простейшим разъясню никакого. ";
        private static int result = 0, time = 0;
        public static void registration()
        {
            Console.Clear();
            Console.WriteLine("Введите ваше имя:");
            string new_name = Console.ReadLine();
            if (new_name == "") registration();
            if (!inusers(new_name)) start(new_name);
            else registration();
        }
        public static void start(string name)
        {
            ConsoleKeyInfo ch;
            do
            {
                Console.Clear();
                Console.Write($"{text}\n\n\n\nКак только будете готовы - нажмите Enter");
                ch = Console.ReadKey(true);
            } while (ch.Key != ConsoleKey.Enter);
            test_start = true;
            new Thread(Timer).Start();
            do
            {
                ch = Console.ReadKey(true);
                char chr = text[result];
                if (ch.KeyChar.ToString() == chr.ToString())
                {
                    int sop = result / 120, pos = result % 120;
                    Console.SetCursorPosition(result % 120, result / 120);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(ch.KeyChar);
                    result++;
                    if (result - 1 == text.Length)
                        test_start = false;
                }

            } while (test_start);
            Console.SetCursorPosition(0, 20);
            Console.WriteLine("Тест завершен, спасибо за его прохождение, результат будет в таблице участников, нажмите любую кнопку - чтобы продолжить");
            Console.ReadKey(true);
            add_user(new User(get_new_id(), name, result / (60 - time) * 60));

        }
        private static void Timer()
        {
            time = 60;
            do
            {
                Console.SetCursorPosition(20, 15);
                Console.WriteLine("    ");
                Console.SetCursorPosition(20, 15);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(time == 60 ? "1:00" : $"0:{time}");
                Thread.Sleep(1000);
                time--;
                if (time == 0) test_start = false;
            } while (test_start);
        }
    }
}