using System.Collections.Generic;

namespace UAS.UnitTests.Interfaces
{
    public interface ITransactionCode
    {
        bool Exists(int transactionCodeTypeID, int transactionCodeValue);
        int Save(FrameworkUAD_Lookup.Entity.TransactionCode x);
        List<FrameworkUAD_Lookup.Entity.TransactionCode> Select();
        FrameworkUAD_Lookup.Entity.TransactionCode Select(int transactionCodeTypeID, int transactionCodeValue);
        List<FrameworkUAD_Lookup.Entity.TransactionCode> SelectActiveIsFree(bool isFree);
        FrameworkUAD_Lookup.Entity.TransactionCode SelectTransactionCodeValue(int transactionCodeValue);
    }
}