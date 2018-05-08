CREATE PROCEDURE [ccp_SAETB_Demo32_AfterDQM]
@SourceFileID int,
@ProcessCode varchar(50),
@ClientId int = 22,
@FileName VARCHAR(100) = ''
AS
BEGIN

	set nocount on

	/**  
	*** Update FaxPermission if Fax not blank or null for nasa_active and mdb_active files  
	**/

	IF (@FileName like '%nasa_active%' or @FileName like '%mdb_active%' or @FileName like '%SAETB_AutoGen_Members_NTB258_ENewsletter%')
		BEGIN
	
			UPDATE st
				SET st.FaxPermission = 1
				FROM SAETBMasterDB..SubscriberTransformed st WITH(NOLOCK)				
				WHERE 
				(
					ISNULL(Fax,'') != '' 
					/** 
					*** Start of more validation rather than just data is there 
					**/
					--and LEN(ISNULL(Fax,'')) > 6
					--and ISNUMERIC(ISNULL(Fax,'')) = 1
				)
				and SourceFileID = @SourceFileID

			UPDATE st
				SET st.FaxPermission = 0
				FROM SAETBMasterDB..SubscriberTransformed st WITH(NOLOCK)				
				WHERE 
				(
					ISNULL(Fax,'') = '' 
					/** 
					*** Start of more validation rather than just data is there 
					**/
					--and LEN(ISNULL(Fax,'')) > 6
					--and ISNUMERIC(ISNULL(Fax,'')) = 1
				)
				and SourceFileID = @SourceFileID
		END

END
GO