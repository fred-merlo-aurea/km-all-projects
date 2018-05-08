CREATE PROCEDURE [dbo].[job_CodesheetValidation_Delete]
@SourceFileID int,
@ProcessCode varchar(50)
AS
BEGIN   

	SET NOCOUNT ON 

--Delete SDT
	DELETE sdt 
	FROM SubscriberDemographicTransformed sdt
		JOIN SubscriberTransformed st ON sdt.STRecordIdentifier = st.STRecordIdentifier 
	WHERE st.SourceFileID = @SourceFileID AND st.ProcessCode = @ProcessCode 
	
--Delete SDI
	DELETE sdi 
	FROM SubscriberDemographicInvalid sdi
		JOIN SubscriberInvalid si ON sdi.SIRecordIdentifier = si.SIRecordIdentifier 
	WHERE si.SourceFileID = @SourceFileID AND si.ProcessCode = @ProcessCode 
	
--Delete SI
	DELETE SubscriberInvalid 
	WHERE SourceFileID = @SourceFileID AND ProcessCode = @ProcessCode 
	
--Delete ST
	DELETE SubscriberTransformed 
	WHERE SourceFileID = @SourceFileID AND ProcessCode = @ProcessCode 
	
--Delete ImportError
	DELETE ImportError 
	WHERE SourceFileID = @SourceFileID AND ProcessCode = @ProcessCode 

END
GO