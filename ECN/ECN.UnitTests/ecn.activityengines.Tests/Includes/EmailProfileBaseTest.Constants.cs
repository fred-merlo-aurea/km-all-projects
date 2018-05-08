﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecn.activityengines.Tests.Includes
{
    partial class EmailProfileBaseTest
    {
        private const string GetFromQueryStringMethodName = "GetFromQueryString";
        private const string EmailIdQueryStringKey = "eID";
        private const string EmailIdErrorMessage = "EmailId specified does not exist.";
        private const string EmailAddressQueryStringKey = "eAD";
        private const string EmailAddressErrorMessage = "EmailAddress specified does not exist.";
        private const string GroupIdQueryStringKey = "gID";
        private const string GroupIdErrorMessage = "GroupId specified does not exist.";
        private const string LoadProfileDataMethodName = "LoadProfileData";

        private const string MessageLabelControlName = "MessageLabel";
        private const string EmailAddressControlName = "EmailAddress";
        private const string TitleControlName = "Title";
        private const string FirstNameControlName = "FirstName";
        private const string LastNameControlName = "LastName";
        private const string FullNameControlName = "FullName";
        private const string CompanyNameControlName = "CompanyName";
        private const string OccupationControlName = "Occupation";
        private const string AddressControlName = "Address";
        private const string Address2ControlName = "Address2";
        private const string CityControlName = "City";
        private const string StateControlName = "State";
        private const string ZipControlName = "Zip";
        private const string CountryControlName = "Country";
        private const string VoiceControlName = "Voice";
        private const string MobileControlName = "Mobile";
        private const string FaxControlName = "Fax";
        private const string IncomeControlName = "Income";
        private const string GenderControlName = "Gender";
        private const string AgeControlName = "Age";
        private const string WebsiteControlName = "Website";
        private const string BirthDateControlName = "BirthDate";
        private const string User1ControlName = "User1";
        private const string User2ControlName = "User2";
        private const string User3ControlName = "User3";
        private const string User4ControlName = "User4";
        private const string User5ControlName = "User5";
        private const string User6ControlName = "User6";
        private const string UserEvent1ControlName = "UserEvent1";
        private const string UserEvent1DateControlName = "UserEvent1Date";
        private const string UserEvent2ControlName = "UserEvent2";
        private const string UserEvent2DateControlName = "UserEvent2Date";
        private const string EditProfileButtonControlName = "EditProfileButton";
        private const string BounceScoreControlName = "BounceScore";
        private const string SoftBounceScoreControlName = "txtSoftBounceScore";
        private const string PasswordControlName = "Password";

        private const string EmailIdColumnName = "EmailID";
        private const string EmailAddressColumnName = "EmailAddress";
        private const string TitleColumnName = "Title";
        private const string FirstNameColumnName = "FirstName";
        private const string LastNameColumnName = "LastName";
        private const string CompanyNameColumnName = "Company";
        private const string OccupationColumnName = "Occupation";
        private const string AddressColumnName = "Address";
        private const string Address2ColumnName = "Address2";
        private const string CityColumnName = "City";
        private const string ZipColumnName = "Zip";
        private const string CountryColumnName = "Country";
        private const string VoiceColumnName = "Voice";
        private const string MobileColumnName = "Mobile";
        private const string FaxColumnName = "Fax";
        private const string WebsiteColumnName = "Website";
        private const string AgeColumnName = "Age";
        private const string IncomeColumnName = "Income";
        private const string GenderColumnName = "Gender";
        private const string User1ColumnName = "User1";
        private const string User2ColumnName = "User2";
        private const string User3ColumnName = "User3";
        private const string User4ColumnName = "User4";
        private const string User5ColumnName = "User5";
        private const string User6ColumnName = "User6";
        private const string NotesColumnName = "Notes";
        private const string UserEvent1ColumnName = "UserEvent1";
        private const string UserEvent2ColumnName = "UserEvent2";
        private const string BirthDateColumnName = "Birthdate";
        private const string UserEvent1DateColumnName = "UserEvent1Date";
        private const string UserEvent2DateColumnName = "UserEvent2Date";
        private const string StateItemColumnName = "State";
        private const string FormatTypeCodeColumnName = "FormatTypeCode";
        private const string SubTypeCodeColumnName = "SubscribeTypeCode";
        private const string GroupIdColumnName = "GroupID";
        private const string DateUpdatedColumnName = "LastChanged";
        private const string BounceScoreColumnName = "BounceScore";
        private const string SoftBounceScoreColumnName = "SoftBounceScore";
        private const string PasswordColumnName = "Password";

        private const string EmailIdValue = "100";
        private const string GroupIdValue = "200";
        private const string EmailAddressValue = "EmailAddressValue";
        private const string TitleValue = "TitleValue";
        private const string FirstNameValue = "FirstNameValue";
        private const string LastNameValue = "LastNameValue";
        private const string FullNameValue = "FullNameValue";
        private const string CompanyNameValue = "CompanyNameValue";
        private const string OccupationValue = "OccupationValue";
        private const string AddressValue = "AddressValue";
        private const string Address2Value = "Address2Value";
        private const string CityValue = "CityValue";
        private const string StateValue = StateControlItem1;
        private const string ZipValue = "ZipValue";
        private const string CountryValue = "CountryValue";
        private const string VoiceValue = "VoiceValue";
        private const string MobileValue = "MobileValue";
        private const string FaxValue = "FaxValue";
        private const string IncomeValue = "IncomeValue";
        private const string GenderValue = GenderControlItem1;
        private const string AgeValue = "AgeValue";
        private const string WebsiteValue = "WebsiteValue";
        private static readonly DateTime BirthDateValue = new DateTime(1990, 12, 20);
        private const string User1Value = "User2Value";
        private const string User2Value = "User1Value"; 
        private const string User3Value = "User3Value";
        private const string User4Value = "User4Value";
        private const string User5Value = "User5Value";
        private const string User6Value = "User6Value";
        private const string UserEvent1Value = "UserEvent1Value";
        private static readonly DateTime UserEvent1DateValue = new DateTime(2018, 4, 4);
        private const string UserEvent2Value = "UserEvent2Value";
        private static readonly DateTime UserEvent2DateValue = new DateTime(2018, 4, 5);
        private const string BounceScoreValue = "BounceScoreValue";
        private const string SoftBounceScoreValue = "SoftBounceScoreValue";
        private const string PasswordValue = "PasswordValue";

        private const string StateControlItem1 = "STATEVALUE1";
        private const string StateControlItem2 = "STATEVALUE2";
        private const string GenderControlItem1 = "GenderValue1";
        private const string GenderControlItem2 = "GenderValue2";
    }
}
