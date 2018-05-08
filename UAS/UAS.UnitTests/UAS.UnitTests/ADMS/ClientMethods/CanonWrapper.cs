using System;
using Core.ADMS.Events;
using KMPlatform.Entity;
using UAS.UnitTests.Helpers;

namespace UAS.UnitTests.ADMS.ClientMethods
{
    public partial class CanonTest
    {
        private class CanonWrapper
        {
            private readonly Type _canonType;
            private readonly object _canon;
            public CanonWrapper()
            {
                _canonType = Type.GetType("ADMS.ClientMethods.Canon,ADMS");
                if (_canonType == null)
                {
                    throw new TypeInitializationException(typeof(CanonWrapper).FullName, null);
                }

                _canon = Activator.CreateInstance(_canonType, nonPublic: true);
            }
            public void CanonLookupAdHocImport(Client client, FileMoved fileMoved)
            {
                _canonType.CallMethod("CanonLookupAdHocImport", new object[]{ client, fileMoved }, _canon);
            }

            public void CannonTop100AdHocImport(Client client, FileMoved fileMoved)
            {
                _canonType.CallMethod("CannonTop100AdHocImport", new object[] { client, fileMoved }, _canon);
            }
        }
    }
}
