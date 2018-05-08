using System;
using Core.ADMS.Events;
using FrameworkUAS.Entity;
using KMPlatform.Entity;
using UAS.UnitTests.Helpers;

namespace UAS.UnitTests.ADMS.ClientMethods
{
    internal class StagnitoWrapper
    {
        private readonly Type _canonType;
        private readonly object _canon;
        public StagnitoWrapper()
        {
            _canonType = Type.GetType("ADMS.ClientMethods.Stagnito,ADMS");
            if (_canonType == null)
            {
                throw new TypeInitializationException(typeof(StagnitoWrapper).FullName, null);
            }

            _canon = Activator.CreateInstance(_canonType, nonPublic: true);
        }
        public void EnsambleCompanyIDAdHocImport(Client client, SourceFile sourceFile, ClientCustomProcedure clientCustomProcedure, FileMoved fileMoved)
        {
            _canonType.CallMethod(
                "EnsambleCompanyIDAdHocImport",
                new object[]{ client, sourceFile, clientCustomProcedure, fileMoved },
                _canon);
        }

        public void ViolaCompanyIDAdHocImport(Client client, SourceFile sourceFile, ClientCustomProcedure clientCustomProcedure, FileMoved fileMoved)
        {
            _canonType.CallMethod(
                "ViolaCompanyIDAdHocImport",
                new object[] { client, sourceFile, clientCustomProcedure, fileMoved },
                _canon);
        }
    }
}
