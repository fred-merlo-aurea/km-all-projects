namespace SourceMediaPaidPub.Process
{
	public class Constants
	{
		public static readonly string UNITEDSTATESOFAMERICA = "UNITED STATES OF AMERICA";
		public static readonly string CANADA = "CANADA";

		public static readonly string TaxjarURL = "https://api.taxjar.com/v2/rates/";
		public static readonly string ChooseCorrectZipStateForBilling = "Choose Correct Zip State For Billing";

		public const string USCodePattern = @"^\d{5}(?:[-\s]\d{4})?$";
		public const string CAZipCodePattern = @"^([ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ])\ {0,1}(\d[ABCEGHJKLMNPRSTVWXYZ]\d)$";

		public const string ErrorMessageFormsOnlyForRequal = "Form is only for requal";
		public const string ErrorMessageInavlaidSubscription = "Inavlaid Subscription";

	}
}