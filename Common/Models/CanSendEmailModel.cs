using System;

namespace Common.Models
{
    public class CanSendEmailModel
    {
        public CanSendEmailModel(bool canSend, TimeSpan delay)
        {
            CanSend = canSend;
            Delay = delay;
        }

        public bool CanSend { get; set; }
        public TimeSpan Delay { get; set; }
    }
}