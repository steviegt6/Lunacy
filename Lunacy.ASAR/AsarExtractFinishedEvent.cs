using System;

namespace Lunacy.ASAR
{
    public class AsarExtractFinishedEvent : EventArgs
    {
        public bool Successful { get; }

        public AsarExtractFinishedEvent(bool successful)
        {
            Successful = successful;
        }
    }
}