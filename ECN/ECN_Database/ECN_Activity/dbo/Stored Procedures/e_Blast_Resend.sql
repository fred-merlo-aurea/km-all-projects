CREATE PROCEDURE [dbo].[e_Blast_Resend]  
 @blastID int  
AS  
BEGIN  
 DELETE FROM ecn5_communicator..EmailActivityLog WHERE BlastID = @blastID  
 DELETE FROM BlastActivityBounces WHERE BlastID = @blastID  
 DELETE FROM BlastActivityClicks WHERE BlastID = @blastID  
 DELETE FROM BlastActivityConversion WHERE BlastID = @blastID  
 DELETE FROM BlastActivityExceptions WHERE BlastID = @blastID  
 DELETE FROM BlastActivityOpens WHERE BlastID = @blastID  
 DELETE FROM BlastActivityRefer WHERE BlastID = @blastID  
 DELETE FROM BlastActivityResends WHERE BlastID = @blastID  
 DELETE FROM BlastActivitySends WHERE BlastID = @blastID  
 DELETE FROM BlastActivitySuppressed WHERE BlastID = @blastID   
 DELETE FROM BlastActivityUnSubscribes WHERE BlastID = @blastID  
 UPDATE ECN5_COMMUNICATOR..Blast set SendTime = GETDATE(), StatusCode = 'pending', BlastEngineID = NULL, SendTotal = 0, AttemptTotal = 0, SuccessTotal = 0 WHERE StatusCode != 'pending' and BlastID = @blastID
 
END
