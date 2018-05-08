---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Author:		Rohit Pooserla
-- Create date: 9/18/2012
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateSMSActivity]
(
	@xmldata xml
)AS
BEGIN
	SET NOCOUNT ON;
	declare @dt datetime;	
	set @dt = GETDATE();
	declare @SMSActivityData TABLE (BlastID int, Mobile varchar(10), Carrier varchar(10),SendStatus  varchar(50),SendTime  date,SendID  varchar(50),FinalStatus  varchar(50), Notes varchar(30));	
			
	insert into @SMSActivityData
	SELECT BlastValues.SMS.value('./@BlastID','INT'),
	BlastValues.SMS.value('./@Mobile','varchar(10)'),
	BlastValues.SMS.value('./@Carrier','varchar(10)'),	
	BlastValues.SMS.value('./@SendStatus','varchar(50)'),
	BlastValues.SMS.value('./@SendTime','date'),
	BlastValues.SMS.value('./@SendID','varchar(50)'),	
	BlastValues.SMS.value('./@FinalStatus','varchar(50)'),
	BlastValues.SMS.value('./@Notes','varchar(50)')
	FROM @xmldata.nodes('/Blast') as BlastValues(SMS) ;
		
	
	WITH SMS_CTE (EmailID, CarrierCode,SendStatus,Notes)
		AS
		(
			select distinct e.emailID, sms.Carrier, sms.SendStatus, sms.Notes
			from
				ecn5_communicator..Emails e with (NOLOCK) 
				INNER JOIN 
				@SMSActivityData sms
				ON 
				sms.Mobile = e.Mobile 
		)
		
		UPDATE ecn5_communicator..Emails
		SET Emails.CarrierCode = s.CarrierCode, DateUpdated= @dt
		FROM ecn5_communicator..Emails e join SMS_CTE s on e.EmailID = s.EmailID 	
		
		UPDATE ecn_Activity..SMSActivityLog
		SET SMSActivityLog.SendStatus = s.SendStatus, SMSActivityLog.Notes= s.Notes
		FROM ecn_Activity..SMSActivityLog e join SMS_CTE s on e.EmailID = s.EmailID 	
	
	
END
