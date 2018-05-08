CREATE PROCEDURE e_Subscriptions_Insert_Email
@Email varchar(100),
@ActivityDate datetime
AS
BEGIN
	
	SET NOCOUNT ON
	
	IF NOT EXISTS(SELECT SubscriptionID FROM Subscriptions with(nolock) WHERE EMAIL = @Email)
		BEGIN
			DECLARE @MaxSeq int = (Select MAX(SEQUENCE)+1 From Subscriptions)

			INSERT INTO Subscriptions (SEQUENCE,EMAIL,EmailExists,SOURCE,PhoneExists,FaxExists,IsExcluded,Latitude,Longitude,IsLatLonValid,LatLonMsg
							,Igrp_No,categoryID,TransactionID,Qdate,Transactiondate)
			VALUES(@MaxSeq,@Email,'true','MailChimp','false','false','false',0,0,'false','No Address'
					,NEWID(),10,10,@ActivityDate,@ActivityDate)
		
			SELECT * 
			FROM Subscriptions 
			WHERE EMAIL = @Email
		END

END