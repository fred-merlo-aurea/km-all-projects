CREATE PROCEDURE [dbo].[NEBook_CopySurveys]
	@SurveyID int,
	@CustomerIDs varchar(2000)
AS
BEGIN
	SET NOCOUNT ON;

	declare @Customers table (CustomerID int, CopiedSurveyID int)

	declare	@CustomerID int,
			@Survey_Title	varchar(100),    
			@Description	varchar(1000),    	
			@UserID			int,    
			@EnableDate		varchar(10),    
			@DisableDate	varchar(10),  
			@newSurveyID	int,
			@groupID		int,
			@surveyCustomerID int,
			@deletesurveyID int

	select @Survey_Title = 	SurveyTitle,
		   @Description	= Description,
		   @EnableDate = EnableDate,		
		   @DisableDate	= DisableDate,
		   @surveyCustomerID = CustomerID
	from  ecn5_collector..survey
	where surveyid = @SurveyID

	insert into @customers (customerID) 
	select items from ecn5_communicator.dbo.fn_split(@CustomerIDs,',')

	
	/* Delete already copied surveys in the store*/

	DECLARE c_deletesurvey CURSOR FOR select CopiedSurveyID from NEBook_CopiedSurveys where  SurveyID = @SurveyID and CopyToCustomerID in (select customerID from @customers)

	OPEN c_deletesurvey  
   
	FETCH NEXT FROM c_deletesurvey INTO @deletesurveyID
   
	WHILE @@FETCH_STATUS = 0  
	BEGIN  
		exec ecn5_collector..sp_deletesurvey @deletesurveyID
		delete from NEBook_CopiedSurveys where SurveyID = @SurveyID and copiedsurveyID = @deletesurveyID

		FETCH NEXT FROM c_deletesurvey INTO @deletesurveyID
	
	End  
   
	CLOSE c_deletesurvey  
	DEALLOCATE c_deletesurvey  


	DECLARE c_deletesurvey CURSOR FOR  SELECT CustomerID from @customers

	OPEN c_deletesurvey  
   
	FETCH NEXT FROM c_deletesurvey INTO @CustomerID
   
	WHILE @@FETCH_STATUS = 0  
	BEGIN  
		Insert into ecn5_collector..Survey (surveytitle, Description, customerid, CreatedUserID, enabledate, disabledate, IntroHTML, ThankyouHTML, IsActive, CompletedStep, CreatedDate, UpdatedUserID)     
		select @Survey_Title, @Description, @CustomerID, (select top 1 userID from ecn5_accounts..users where customerID = @CustomerID),
				case when len(ltrim(rtrim(@EnableDate))) = 0 then NULL else @EnableDate end,     
				case when len(ltrim(rtrim(@DisableDate))) = 0 then NULL else @DisableDate end,
				NULL, NULL, 1, 5, getdate(), (select top 1 userID from ecn5_accounts..users where customerID = @CustomerID)

		set @newSurveyID = @@IDENTITY   
		
		update @Customers set CopiedSurveyID = @newSurveyID where customerID = @CustomerID

		Insert into ecn5_communicator..Groups (CustomerID,FolderID,GroupName,GroupDescription,OwnerTypeCode,MasterSupression,PublicFolder,OptinHTML,OptinFields,AllowUDFHistory)
		values (@CustomerID, 0, replace(@Survey_Title,' ', '_') + '_Group', replace(@Survey_Title,' ', '_') + '_Group', 'customer', 0, 1, '', '', 'N')
		
		set @groupID = @@IDENTITY    

		update ecn5_collector..survey set groupID = @groupID where surveyID = @newSurveyID  

		exec ecn5_collector..sp_copysurvey @newSurveyID, @SurveyID

		FETCH NEXT FROM c_deletesurvey INTO @CustomerID
	
	End  
   
	CLOSE c_deletesurvey  
	DEALLOCATE c_deletesurvey  

	insert into NEBook_CopiedSurveys
	select @surveyCustomerID, @SurveyID, CustomerID, CopiedSurveyID from @Customers where copiedsurveyID is not null

END
