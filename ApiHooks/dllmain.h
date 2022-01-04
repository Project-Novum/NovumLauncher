#pragma once

#define WIN32_LEAN_AND_MEAN             // Exclude rarely-used stuff from Windows headers
// Windows Header Files
#include <windows.h>
#include <iostream>

#pragma comment(lib, "KERNEL32.lib")


static BOOL (WINAPI* native_set_process_affinity_mask)(HANDLE hProcess, DWORD_PTR dwProcessAffinityMask) =
    SetProcessAffinityMask;
static DWORD_PTR (WINAPI* native_set_thread_affinity_mask)(HANDLE hThread, DWORD_PTR dwThreadAffinityMask) =
    SetThreadAffinityMask;

static BOOL (WINAPI* native_create_process_a)(
_In_opt_ LPCSTR lpApplicationName,
_Inout_opt_ LPSTR lpCommandLine,
_In_opt_ LPSECURITY_ATTRIBUTES lpProcessAttributes,
_In_opt_ LPSECURITY_ATTRIBUTES lpThreadAttributes,
_In_ BOOL bInheritHandles,
_In_ DWORD dwCreationFlags,
_In_opt_ LPVOID lpEnvironment,
_In_opt_ LPCSTR lpCurrentDirectory,
_In_ LPSTARTUPINFOA lpStartupInfo,
_Out_ LPPROCESS_INFORMATION lpProcessInformation) = CreateProcessA;


static KAFFINITY affinity = 0x00000000000000FF;

BOOL WINAPI set_process_affinity_mask_hook(HANDLE hProcess, DWORD_PTR dwProcessAffinityMask);

DWORD_PTR WINAPI set_thread_affinity_mask_hook(HANDLE hThread, DWORD_PTR dwThreadAffinityMask);