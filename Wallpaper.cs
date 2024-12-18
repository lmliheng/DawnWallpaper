﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DawnWallpaper
{
    internal class Wallpaper
    {
        [DllImport("user32.dll", EntryPoint = "SetParent")]
        private static extern int SetParent(int hWndChild, int hWndNewParent);
        [DllImport("user32.dll", EntryPoint = "FindWindowA")]
        private static extern IntPtr FindWindowA(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "FindWindowExA")]
        private static extern IntPtr FindWindowExA(IntPtr hWndParent, IntPtr hWndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll", EntryPoint = "GetClassNameA")]
        private static extern IntPtr GetClassNameA(IntPtr hWnd, IntPtr lpClassName, int nMaxCount);
        [DllImport("user32.dll", EntryPoint = "GetParent")]
        private static extern IntPtr GetParent(IntPtr hWnd);
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int SystemParametersInfo(int uAction, int uParam, StringBuilder lpvParam, int fuWinIni);

        public static void SetFather(Form form)
        {
            SetParent((int)form.Handle, GetBackground());
        }

        private static int GetBackground()
        {
            IntPtr background = IntPtr.Zero;
            IntPtr father = FindWindowA("progman", "Program Manager");
            IntPtr workerW = IntPtr.Zero;
            
            do
            {
                workerW = FindWindowExA(IntPtr.Zero, workerW, "workerW", null);
                if (workerW != IntPtr.Zero)
                {
                    char[] buff = new char[200];
                    IntPtr b = Marshal.UnsafeAddrOfPinnedArrayElement(buff, 0);
                    int ret = (int)GetClassNameA(workerW, b, 400);
                    if (ret == 0) throw new Exception("Error");
                }
                if (GetParent(workerW) == father)
                {
                    background = workerW;
                }
            } while (workerW != IntPtr.Zero);
            return (int)background;
            
        }
        public static bool Refresh()
        {
            StringBuilder wallpaper = new StringBuilder(200);
            SystemParametersInfo(0x73, 200, wallpaper, 0);
            int ret = SystemParametersInfo(20, 1, wallpaper, 3);
            if (ret != 0)
            {
                RegistryKey hk = Registry.CurrentUser;
                RegistryKey run = hk.CreateSubKey(@"Control Panel\Desktop\");
                run.SetValue("Wallpaper", wallpaper.ToString());
                return true;
            }
            return false;
        }
        public static void GETHandleRun(Form form)
        {
            if (File.Exists(Path.Combine(Application.StartupPath, "GETHandle.exe")))
            {
                Process GEThandle = new Process();
                GEThandle.StartInfo.FileName = Path.Combine(Application.StartupPath, "GETHandle.exe");
                GEThandle.Start();
                GEThandle.WaitForExit();

                form.TopMost = true;
                form.TopMost = false;
            }
        }
    }
}
