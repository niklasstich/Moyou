﻿using Moyou.Aspects.Memento;

namespace Moyou.CompileTimeTest.MementoTests.RestoreHookAttributeTests;

[Memento]
internal class HasNoParameters
{
    [MementoRestoreHook]
    public void RestoreMementoHook()
    {
    }

    private record Memento
    {

    }
}
