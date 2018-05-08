using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public class ChampionAudit
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.ChampionAudit;

        public static int Insert(ECN_Framework_Entities.Communicator.ChampionAudit champion, KMPlatform.Entity.User user)
        {
            Validate(champion, user);
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.ChampionAudit.Insert(champion);
                scope.Complete();
            }
            return champion.ChampionAuditID;
        }
        public static bool Exists(int ChampionAuditID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope())
            {
                exists = ECN_Framework_DataLayer.Communicator.ChampionAudit.Exists(ChampionAuditID);
                scope.Complete();
            }

            return exists;
        }
        public static bool ExistsByBlastIDChampion(int BlastIDChampion)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope())
            {
                exists = ECN_Framework_DataLayer.Communicator.ChampionAudit.ExistsByBlastIDChampion(BlastIDChampion);
                scope.Complete();
            }

            return exists;
        }
        public static bool ExistsBySampleID(int SampleID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope())
            {
                exists = ECN_Framework_DataLayer.Communicator.ChampionAudit.ExistsBySampleID(SampleID);
                scope.Complete();
            }

            return exists;
        }
        public static ECN_Framework_Entities.Communicator.ChampionAudit GetByChampionAuditID(int championAuditID)
        {
            ECN_Framework_Entities.Communicator.ChampionAudit championAudit = new ECN_Framework_Entities.Communicator.ChampionAudit();
            using (TransactionScope scope = new TransactionScope())
            {
                championAudit = ECN_Framework_DataLayer.Communicator.ChampionAudit.GetByChampionAuditID(championAuditID);
                scope.Complete();
            }
            return championAudit;
        }
        private static void Validate(ECN_Framework_Entities.Communicator.ChampionAudit champion, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errList = new List<ECNError>();
            if (champion.SampleID == null)
            {
                errList.Add(new ECNError(Entity, Method, "Sample ID cannot be null"));
            }
            else
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if(!ECN_Framework_BusinessLayer.Communicator.Sample.Exists(champion.SampleID.Value,user.CustomerID))
                    {
                        errList.Add(new ECNError(Entity, Method, "AB sample record does not exist"));
                    }
                    scope.Complete();
                }
            }
            if (champion.BlastIDA == null)
            {
                errList.Add(new ECNError(Entity, Method, "the blast A ID cannot be null"));
            }
            else
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!ECN_Framework_BusinessLayer.Communicator.Blast.Exists(champion.BlastIDA.Value, user.CustomerID))
                    {
                        errList.Add(new ECNError(Entity, Method, "Blast A does not exist"));
                    }
                    scope.Complete();
                }
            }
            if (champion.BlastIDB == null)
            {
                errList.Add(new ECNError(Entity, Method, "The blast B ID cannot be null"));
            }
            else
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!ECN_Framework_BusinessLayer.Communicator.Blast.Exists(champion.BlastIDB.Value, user.CustomerID))
                    {
                        errList.Add(new ECNError(Entity, Method, "Blast B does not exist"));
                    }
                    scope.Complete();
                }
            }
            if (champion.BlastIDChampion == null)
            {
                errList.Add(new ECNError(Entity, Method, "The champion blast ID cannot be null"));
            }
            else
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if(!ECN_Framework_BusinessLayer.Communicator.BlastChampion.Exists(champion.BlastIDChampion.Value,user.CustomerID))
                    {
                        errList.Add(new ECNError(Entity, Method, "The champion blast does not exist"));
                    }
                    scope.Complete();
                }
            }
            if (champion.ClicksA < 0)
            {
                errList.Add(new ECNError(Entity, Method, "Clicks for blast A must be at least zero"));
            }
            if (champion.ClicksB < 0)
            {
                errList.Add(new ECNError(Entity, Method, "Clicks for blast B must be at least zero"));
            }
            if (champion.OpensA < 0)
            {
                errList.Add(new ECNError(Entity, Method, "Opens for blast A must be at least zero"));
            }
            if (champion.OpensB < 0)
            {
                errList.Add(new ECNError(Entity, Method, "Opens for blast B must be at least zero"));
            }
            if (champion.BouncesA < 0)
            {
                errList.Add(new ECNError(Entity, Method, "Bounces for blast A must be at least zero"));
            }
            if (champion.BouncesB < 0)
            {
                errList.Add(new ECNError(Entity, Method, "Bounces for blast B must be at least zero"));
            }
            if (champion.BlastIDWinning != champion.BlastIDA && champion.BlastIDWinning != champion.BlastIDB)
            {
                errList.Add(new ECNError(Entity, Method, "The winning blast must be either blast A or blast B"));
            }
            if (champion.SendToNonWinner == null)
            {
                errList.Add(new ECNError(Entity, Method, "Send to nonwinner must be specified"));
            }
            if (champion.Reason == null)
            {
                errList.Add(new ECNError(Entity, Method, "Reason for winner selection must be specified"));
            }

            if (errList.Count > 0)
            {
                throw new ECNException(errList);
            }
        }
    }
}
