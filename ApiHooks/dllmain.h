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



static KAFFINITY affinity = 0x00000000000000FF;

BOOL WINAPI set_process_affinity_mask_hook(HANDLE hProcess, DWORD_PTR dwProcessAffinityMask);

DWORD_PTR WINAPI set_thread_affinity_mask_hook(HANDLE hThread, DWORD_PTR dwThreadAffinityMask);