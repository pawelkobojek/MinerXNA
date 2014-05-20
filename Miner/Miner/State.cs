using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miner
{
    /// <summary>
    /// Wyliczenie określające stan obiektu. Jego wartość mówi, czy obiekt stoi na ziemi, skacze, swobodnie spada lub kopie.
    /// </summary>
    public enum State
    {
        OnGround,
        Jumping,
        Falling,
        Digging
    };
}
