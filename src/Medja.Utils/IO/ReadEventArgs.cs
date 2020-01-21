using System;

namespace Medja.Utils.IO
{
    public class ReadEventArgs : EventArgs
    {
        public string Data { get; }

        public ReadEventArgs(string data)
        {
            Data = data;
        }
    }
}