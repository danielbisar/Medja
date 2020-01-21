using System;

namespace ReadFileContinously
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