using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using Common.Models;
using Common.StructLayout;
using Common.Wrappers;
using Microsoft.Win32;
using Newtonsoft.Json;
using static Common.NativeMethods;

namespace Common.Utility;

public sealed class Utils
{
    private static Utils _instance;
    private const AllocationType _allocationType = AllocationType.Commit_Reserve;
    private const MemoryProtection _memoryProtection = MemoryProtection.ReadWrite;
    private const FreeType _freeType = FreeType.Release;

    public static Utils Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Utils();
            }

            return _instance;
        }
    }


    public string GameInstallLocation()
    {
        string fullRegLocationPath = "";

        if (Environment.Is64BitOperatingSystem)
        {
            fullRegLocationPath = $"HKEY_LOCAL_MACHINE\\SOFTWARE\\WOW6432Node\\{Constants.RegLocation}";
        }
        else
        {
            fullRegLocationPath = $"HKEY_LOCAL_MACHINE\\SOFTWARE\\{Constants.RegLocation}";
        }

        string? installLocation = Registry.GetValue(fullRegLocationPath, "InstallLocation", null) as string;
        string? displayName = Registry.GetValue(fullRegLocationPath, "DisplayName", null) as string;

        if (installLocation == null || displayName == null)
        {
            throw new Exception("Error while finding the game installation location from Registry");
        }

        return $"{installLocation}\\{displayName}";
    }

    public void WriteToMemory(IntPtr hProcess, IntPtr address, byte[] patchBytes, int patchSize)
    {
        MemoryAccessWrapper.VirtualProtectEx(hProcess, address, new UIntPtr((uint)patchSize), 0x00000004,
            out uint lpflOldProtect);

        MemoryAccessWrapper.WriteProcessMemory(hProcess, address, patchBytes, patchSize, out IntPtr _);


        MemoryAccessWrapper.VirtualProtectEx(hProcess, address, new UIntPtr((uint)patchSize), lpflOldProtect,
            out lpflOldProtect);
    }
    
    
    public void InjectDllAndResumeThread(IntPtr hProcess, IntPtr hThread, string dllName)
    {
        IntPtr moduleHandle = GetModuleHandle("kernel32.dll");
        if (moduleHandle == IntPtr.Zero) throw new Win32Exception(Marshal.GetLastWin32Error());


        IntPtr loadLibraryAdder = GetProcAddress(moduleHandle, "LoadLibraryA");

        if (loadLibraryAdder == IntPtr.Zero) throw new Win32Exception(Marshal.GetLastWin32Error());


        IntPtr allocMemAddress = VirtualAllocEx(hProcess, IntPtr.Zero,
            (uint)((dllName.Length + 1) * Marshal.SizeOf(typeof(char))), _allocationType, _memoryProtection);

        if (allocMemAddress == IntPtr.Zero) throw new Win32Exception(Marshal.GetLastWin32Error());


        MemoryAccessWrapper.WriteProcessMemory(hProcess, allocMemAddress, Encoding.Default.GetBytes(dllName),
            dllName.Length + 1, out IntPtr _);


        IntPtr remoteThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, loadLibraryAdder,
            allocMemAddress, 0,
            out IntPtr _);

        if (remoteThread == IntPtr.Zero) throw new Win32Exception(Marshal.GetLastWin32Error());


        if (WaitForSingleObject(remoteThread, 80000) == 0x00000000)
            ResumeThread(hThread);
        else
            throw new Exception("Wait for Single Object timed out");


        VirtualFreeEx(hProcess, allocMemAddress, 0, _freeType);

        CloseHandle(hProcess);
        CloseHandle(hThread);
    }


    public string GetHostNameIP(string hostname)
    {
        IPAddress[] ipAddresses = Dns.GetHostAddresses(hostname);

        return ipAddresses.First(a => a.AddressFamily == AddressFamily.InterNetwork).ToString();
    }

    public string GetSha1Hash(string filePath)
    {
        string result = "";

        using (SHA1 sha1 = SHA1.Create())
        {
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                byte[] sha1Bytes = sha1.ComputeHash(fileStream);
                
                foreach (byte b in sha1Bytes) result += b.ToString("x2");
            }
        }

        return result;
    }


    public ServerInfoModel GetSelectedServer()
    {
        ServerInfoModel serverInfoModel = JsonConvert.DeserializeObject<ServerInfoModel>(
            File.ReadAllText($"{GameInstallLocation()}\\SelectedServer.json"));

        return serverInfoModel;
    }

    /// <summary>
    /// Source: https://bitbucket.org/Ioncannon/project-meteor-server/raw/4762811347383c2ef6a013eca643e27176f1c22d/Launcher%20Editor/Program.cs
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public string FFXIVLoginStringDecode(byte[] data)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        while (true)
        {
            string result = "";
            uint key = (uint)data[0] << 8 | data[1];
            uint key2 = data[2];
            key = RotateRight(key, 1) & 0xFFFF;
            key -= 0x22AF;
            key &= 0xFFFF;
            key2 = key2 ^ key;
            key = RotateRight(key, 1) & 0xFFFF;
            key -= 0x22AF;
            key &= 0xFFFF;
            uint finalKey = key;
            key = data[3];
            uint count = (key2 & 0xFF) << 8;
            key = key ^ finalKey;
            key &= 0xFF;
            count |= key;

            int count2 = 0;
            while (count != 0)
            {
                uint encrypted = data[4 + count2];
                finalKey = RotateRight(finalKey, 1) & 0xFFFF;
                finalKey -= 0x22AF;
                finalKey &= 0xFFFF;
                encrypted = encrypted ^ (finalKey & 0xFF);

                result += (char)encrypted;
                count--;
                count2++;
            }

            return result;
            //offset += 4 + count2;
        }
    }
    
    /// <summary>
    /// Source : https://bitbucket.org/Ioncannon/project-meteor-server/raw/4762811347383c2ef6a013eca643e27176f1c22d/Launcher%20Editor/Program.cs
    /// </summary>
    /// <param name="key"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    public byte[] FFXIVLoginStringEncode(uint key, string text)
    {
        key = key & 0xFFFF;

        uint count = 0;
        byte[] asciiBytes = Encoding.ASCII.GetBytes(text);
        byte[] result = new byte[4 + text.Length];
        for (count = 0; count < text.Length; count++)
        {
            result[result.Length - count - 1] = (byte)(asciiBytes[asciiBytes.Length - count - 1] ^ (key & 0xFF));
            key += 0x22AF;
            key &= 0xFFFF;
            key = RotateLeft(key, 1);
            key &= 0xFFFF;
        }

        count = count ^ key;
        result[3] = (byte)(count & 0xFF);

        key += 0x22AF;
        key &= 0xFFFF;
        key = RotateLeft(key, 1);
        key &= 0xFFFF;

        result[2] = (byte)(key & 0xFF);

        key += 0x22AF;
        key &= 0xFFFF;
        key = RotateLeft(key, 1);
        key &= 0xFFFF;

        result[1] = (byte)(key & 0xFF);
        result[0] = (byte)((key >> 8) & 0xFF);

        return result;
    }

    private uint RotateLeft(uint value, int bits)
    {
        return (value << bits) | (value >> (16 - bits));
    }

    private uint RotateRight(uint value, int bits)
    {
        return (value >> bits) | (value << (16 - bits));
    }
}