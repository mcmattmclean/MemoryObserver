using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MemoryObserver
{
    class Program
    {
        const int PROCESS_VM_READ = 0x0010;
        
        static Process GetValidProcess()
        {
            bool isValid = false;
            Process process = null;
            do
            {
                Console.Write("Please input the ID for the process you wish to examine: ");
                try
                {
                    process = Process.GetProcessById(Int32.Parse(Console.ReadLine()));
                    isValid = true;
                }
                catch (Exception)
                {
                    Console.WriteLine("Sorry, that is not a valid process.");
                }
            } while (!isValid);

            return process;
        }

        static void PrintProcesses()
        {
            const string format = "{0,-40} {1}";
            var allProcesses = Process.GetProcesses();
            Console.WriteLine(format, "Process Name", "Id");
            foreach (Process process in allProcesses)
            {
                Console.WriteLine(format, process.ProcessName, process.Id);
            }
        }

        static void ListThreads()
        {
            const string format = "{0,-10} {1}";
            var process = GetValidProcess();
            try
            {
                Console.WriteLine(format, "Thread Id", "Start Time");
                foreach (ProcessThread thread in process.Threads)
                {
                    Console.WriteLine(format, thread.Id, thread.StartTime);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void PrintModules()
        {
            const string format = "{0,-32} {1}";
            var process = GetValidProcess();
            try
            {
                Console.WriteLine(format, "Module Name", "Base Address");
                foreach (ProcessModule module in process.Modules)
                {
                    Console.WriteLine(format, module.ModuleName, module.BaseAddress.ToString("X"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void ShowProcessMemory()
        {
            var process = GetValidProcess();

            try
            {
                var lpBaseAddress = process.MainModule.BaseAddress.ToInt64();
                var moduleSize = process.MainModule.ModuleMemorySize;
                var lpBuffer = new byte[moduleSize];
                int lpNumberOfBytesRead = 0;

                Toolhelp32ReadProcessMemory(process.Id, lpBaseAddress, lpBuffer, lpBuffer.Length, ref lpNumberOfBytesRead);
                var hexDump = BitConverter.ToString(lpBuffer).Replace("-", "");

                Console.WriteLine(hexDump);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [DllImport("kernel32.dll")]
        public static extern bool Toolhelp32ReadProcessMemory(int th32ProcessID, Int64 lpBaseAddress, byte[] lpBuffer, int cbRead, ref int lpNumberOfBytesRead);
    
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to MemoryObserver. Please choose an option from the following:");

            ConsoleKeyInfo choice;
            do
            {
                Console.WriteLine("\n");
                Console.WriteLine("F1 - Enumerate running processes");
                Console.WriteLine("F2 - List running threads within a process boundary");
                Console.WriteLine("F3 - Enumerate loaded modules within a process");
                Console.WriteLine("F4 - Read executable pages of a process");
                Console.WriteLine("Esc - Exit");
                Console.Write("> ");

                choice = Console.ReadKey();
                Console.WriteLine("\n");
                switch(choice.Key)
                {
                    case ConsoleKey.F1:
                        PrintProcesses();
                        break;
                    case ConsoleKey.F2:
                        ListThreads();
                        break;
                    case ConsoleKey.F3:
                        PrintModules();
                        break;
                    case ConsoleKey.F4:
                        ShowProcessMemory();
                        break;
                    default:
                        break;
                }
            } while (choice.Key != ConsoleKey.Escape);
        }
    }
}
