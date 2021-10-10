//
//      DO NOT BUILD `AnyCPU`
// in AnyCPU build DLL will not work!
//   !!! Build only x86 or x64 !!!
//
//
// Original Manual at:
//   https://www.sites.google.com/site/robertgiesecke/Home/uploads/unmanagedexports
// NuGet:
//   https://www.nuget.org/packages/UnmanagedExports
// Build on article:
//   https://www.c-sharpcorner.com/article/export-managed-code-as-unmanaged/
//

//
// Это библиотека C#, которая экспортирует функции для других программ не .Net
//

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using RGiesecke.DllExport;

using System.Windows.Forms;

namespace csharplib
{
    // Class Library Main Export //
    internal static class UnmanagedExports
    {
        // PREDEFINED METHODS //

        private const string LIBNAME = "UnmanagedExports C# Sample Library";         
        private static string CALLER = "Unknown";
        
        // C++    -- typedef void (__cdecl * TestFunc)();
        // python -- _cdll.Test()
        // delphi -- procedure Test(); cdecl; external 'UnmanagedExports.dll';
        [DllExport("Test", CallingConvention = CallingConvention.Cdecl)]
        static void Test()
        {
            
        }

        // C++    -- typedef char* (__cdecl * GetLibNameFunc)();
        // python -- c_char_p(_cdll.GetLibName())
        // delphi -- function GetLibName(): PChar; cdecl; external 'UnmanagedExports.dll';
        [DllExport("GetLibName", CallingConvention = CallingConvention.Cdecl)]
        static string GetLibName()
        {
          return LIBNAME;
        }

        // C++    -- typedef void* (__cdecl * SetCallerNameFunc)(char*);
        // python -- _cdll.SetCallerName(c_char_p(b"Passed by Python"))
        // delphi -- procedure SetCallerName(str: PChar); cdecl; external 'UnmanagedExports.dll';
        [DllExport("SetCallerName", CallingConvention = CallingConvention.Cdecl)]
        static void SetCallerName(string name)
        {
            CALLER = String.IsNullOrEmpty(name) ? "Unknown" : name;
        }

        // C++    -- typedef char* (__cdecl * GetCallerNameFunc)();
        // python -- c_char_p(_cdll.GetCallerName())
        // delphi -- function GetCallerName(): PChar; cdecl; external 'UnmanagedExports.dll';
        [DllExport("GetCallerName", CallingConvention = CallingConvention.Cdecl)]
        static string GetCallerName()
        {
            return CALLER;
        }
    }

    // Console Application Entry Point //
    internal static class TestUnmanagedExports
    {
        // Test DLL in C# Application //

        private const string LIBFILE = @"UnmanagedExports.dll";

        [DllImport(LIBFILE, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Test();

        [DllImport(LIBFILE, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern string GetLibName();

        [DllImport(LIBFILE, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetCallerName(string name);

        [DllImport(LIBFILE, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern string GetCallerName();

        static void Main()
        {
            Test();
            string lib = GetLibName();
            string wcn = GetCallerName();
            SetCallerName("Passed by C#");
            string scn = GetCallerName();
        }
    }
}

/*
 #
 # Calling DLL from Python
 #
 
 from ctypes import *
 
 print "C# lib:"
 csdll = cdll.LoadLibrary("UnmanagedExports.dll") # cdecl dll
 print "  ", csdll._name, " - ok"
 csdll.Test()
 print "   Test() called"
 print "   GetLibName() = ", c_char_p(csdll.GetLibName())
 cal_was = c_char_p(csdll.GetCallerName())
 csdll.SetCallerName(c_char_p(b"Passed by Python"))
 cal_set = c_char_p(csdll.GetCallerName())
 print "   Caller set from", cal_was, "to" , cal_set
 
 */

/*
 //
 // Calling DLL from C++
 //
 
 char* dllNameA    =  "csharplib.dll"; // ANSI string
 HINSTANCE pLib = LoadLibraryA(dllNameA);  
 typedef void (__cdecl * TestFunc)();	
 TestFunc Test = (TestFunc) GetProcAddress(pLib, "Test");	
 Test();

 typedef char* (__cdecl * GetLibNameFunc)();
 GetLibNameFunc GetLibName = (GetLibNameFunc) GetProcAddress(pLib, "GetLibName");
 char* nm = GetLibName();

 typedef char* (__cdecl * GetCallerNameFunc)();
 typedef void* (__cdecl * SetCallerNameFunc)(char*);
 GetCallerNameFunc GetCallerName = (GetCallerNameFunc) GetProcAddress(pLib, "GetCallerName");
 SetCallerNameFunc SetCallerName = (SetCallerNameFunc) GetProcAddress(pLib, "SetCallerName");

 char* caller = GetCallerName();
 SetCallerName("HelloWorld");
 caller = GetCallerName();
 
 */

/*
 //
 // Calling DLL from Delphi
 //
 
 procedure Test(); cdecl; external 'UnmanagedExports.dll'; 
 function GetLibName(): PChar; cdecl; external 'UnmanagedExports.dll';
 procedure SetCallerName(str: PChar); cdecl; external 'UnmanagedExports.dll';
 function GetCallerName(): PChar; cdecl; external 'UnmanagedExports.dll';
 
 var
   str: PChar;
 begin
   Test();
   str := GetLibName();
   str := PChar('Passed From Delphi');
   SetCallerName(str);
   str := GetCallerName();
 end;
 
 */
