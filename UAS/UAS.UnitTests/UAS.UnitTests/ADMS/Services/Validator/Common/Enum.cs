using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UAS.UnitTests.ADMS.Services.Validator.Common
{
    public enum AcsMailerId
    {
        None = 0,
        BXNDYYQ = 999944317,
        BXNDYXS = 999944318,
        BXNDYWV = 999944319,
        BXNQVJK = 900001537,
        BXNPHZV = 901456232,
        BXNPHYX = 999944320,
        BXNPHXZ = 999944321,
        BXNRRWP = 999944322,
        BXNQVLF = 999944323,
    }

    public enum TransactionCodes
    {
        None = 0,
        Code21 = 21,
        Code31 = 31,
        Code32 = 32,
    }

    public enum RuleCodeName
    {
        None = 0,
        Or = 4,
        And = 5,
        Overwrite = 6,
        BreakFalse = 7,
        BreakTrue = 8,
        BreakAlways = 9,
    }

    public enum SubscriptionType
    {
        Both = 0,
        Digital,
        Print
    }

    public enum AuditCategoryCode
    {
        NQP = 0,
        NQO,
        PI,
        PB,
        PM,
        PQ
    }

    public enum AuditRequestTypeCode
    {
        NRI = 0,
        OTH,
        CDW,
        CDI,
        PDI,
        PDT,
        PDW,
        QDW,
    }
}
