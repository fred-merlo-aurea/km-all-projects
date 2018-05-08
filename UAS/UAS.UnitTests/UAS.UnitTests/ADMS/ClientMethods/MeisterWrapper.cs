using System;
using Core.ADMS.Events;
using FrameworkUAS.Entity;
using UAS.UnitTests.Helpers;

namespace UAS.UnitTests.ADMS.ClientMethods
{
    internal class MeisterWrapper
    {
        private readonly Type _objectType;
        private readonly object _object;

        public MeisterWrapper()
        {
            _objectType = Type.GetType("ADMS.ClientMethods.Meister,ADMS");
            if (_objectType == null)
            {
                throw new TypeInitializationException(typeof(MeisterWrapper).FullName, null);
            }

            _object = Activator.CreateInstance(_objectType, nonPublic: true);
        }

        public void CventOptOuts(KMPlatform.Entity.Client client, SourceFile cSpecialFile, ClientCustomProcedure ccp, FileMoved eventMessage)
        {
            _objectType.CallMethod(
                "CventOptOuts",
                new object[] { client, cSpecialFile, ccp, eventMessage},
                _object);
        }

        public void FipsCounty(KMPlatform.Entity.Client client, SourceFile cSpecialFile, ClientCustomProcedure ccp, FileMoved eventMessage)
        {
            _objectType.CallMethod(
                "FipsCounty",
                new object[] { client, cSpecialFile, ccp, eventMessage },
                _object);
        }

        public void RemoveBadPhoneNumber(KMPlatform.Entity.Client client, SourceFile cSpecialFile, ClientCustomProcedure ccp, FileMoved eventMessage)
        {
            _objectType.CallMethod(
                "RemoveBadPhoneNumber",
                new object[] { client, cSpecialFile, ccp, eventMessage },
                _object);
        }
    }
}
