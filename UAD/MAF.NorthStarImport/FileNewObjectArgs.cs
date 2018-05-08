using System;

namespace MAF.NorthStarImport
{
    public class FileNewObjectArgs
    {
        public FileNewObjectArgs(
            Process myProcess, 
            Subscribe newSub, 
            UpdateEmailStatus upEmailStatus, 
            UpdateEmailStatus spamComplaint, 
            DateTime spamDate, 
            Unsubscribe unSub, 
            DateTime unSubDate)
        {
            MyProcess = myProcess;
            NewSub = newSub;
            UpEmailStatus = upEmailStatus;
            SpamComplaint = spamComplaint;
            SpamDate = spamDate;
            UnSub = unSub;
            UnSubDate = unSubDate;
        }

        public Process MyProcess { get; private set; }
        public Subscribe NewSub { get; private set; }
        public UpdateEmailStatus UpEmailStatus { get; private set; }
        public UpdateEmailStatus SpamComplaint { get; private set; }
        public DateTime SpamDate { get; private set; }
        public Unsubscribe UnSub { get; private set; }
        public DateTime UnSubDate { get; private set; }
    }
}