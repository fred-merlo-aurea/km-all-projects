CREATE PROCEDURE [ccp_SAETB_Demo33_AfterDQM]
@SourceFileID int,
@ProcessCode varchar(50),
@ClientId int = 22,
@FileName VARCHAR(100) = ''
AS
BEGIN

	set nocount on

	/**  
	*** Update PhonePermission if Phone not blank or null for nasa_active and mdb_active files  
	**/

	IF (@FileName like '%SAETB_AutoGen_Members_NTB258_ENewsletter%')
		BEGIN
	
			UPDATE st
				SET st.PhonePermission = 1
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
				SET st.PhonePermission = 0
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

END
GO