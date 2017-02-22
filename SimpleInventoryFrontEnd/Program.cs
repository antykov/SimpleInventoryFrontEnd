using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SimpleInventoryFrontEnd
{
    static class Program
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        internal static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool FreeLibrary(IntPtr hModule);

        internal delegate int PointerToMethodInvoker();

        [STAThread]
        static void Main()
        {
            if (!CheckAtolBarcodeReader())
                return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        static bool CheckAtolBarcodeReader()
        {
            if (Type.GetTypeFromProgID("AddIn.Scaner45") != null)
                return true;

            string scanerDllPath = Path.Combine(Environment.CurrentDirectory, "Scaner1C.dll");
            if (!File.Exists(scanerDllPath))
            {
                MessageBox.Show("Отсутствует библиотека Scaner1C.dll!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                IntPtr hLib = LoadLibrary(scanerDllPath);
                if (hLib == IntPtr.Zero)
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());

                IntPtr dllEntryPoint = GetProcAddress(hLib, "DllRegisterServer");
                if (dllEntryPoint == IntPtr.Zero)
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());

                PointerToMethodInvoker dllRegisterDelegate =
                       (PointerToMethodInvoker)Marshal.GetDelegateForFunctionPointer(dllEntryPoint, typeof(PointerToMethodInvoker));
                dllRegisterDelegate();

                FreeLibrary(hLib);

                if (Type.GetTypeFromProgID("AddIn.Scaner45") == null)
                    throw new Exception("Не удалось зарегистрировать библиотеку Scaner1C.dll!");

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка регистрации библиотеки", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }
    }
}
