using Grpc.Core;
using Grpc.Net.Client;
using RevitOutOfContext_gRPC_ProtosF;


//The port number must match the port of the gRPC server.
Console.WriteLine("Write some text to send");
Console.WriteLine("Or press Q key to exit...");

while (true)
{
    using var channel = GrpcChannel.ForAddress("http://localhost:5064");
    var client = new Greeter.GreeterClient(channel);
    var userName = Environment.UserName;

    var text = Console.ReadLine();
    CallOptions optionss = new CallOptions();
    CommandReply reply = client.SayHello(new HelloRequest { Name = userName, Text = text }, optionss);
    Console.WriteLine(reply.Command);
    var test = Console.ReadKey();
    var test2 = test.Key.ToString();
    if (test2 == "Q")
    {
        return;
    }
}


//using System;
//using System.Diagnostics;
//using System.Runtime.InteropServices;
//using Microsoft.Win32.SafeHandles;

//class Program
//{
//    static void Main()
//    {
//        GetHandlesFromCurrentProcess();
//    }

//    static void GetHandlesFromCurrentProcess()
//    {
//        Process currentProcess = Process.GetCurrentProcess();

//        Console.WriteLine($"Handles for the current process (PID {currentProcess.Id}):");

//        foreach (SafeFileHandle handle in GetHandles(currentProcess.Id))
//        {
//            Console.WriteLine($"Handle: {handle.DangerousGetHandle()}");
//        }
//    }

//    static SafeFileHandle[] GetHandles(int processId)
//    {
//        IntPtr processHandle = IntPtr.Zero;

//        try
//        {
//            // Open the process with PROCESS_DUP_HANDLE access rights
//            processHandle = OpenProcess(ProcessAccessFlags.DupHandle, false, processId);

//            // Get the list of handles for the process
//            return GetHandlesFromProcess(processHandle);
//        }
//        finally
//        {
//            if (processHandle != IntPtr.Zero)
//            {
//                CloseHandle(processHandle);
//            }
//        }
//    }

//    static SafeFileHandle[] GetHandlesFromProcess(IntPtr processHandle)
//    {
//        const int bufferSize = 1024;
//        IntPtr buffer = Marshal.AllocHGlobal(bufferSize);

//        try
//        {
//            // Call NtQuerySystemInformation to get information about handles
//            int status = NtQuerySystemInformation(SYSTEM_INFORMATION_CLASS.SystemHandleInformation, buffer, bufferSize, IntPtr.Zero);

//            if (status != 0)
//            {
//                throw new System.ComponentModel.Win32Exception(status);
//            }

//            // Parse the information to get the handles
//            SYSTEM_HANDLE_INFORMATION handleInfo = Marshal.PtrToStructure<SYSTEM_HANDLE_INFORMATION>(buffer);
//            SafeFileHandle[] handles = new SafeFileHandle[handleInfo.NumberOfHandles];

//            for (int i = 0; i < handleInfo.NumberOfHandles; i++)
//            {
//                SYSTEM_HANDLE handle = handleInfo.Handles[i];
//                DuplicateHandle(processHandle, handle.Handle, GetCurrentProcess(), out handles[i], 0, false, DuplicateOptions.DUPLICATE_SAME_ACCESS);
//            }

//            return handles;
//        }
//        finally
//        {
//            Marshal.FreeHGlobal(buffer);
//        }
//    }

//    #region P/Invoke Declarations

//    [DllImport("kernel32.dll", SetLastError = true)]
//    static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

//    [DllImport("kernel32.dll", SetLastError = true)]
//    [return: MarshalAs(UnmanagedType.Bool)]
//    static extern bool CloseHandle(IntPtr hObject);

//    [DllImport("kernel32.dll")]
//    static extern IntPtr GetCurrentProcess();

//    [DllImport("ntdll.dll")]
//    static extern int NtQuerySystemInformation(SYSTEM_INFORMATION_CLASS SystemInformationClass, IntPtr SystemInformation, int SystemInformationLength, IntPtr ReturnLength);

//    [DllImport("kernel32.dll", SetLastError = true)]
//    [return: MarshalAs(UnmanagedType.Bool)]
//    static extern bool DuplicateHandle(IntPtr hSourceProcessHandle, IntPtr hSourceHandle, IntPtr hTargetProcessHandle, out SafeFileHandle lpTargetHandle, uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, DuplicateOptions dwOptions);

//    #endregion
//}

//[Flags]
//enum ProcessAccessFlags : uint
//{
//    DupHandle = 0x00000040,
//}

//[Flags]
//enum DuplicateOptions : uint
//{
//    DUPLICATE_SAME_ACCESS = 0x00000002,
//}

//enum SYSTEM_INFORMATION_CLASS
//{
//    SystemHandleInformation = 0x10,
//}

//[StructLayout(LayoutKind.Sequential)]
//struct SYSTEM_HANDLE
//{
//    public uint ProcessId;
//    public byte ObjectTypeNumber;
//    public byte Flags;
//    public ushort Handle;
//    public IntPtr Object;
//    public uint GrantedAccess;
//}

//[StructLayout(LayoutKind.Sequential)]
//struct SYSTEM_HANDLE_INFORMATION
//{
//    public int NumberOfHandles;
//    public SYSTEM_HANDLE Handles;
//}

