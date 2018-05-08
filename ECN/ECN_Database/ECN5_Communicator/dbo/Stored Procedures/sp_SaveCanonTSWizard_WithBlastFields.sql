--3 steps for champion, 5 steps for regular and 6 steps for a/b
CREATE Proc [dbo].[sp_SaveCanonTSWizard_WithBlastFields]            
(              
	@WizardID int,    
	@StepNo  int, 
	@WizardName varchar(100),    
	@EmailSubject varchar(255),    
	@FromName varchar(100),    
	@FromEmail varchar(100),    
	@ReplyTo varchar(100),    
	@status varchar(25),
	@UserID int,    
	@GroupID int,    
	@ContentID int,    
	@LayoutID int,    
	@BlastID int,    
	@FilterID int ,
	@CardHolderName varchar(100), 
	@CardType varchar(20), 
	@CardNumber varchar(50), 
	@CardcvNumber varchar(50), 
	@CardExpiration varchar(50),
	@TransactionNo varchar(50),
	@MultiGroupIDs varchar(2000),
	@SuppressionGroupIDs varchar(255),
	@Field3 varchar(255),
	@Field4 varchar(255),
	@Field5 varchar(255),	
	@BlastType varchar(10),
	@EmailSubject2 varchar(255),
	@ContentID2 int,
	@LayoutID2 int,
	@SampleWizardID int,
	@DynamicFromEmail varchar(100) = '',
	@DynamicFromName varchar(100) = '',
	@DynamicReplyToEmail varchar(100) = ''
)    
as    
Begin    
    if @StepNo = 0 
	Begin
			Insert into Wizard     
				(WizardName, EmailSubject, FromName, FromEmail, ReplyTo, CreatedUserID, GroupID, ContentID, LayoutID, BlastID, FilterID, StatusCode, CompletedStep, BlastType, EmailSubject2, SampleWizardID, DynamicFromEmail, DynamicFromName, DynamicReplyToEmail)     
			values    
				(@WizardName, @EmailSubject, @FromName, @FromEmail, @ReplyTo, @UserID, @GroupID, @ContentID, @LayoutID, @BlastID, 0, 'pending', 0, @BlastType, @EmailSubject2, @SampleWizardID, @DynamicFromEmail, @DynamicFromName, @DynamicReplyToEmail)       

			set @WizardID = @@IDENTITY  
			
			if @Field3 <> '' or @Field4 <> '' or @Field5 <> ''
			Begin
				Insert into WizardBlastFields     
					(WizardID, Field3, Field4, Field5)     
				values    
					(@WizardID, @Field3, @Field4, @Field5)
			End  

	End
	else if @StepNo = 1    
	Begin    
		if @WizardID = 0    
		Begin    
			Insert into Wizard     
				(WizardName, EmailSubject, FromName, FromEmail, ReplyTo, CreatedUserID, GroupID, ContentID, LayoutID, BlastID, FilterID, StatusCode, CompletedStep, BlastType, EmailSubject2, SampleWizardID, DynamicFromEmail, DynamicFromName, DynamicReplyToEmail)    
			values    
				(@WizardName, @EmailSubject, @FromName, @FromEmail, @ReplyTo, @UserID, 0, 0, 0, 0, 0, 'pending', @StepNo, @BlastType, @EmailSubject2, @SampleWizardID, @DynamicFromEmail, @DynamicFromName, @DynamicReplyToEmail)          

			set @WizardID = @@IDENTITY  
			
			if @Field3 <> '' or @Field4 <> '' or @Field5 <> ''
			Begin
				Insert into WizardBlastFields     
					(WizardID, Field3, Field4, Field5)     
				values    
					(@WizardID, @Field3, @Field4, @Field5)
			End   
		End    
		Else    
		Begin    
			Update	Wizard    
			Set		WizardName=@WizardName,     
					EmailSubject=@EmailSubject,     
					FromName=@FromName,     
					FromEmail=@FromEmail,     
					ReplyTo=@ReplyTo,     
					UpdatedDate=getdate() ,
					BlastType=@BlastType,
					EmailSubject2=@EmailSubject2,  
					SampleWizardID = @SampleWizardID,
					DynamicFromEmail = @DynamicFromEmail,
					DynamicFromName = @DynamicFromName,
					DynamicReplyToEmail = @DynamicReplyToEmail,
					UpdatedUserID = @UserID
			where     
					WizardID = @WizardID
					
			Declare @FieldsExist int		
			select @FieldsExist = COUNT(WizardID) from Wizard where WizardID = @WizardID 
			If @FieldsExist > 0
			Begin
				Update	WizardBlastFields    
				Set		Field3=@Field3,     
						Field4=@Field4,     
						Field5=@Field5     
				where     
						WizardID = @WizardID
			End
			Else
			Begin
				if @Field3 <> '' or @Field4 <> '' or @Field5 <> ''
				Begin
					Insert into WizardBlastFields     
						(WizardID, Field3, Field4, Field5)     
					values    
						(@WizardID, @Field3, @Field4, @Field5)
				End  
			End   
		End    
	End    
	else if @StepNo = 2    
	Begin 
		if @BlastType = 'Champion'
		Begin
			Update	Wizard    
			Set		CompletedStep = (case when completedStep > @StepNo then completedStep else @StepNo end),    
					UpdatedDate=getdate() ,
					UpdatedUserID = @UserID    
			where     
					WizardID = @WizardID 
		End
		Else
		Begin	   
			Update	Wizard    
			Set		GroupID = @GroupID,
					FilterID = @FilterID,       
					CompletedStep = (case when completedStep > @StepNo then completedStep else @StepNo end),    
					UpdatedDate=getdate(),
					MultiGroupIDs = @MultiGroupIDs,
					SuppressionGroupIDs = @SuppressionGroupIDs,
					UpdatedUserID = @UserID
			where     
					WizardID = @WizardID  
		End    
	End    
	else if @StepNo = 3    
	Begin
		if @BlastType = 'Champion'
		Begin
			Update	Wizard    
			Set		BlastID			= @BlastID,
					StatusCode		= @status,
					CompletedStep	= (case when  completedStep > @StepNo then completedStep else @StepNo end),    
					UpdatedDate		= getdate(),
					UpdatedUserID = @UserID
			where     
					WizardID = @WizardID  
		End
		Else
		Begin    
			Update	Wizard    
			Set		ContentID = @ContentID,    
					LayoutID = @LayoutID,     
					CompletedStep = (case when completedStep > @StepNo then completedStep else @StepNo end),    
					UpdatedDate=getdate()  ,
					UpdatedUserID = @UserID   
			where     
					WizardID = @WizardID 
		End     
	End    
	else if @StepNo = 4    
	Begin
		if @BlastType = 'A/B'
		Begin
			Update	Wizard    
			Set		ContentID2 = @ContentID2,    
					LayoutID2 = @LayoutID2,     
					CompletedStep = (case when completedStep > @StepNo then completedStep else @StepNo end),    
					UpdatedDate=getdate() ,
					UpdatedUserID = @UserID    
			where     
					WizardID = @WizardID 
		End
		Else
		Begin	    
			Update	Wizard    
			Set		CompletedStep = (case when completedStep > @StepNo then completedStep else @StepNo end),    
					UpdatedDate=getdate() ,
					UpdatedUserID = @UserID    
			where     
					WizardID = @WizardID     
		End 
	End    
	else if @StepNo = 5    
	Begin 
		if @BlastType = 'A/B'
		Begin
			Update	Wizard    
			Set		CompletedStep = (case when completedStep > @StepNo then completedStep else @StepNo end),    
					UpdatedDate=getdate(),
					UpdatedUserID = @UserID     
			where     
					WizardID = @WizardID 
		End
		Else
		Begin
			Update	Wizard    
			Set		BlastID			= @BlastID,
					StatusCode		= @status,
					CompletedStep	= (case when  completedStep > @StepNo then completedStep else @StepNo end),    
					UpdatedDate		= getdate(),
				UpdatedUserID = @UserID
			where     
					WizardID = @WizardID    
		End  
	End    
	else if @StepNo = 6 and @BlastType = 'A/B'    
	Begin 
		Update	Wizard    
		Set		BlastID			= @BlastID,
				StatusCode		= @status,
				CompletedStep	= (case when  completedStep > @StepNo then completedStep else @StepNo end),    
				UpdatedDate		= getdate(),
				UpdatedUserID = @UserID
		where     
				WizardID = @WizardID
	End
	Select @WizardID    
End
