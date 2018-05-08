CREATE PROCEDURE [dbo].[e_SubscriberFinal_FinalizeForMasterDB]
	@ProcessCode varchar(50),
	@SourceFileID int
AS
BEGIN

	SET NOCOUNT ON

-- default values on subscriberFinal
	UPDATE SubscriberFinal
	SET MailPermission = CASE WHEN IsMailable = 0 THEN 0
					  when ISNULL(MailPermission,'')='' then 1 else MailPermission end,
		FaxPermission = case when ISNULL(FaxPermission,'')='' then 1 else FaxPermission end,
		PhonePermission = case when ISNULL(PhonePermission,'')='' then 1 else PhonePermission end,
		OtherProductsPermission = case when ISNULL(OtherProductsPermission,'')='' then 1 else OtherProductsPermission end,
		ThirdPartyPermission = case when ISNULL(ThirdPartyPermission,'')='' then 1 else ThirdPartyPermission end,
		EmailRenewPermission = case when ISNULL(EmailRenewPermission,'')='' then 1 else EmailRenewPermission end,
		TextPermission = case when ISNULL(TextPermission,'')='' then 1 else TextPermission end,
		CategoryID = case when ISNULL(CategoryId,'')='' OR CategoryID = 0 then 10 else CategoryID end,
		TransactionID = case when ISNULL(TransactionID,'')='' OR TransactionID = 0 then 10 else TransactionID end,
		QSourceId = case when isnull(QsourceId,'')='' then 3 else QsourceId end,
		TransactionDate = CASE WHEN ISNULL(TransactionDate,'')='' and ISNULL(qdate,'')!='' THEN QDATE
							   WHEN ISNULL(TransactionDate,'')='' and ISNULL(qdate,'')='' THEN GETDATE() ELSE TransactionDate END
	WHERE ProcessCode = @ProcessCode and SourceFileID = @SourceFileId

END