namespace libwhm;

public record W32Hook(IntPtr HookReference, uint EventStart, uint EventEnd, Action<WindowInfo, uint> Action);

public static class Hooks
{
    private static readonly List<W32Hook> hooks = [];

    public static void Add(uint eventId, Action<WindowInfo, uint> action) => Add(eventId, eventId, action);
    public static void Add(uint eventStart, uint eventEnd, Action<WindowInfo, uint> action)
    {
        IntPtr hookPointer = Native.SetWinEventHook(eventStart, eventEnd, IntPtr.Zero, HookCallback, 0, 0, 0);
        if (hookPointer != IntPtr.Zero) hooks.Add(new W32Hook(hookPointer, eventStart, eventEnd, action));
    }

    public static void Remove(uint eventId) => Remove(eventId, eventId);
    public static void Remove(uint eventStart, uint eventEnd)
    {
        W32Hook? hook = hooks.Find(x => x.EventStart == eventStart && x.EventEnd == eventEnd);
        if (hook != null) { hooks.Remove(hook); Native.UnhookWinEvent(hook.HookReference); }
    }

    private static void HookCallback(nint _, uint eventId, IntPtr hWnd, uint objId, uint chlId, uint _1, uint _2)
    {
        W32Hook? hook = hooks.Find(x => x.EventStart == eventId && x.EventEnd == eventId);

        if (hook == null || hook.Action == null || chlId != 0 ||
            objId != 0 || hWnd == 0) { return; }

        hook.Action.Invoke(new WindowInfo(hWnd), eventId);
    }
}

public enum EVENT : uint
{
    WINDOW_TITLE_CHANGED = 0x800C,
    FOREGROUND_CHANGED = 0x0003,
    WINDOW_OPENED = 0x8000,
    WINDOW_CLOSED = 0x8001,
}