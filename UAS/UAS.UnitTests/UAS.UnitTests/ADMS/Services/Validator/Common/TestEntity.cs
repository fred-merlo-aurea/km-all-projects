using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.Object;
using FrameworkUAS.Entity;
using KMPlatform.Entity;
using ADMS_Validator = ADMS.Services.Validator.Validator;

namespace UAS.UnitTests.ADMS.Services.Validator.Common
{
    [ExcludeFromCodeCoverage]
    public class TestEntity
    {
        public TestEntity() : this(new ADMS_Validator())
        {
        }

        public TestEntity(ADMS_Validator validator)
        {
            Validator = validator;
            Mocks = new Mocks();
        }

        public ADMS_Validator Validator { get; }

        public Client Client { get; set; }

        public SaveSubscriber Subscriber { get; set; }

        public SourceFile SourceFile { get; set; }

        public Mocks Mocks { get; }

        public bool IsNullSourceFile { get; set; }

        public bool IsPubCodeValid { get; set; }

        public bool IsProductSubscriberCreated { get; set; }

        public bool ValidateProductSubscription { get; set; }

        public bool ValidateConsensusDemographics { get; set; }

        public bool SaveSubscriberTransformed { get; set; }

        public bool SetProductId { get; set; }

        public bool IsCirculation { get; set; }
    }
}
