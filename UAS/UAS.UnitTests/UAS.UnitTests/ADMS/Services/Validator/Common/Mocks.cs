using System.Diagnostics.CodeAnalysis;
using Moq;
using UAS.UnitTests.Interfaces;

namespace UAS.UnitTests.ADMS.Services.Validator.Common
{
    [ExcludeFromCodeCoverage]
    public class Mocks
    {
        public Mocks()
        {
            ServiceBase = new Mock<IServiceBase>();
            Product = new Mock<IProduct>();
            EmailStatus = new Mock<IEmailStatus>();
            KmClient = new Mock<IClient>();
            SourceFile = new Mock<ISourceFile>();
            Code = new Mock<ICode>();
            Service = new Mock<IService>();
            AdmsLog = new Mock<IAdmsLog>();
            DQmQue = new Mock<IDqmQue>();
            AddressClean = new Mock<IAddressClean>();
            CategoryCode = new Mock<ICategoryCode>();
            TransactionCode = new Mock<ITransactionCode>();
            ProductSubscriptionsExtensionMapper = new Mock<IProductSubscriptionsExtensionMapper>();
            SubscriberOriginal = new Mock<ISubscriberOriginal>();
            SubscriberTransformed = new Mock<ISubscriberTransformed>();
            Transformation = new Mock<ITransformation>();
            TransformDataMap = new Mock<ITransformDataMap>();
            TransformAssign = new Mock<ITransformAssign>();
            TransformSplit = new Mock<ITransformSplit>();
            TransformJoin = new Mock<ITransformJoin>();
            TransformSplitTrans = new Mock<ITransformSplitTrans>();
            ServiceFeature = new Mock<IServiceFeature>();
        }

        public Mock<IServiceBase> ServiceBase { get; }
        public Mock<IProduct> Product { get; }
        public Mock<IEmailStatus> EmailStatus { get; }
        public Mock<IClient> KmClient { get; }
        public Mock<ISourceFile> SourceFile { get; }
        public Mock<ICode> Code { get; }
        public Mock<IService> Service { get; }
        public Mock<IAdmsLog> AdmsLog { get; }
        public Mock<IDqmQue> DQmQue { get; }
        public Mock<IAddressClean> AddressClean { get; }
        public Mock<ICategoryCode> CategoryCode { get; }
        public Mock<ITransactionCode> TransactionCode { get; }
        public Mock<ISubscriberOriginal> SubscriberOriginal { get; }
        public Mock<ISubscriberTransformed> SubscriberTransformed { get; }
        public Mock<IProductSubscriptionsExtensionMapper> ProductSubscriptionsExtensionMapper { get; }
        public Mock<ITransformation> Transformation { get; }
        public Mock<ITransformAssign> TransformAssign { get; }
        public Mock<ITransformDataMap> TransformDataMap { get; }
        public Mock<ITransformSplit> TransformSplit { get; }
        public Mock<ITransformSplitTrans> TransformSplitTrans { get; }
        public Mock<ITransformJoin> TransformJoin { get; }
        public Mock<IServiceFeature> ServiceFeature { get; }

        public void VerifyNoOtherCalls()
        {
            ServiceBase.VerifyNoOtherCalls();
            Product.VerifyNoOtherCalls();
            EmailStatus.VerifyNoOtherCalls();
            KmClient.VerifyNoOtherCalls();
            SourceFile.VerifyNoOtherCalls();
            Code.VerifyNoOtherCalls();
            Service.VerifyNoOtherCalls();
            AdmsLog.VerifyNoOtherCalls();
            DQmQue.VerifyNoOtherCalls();
            AddressClean.VerifyNoOtherCalls();
            CategoryCode.VerifyNoOtherCalls();
            TransactionCode.VerifyNoOtherCalls();
            ProductSubscriptionsExtensionMapper.VerifyNoOtherCalls();
            SubscriberOriginal.VerifyNoOtherCalls();
            SubscriberTransformed.VerifyNoOtherCalls();
            Transformation.VerifyNoOtherCalls();
            TransformAssign.VerifyNoOtherCalls();
            TransformJoin.VerifyNoOtherCalls();
            TransformSplit.VerifyNoOtherCalls();
            TransformDataMap.VerifyNoOtherCalls();
            TransformSplitTrans.VerifyNoOtherCalls();
            ServiceFeature.VerifyNoOtherCalls();
        }

        public void ResetCalls()
        {
            ServiceBase.ResetCalls();
            Product.ResetCalls();
            EmailStatus.ResetCalls();
            KmClient.ResetCalls();
            SourceFile.ResetCalls();
            Code.ResetCalls();
            Service.ResetCalls();
            AdmsLog.ResetCalls();
            DQmQue.ResetCalls();
            AddressClean.ResetCalls();
            CategoryCode.ResetCalls();
            TransactionCode.ResetCalls();
            ProductSubscriptionsExtensionMapper.ResetCalls();
            SubscriberOriginal.ResetCalls();
            SubscriberTransformed.ResetCalls();
            Transformation.ResetCalls();
            TransformAssign.ResetCalls();
            TransformJoin.ResetCalls();
            TransformSplit.ResetCalls();
            TransformDataMap.ResetCalls();
            TransformSplitTrans.ResetCalls();
            ServiceFeature.ResetCalls();
        }
    }
}
