using System.Collections.Generic;
using AMS_Operations;
using FrameworkUAD.Entity;
using UAS.UnitTests.Helpers;

namespace UAS.UnitTests.AMS_Operations.FileValidator_cs
{
    public static class FileValidatorTestExtensions
    {
        private const string HasQualifiedProfileMethod = "HasQualifiedProfile";
        private const string HasQualifiedNameMethod = "HasQualifiedName";
        private const string HasQualifiedAddressMethod = "HasQualifiedAddress";
        private const string HasQualifiedPhoneMethod = "HasQualifiedPhone";
        private const string HasQualifiedCompanyMethod = "HasQualifiedCompany";
        private const string HasQualifiedTiteMethod = "HasQualifiedTitle";
        private const string ErrorMessagesFieldName = "ErrorMessages";

        public static bool CallHasQualifiedCompany(
            this FileValidator validator,
            SubscriberTransformed subscriberTransformed)
        {
            return CallNonPublicMethod<bool>(validator, HasQualifiedCompanyMethod, new object[] { subscriberTransformed });
        }

        public static bool CallHasQualifiedTitle(
            this FileValidator validator,
            SubscriberTransformed subscriberTransformed)
        {
            return CallNonPublicMethod<bool>(validator, HasQualifiedTiteMethod, new object[] { subscriberTransformed });
        }

        public static bool CallHasQualifiedPhone(
            this FileValidator validator,
            SubscriberTransformed subscriberTransformed)
        {
            return CallNonPublicMethod<bool>(validator, HasQualifiedPhoneMethod, new object[] { subscriberTransformed });
        }

        public static bool CallHasQualifiedAddress(
            this FileValidator validator,
            SubscriberTransformed subscriberTransformed)
        {
            return CallNonPublicMethod<bool>(validator, HasQualifiedAddressMethod, new object[] { subscriberTransformed });
        }

        public static bool CallHasQualifiedName(
            this FileValidator validator,
            SubscriberTransformed subscriberTransformed)
        {
            return CallNonPublicMethod<bool>(validator, HasQualifiedNameMethod, new object[] { subscriberTransformed });
        }

        public static IList<SubscriberTransformed> CallHasQualifiedProfile(
            this FileValidator validator,
            IList<SubscriberTransformed> checkList)
        {
            return CallNonPublicMethod<IList<SubscriberTransformed>>(validator, HasQualifiedProfileMethod, new object[] { checkList });
        }

        public static bool ContainsErrorMessagesAtIndex(this FileValidator validator, string message, int index)
        {
            return GetErrorMessages(validator)[index].Contains(message);
        }

        public static IList<string> GetErrorMessages(this FileValidator fileValidator)
        {
            return fileValidator.GetFieldValue(ErrorMessagesFieldName) as List<string>;
        }

        private static T CallNonPublicMethod<T>(this FileValidator validator, string methodName, object[] parameter)
        {
            return (T)typeof(FileValidator).CallMethod(methodName, parameter, validator);
        }
    }
}
