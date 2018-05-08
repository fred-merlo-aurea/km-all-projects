CREATE PROCEDURE [dbo].[e_PubSubscriptions_Select_Paid_Info]
(
	@Filters TEXT = '<XML><Filters></Filters></XML>',
	@AdHocFilters TEXT = '<XML></XML>'
)
AS
BEGIN

	SET NOCOUNT ON

	--DECLARE @Filters varchar(max) = '<XML><Filters><ProductID>1</ProductID></Filters></XML>'
	--DECLARE @AdHocFilters varchar(max) = '<XML><FilterDetail><FilterField>SequenceID</FilterField><FilterObjectType>Standard</FilterObjectType><SearchCondition>Equal</SearchCondition><AdHocFieldValue>15219</AdHocFieldValue></FilterDetail></XML>'
	
	IF 1=0 
		BEGIN
			SET FMTONLY OFF
		END
	
	CREATE TABLE #Subscriptions (PubSubscriptionID int)
	INSERT INTO #Subscriptions
	EXEC rpt_GetSubscriptionIDs_Copies_From_Filter_XML @Filters, @AdHocFilters
	DECLARE @QSourceID int
	SET @QSourceID = (SELECT CodeTypeID FROM UAD_Lookup..CodeType WHERE CodeTypeName = 'Qualification Source')
	
	SELECT SequenceID, 
		sp.TotalIssues, sp.Term, sp.StartIssueDate, sp.ExpireIssueDate, sp.Amount, sp.PaidDate, cPay.DisplayName as 'PaymentType', cCard.DisplayName as 'CreditCardType', 
		sp.CheckNumber, sp.CCNumber, sp.CCExpirationMonth, sp.CCExpirationYear, sp.CCHolderName
	FROM PubSubscriptions ps  WITH (NOLOCK)
		JOIN #Subscriptions s ON s.PubSubscriptionID = ps.PubSubscriptionID
		LEFT JOIN SubscriptionPaid sp WITH (NOLOCK) ON sp.PubSubscriptionID = ps.PubSubscriptionID
		LEFT JOIN UAD_Lookup..Code cPay WITH (NOLOCK) ON cPay.CodeId = sp.PaymentTypeID
		LEFT JOIN UAD_Lookup..Code cCard WITH (NOLOCK) ON cCard.CodeId = sp.CreditCardTypeID
	
	DROP TABLE #Subscriptions

END