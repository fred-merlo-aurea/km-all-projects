CREATE PROCEDURE [dbo].[ccp_BriefMedia_BeforeDQM]
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @err_message VARCHAR(255)
	BEGIN TRY
		--fix qdates
		update SubscriberTransformed
		set QDate = '2014-01-15'
		where (QDate<'1990-01-01' or QDate>'2019-01-01' or isnull(qdate,'')='')

		--fix bad addresses
		UPDATE SubscriberTransformed SET [Address] = REPLACE([Address], '%&#%',''),
			[MailStop] =  REPLACE([MailStop], '%&#%',''),
			[City] = REPLACE([City], '%&#%','')

	END TRY
	BEGIN CATCH
		set @err_message = ERROR_MESSAGE();
		PRINT(@err_message);
	END CATCH
END