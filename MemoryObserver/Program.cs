using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryObserver
{
    class Program
    {
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
            var allProcesses = Process.GetProcesses();
            foreach (Process process in allProcesses)
            {
                Console.WriteLine("{0} {1}", process.ProcessName, process.Id);
            }
        }

        static void ListThreads()
        {
            var process = GetValidProcess();
            try
            {
                foreach(ProcessThread thread in process.Threads)
                {
                    Console.WriteLine("{0} {1}", thread.StartTime, thread.Id);
                }
            }
            catch(System.ComponentModel.Win32Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void PrintModules()
        {
            var process = GetValidProcess();
            try
            {
                foreach (ProcessModule module in process.Modules)
                {
                    Console.WriteLine("{0} {1}", module.ModuleName, module.EntryPointAddress);
                }
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

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
                Console.WriteLine("F4 - Show executable pages within a process");
                Console.WriteLine("F5 - Read the memory of a process");
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
                        break;
                    case ConsoleKey.F5:
                        break;
                }

            } while (choice.Key != ConsoleKey.Escape);


            //using (Process myProcess = new Process())
            //{
            //    // Get the process start information of notepad.
            //    ProcessStartInfo myProcessStartInfo = new ProcessStartInfo("notepad.exe");
            //    // Assign 'StartInfo' of notepad to 'StartInfo' of 'myProcess' object.
            //    myProcess.StartInfo = myProcessStartInfo;
            //    // Create a notepad.
            //    myProcess.Start();
            //    System.Threading.Thread.Sleep(1000);
            //    ProcessModule myProcessModule;
            //    // Get all the modules associated with 'myProcess'.
            //    ProcessModuleCollection myProcessModuleCollection = myProcess.Modules;
            //    Console.WriteLine("Properties of the modules  associated with 'notepad' are:");
            //    // Display the properties of each of the modules.
            //    for (int i = 0; i < myProcessModuleCollection.Count; i++)
            //    {
            //        myProcessModule = myProcessModuleCollection[i];
            //        Console.WriteLine("The moduleName is "
            //            + myProcessModule.ModuleName);
            //        Console.WriteLine("The " + myProcessModule.ModuleName + "'s base address is: "
            //            + myProcessModule.BaseAddress);
            //        Console.WriteLine("The " + myProcessModule.ModuleName + "'s Entry point address is: "
            //            + myProcessModule.EntryPointAddress);
            //        Console.WriteLine("The " + myProcessModule.ModuleName + "'s File name is: "
            //            + myProcessModule.FileName);
            //    }
            //    // Get the main module associated with 'myProcess'.
            //    myProcessModule = myProcess.MainModule;
            //    // Display the properties of the main module.
            //    Console.WriteLine("The process's main moduleName is:  "
            //        + myProcessModule.ModuleName);
            //    Console.WriteLine("The process's main module's base address is: "
            //        + myProcessModule.BaseAddress);
            //    Console.WriteLine("The process's main module's Entry point address is: "
            //        + myProcessModule.EntryPointAddress);
            //    Console.WriteLine("The process's main module's File name is: "
            //        + myProcessModule.FileName);
            //    myProcess.CloseMainWindow();
            //}
        }
    }
}
