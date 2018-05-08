CREATE PROCEDURE [dbo].[e_GlobalMasterSuppressionList_Select_EmailAddress_Paging]   
(  
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
	   
	declare @GMS TABLE (RowNum int, GSID int , Emailaddress varchar(100), CreatedDate datetime)		

	if LEN(@EmailSearchString) > 0
	begin
		INSERT INTO @GMS 
		SELECT ROW_NUMBER() OVER(ORDER BY EmailAddress ASC), GSID, EmailAddress, CreatedDate 
		from GlobalMasterSuppressionList  with (NOLOCK) 
		WHERE EmailAddress like '%' + @EmailSearchString + '%' and IsDeleted = 0
	End
	Else
	Begin
		INSERT INTO @GMS 
		SELECT ROW_NUMBER() OVER(ORDER BY EmailAddress ASC), GSID, EmailAddress, CreatedDate 
		from GlobalMasterSuppressionList  with (NOLOCK) 
		WHERE IsDeleted = 0
	End
	
		SELECT count(GSID) from @GMS
	
	SELECT	GSID,  EmailAddress, CreatedDate
	FROM @GMS
	WHERE RowNum >= @FirstRec 
	AND RowNum <= @LastRec
	
	SET NOCOUNT OFF;
			
End
