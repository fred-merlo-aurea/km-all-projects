CREATE PROCEDURE [dbo].[ccp_France_BeforeDQM]
@SourceFileID int,
@ProcessCode varchar(50) = '',
@ClientId int = 8
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	update SubscriberTransformed
	set qdate = getdate()
	where (qdate is null or qdate = '1900-01-01 00:00:00.000')

	update SubscriberTransformed
	set TransactionDate = QDate
	where (TransactionDate is null or TransactionDate = '1900-01-01 00:00:00.000')
	
END