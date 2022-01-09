#include "dllmain.h"
#include <atlbase.h>
#include <detours.h>


BOOL WINAPI set_process_affinity_mask_hook(HANDLE hProcess, DWORD_PTR dwProcessAffinityMask)
{
    return native_set_process_affinity_mask(hProcess, affinity);
}

DWORD_PTR WINAPI set_thread_affinity_mask_hook(HANDLE hThread, DWORD_PTR dwThreadAffinityMask)
{
    return native_set_thread_affinity_mask(hThread, affinity);
}




BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
    if (DetourIsHelperProcess())
    {
        return TRUE;
    }
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
        DetourRestoreAfterWith();
        DetourTransactionBegin();
        DetourUpdateThread(GetCurrentThread());
        DetourAttach(&(PVOID&)native_set_process_affinity_mask, set_process_affinity_mask_hook);
        DetourAttach(&(PVOID&)native_set_thread_affinity_mask, set_thread_affinity_mask_hook);
        DetourTransactionCommit();
        break;
    case DLL_THREAD_ATTACH:
        break;
    case DLL_THREAD_DETACH:
        break;
    case DLL_PROCESS_DETACH:
        DetourTransactionBegin();
        DetourUpdateThread(GetCurrentThread());
        DetourDetach(&(PVOID&)native_set_process_affinity_mask, set_process_affinity_mask_hook);
        DetourDetach(&(PVOID&)native_set_thread_affinity_mask, set_thread_affinity_mask_hook);
        DetourTransactionCommit();
        break;
    }
	
    return TRUE;
}


