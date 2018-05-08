using System;
using Core.ADMS.Events;
using UAS.UnitTests.Helpers;

namespace UAS.UnitTests.ADMS.ClientMethods
{
    internal class ScrantonWrapper
    {
        private readonly Type _objectType;
        private readonly object _object;

        public ScrantonWrapper()
        {
            _objectType = Type.GetType("ADMS.ClientMethods.Scranton,ADMS");
            if (_objectType == null)
            {
                throw new TypeInitializationException(typeof(ScrantonWrapper).FullName, null);
            }

            _object = Activator.CreateInstance(_objectType, nonPublic: true);
        }

        public void SurveyAdHocImport(FileMoved fileMoved)
        {
            _objectType.CallMethod(
                "SurveyAdHocImport",
                new object[] {fileMoved},
                _object);
        }

        public void SurveyDomainsImport(FileMoved fileMoved)
        {
            _objectType.CallMethod(
                "SurveyDomainsImport",
                new object[] { fileMoved },
                _object);
        }
    }
}
