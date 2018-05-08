CREATE PROCEDURE [dbo].[ccp_SAETB_Demo33_AfterDQM]
@SourceFileID int
AS

/**  
*** Update Demo33 if Phone not blank or null for nasa_active and mdb_active files  
**/

DECLARE @FileName VARCHAR(100) = (SELECT FileName FROM UAS.dbo.SourceFile WHERE SourceFileID = @SourceFileID)
IF (@FileName like '%SAETB_AutoGen_Members_NTB258_ENewsletter%')
BEGIN
	
	UPDATE st
		SET st.Demo33 = 1
		FROM SAETBMasterDB..SubscriberTransformed st WITH(NOLOCK)				
		WHERE 
		(
			ISNULL(Phone,'') != '' 
			/** 
			*** Start of more validation rather than just data is there 
			**/
			--and LEN(ISNULL(Phone,'')) > 6
			--and ISNUMERIC(ISNULL(Phone,'')) = 1
		)
		and SourceFileID = @SourceFileID

	UPDATE st
		SET st.Demo33 = 0
		FROM SAETBMasterDB..SubscriberTransformed st WITH(NOLOCK)				
		WHERE 
		(
			ISNULL(Phone,'') = '' 
			/** 
			*** Start of more validation rather than just data is there 
			**/
			--and LEN(ISNULL(Phone,'')) > 6
			--and ISNUMERIC(ISNULL(Phone,'')) = 1
		)
		and SourceFileID = @SourceFileID
END
