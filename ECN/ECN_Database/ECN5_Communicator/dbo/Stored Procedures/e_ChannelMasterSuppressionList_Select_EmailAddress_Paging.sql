CREATE PROCEDURE [dbo].[e_ChannelMasterSuppressionList_Select_EmailAddress_Paging]   
(  
	@BaseChannelID int,   
	@PageNo int,  
	@PageSize int,
	@EmailSearchString varchar(50)     
)       
as        
  
Begin  

	SET NOCOUNT ON;

	DECLARE @FirstRec int, @LastRec int
	
	if @PageNo = 1
	Begin
		set @FirstRec = 1;
		set @LastRec = @PageSize; 
	End
	Else
	Begin
		set @FirstRec = (@PageNo * @PageSize + 1);
		set @LastRec = (@FirstRec + @PageSize - 1); 
	End
	   
	declare @CMS TABLE (RowNum int, CMSID int , Emailaddress varchar(100), CreatedDate datetime)		

	if LEN(@EmailSearchString) > 0
	begin
		INSERT INTO @CMS 
		SELECT ROW_NUMBER() OVER(ORDER BY EmailAddress ASC), CMSID,EmailAddress, CreatedDate 
		from ChannelMasterSuppressionList  with (NOLOCK) 
		WHERE BaseChannelID = @BaseChannelID and EmailAddress like '%' + @EmailSearchString + '%' and IsDeleted = 0
	End
	Else
	Begin
		INSERT INTO @CMS 
		SELECT ROW_NUMBER() OVER(ORDER BY EmailAddress ASC), CMSID,EmailAddress, CreatedDate 
		from ChannelMasterSuppressionList  with (NOLOCK) 
		WHERE BaseChannelID = @BaseChannelID and IsDeleted = 0
	End
	
		SELECT count(CMSID) from @CMS
	
	--WITH TempResult (RowNum, CMSID, Emailaddress, DateAdded) AS
	--(
	--	SELECT  ROW_NUMBER() OVER(ORDER BY EmailAddress ASC) as 'RowNum', c.CMSID,
	--	EmailAddress, DateAdded		
	--	from	@CMS c join ChannelMasterSuppressionList cms on c.CMSID = cms.CMSID 
	--)				
	--SELECT	CMSID,  EmailAddress, DateAdded FROM TempResult
	--WHERE RowNum >= @FirstRec 
	--AND RowNum <= @LastRec
	
	SELECT	CMSID,  EmailAddress, CreatedDate, @BaseChannelID as 'BaseChannelID'
	FROM @CMS
	WHERE RowNum >= @FirstRec 
	AND RowNum <= @LastRec
	
	SET NOCOUNT OFF;
			
End
