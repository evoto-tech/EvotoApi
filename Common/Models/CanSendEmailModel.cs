using System;

namespace Common.Models
{
    public class CanSendEmailModel
    {
        public CanSendEmailModel(bool canSend, TimeSpan remaining)
        {
            CanSend = canSend;
            Remaining = remaining;
        }

        public bool CanSend { get; set; }
        public TimeSpan Remaining { get; set; }
    }
}