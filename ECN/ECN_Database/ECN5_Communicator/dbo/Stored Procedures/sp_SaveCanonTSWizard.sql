CREATE Proc [dbo].[sp_SaveCanonTSWizard]            
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
	@SuppressionGroupIDs varchar(255)
)    
as    
Begin    
    if @StepNo = 0 
	Begin
			Insert into Wizard     
				(WizardName, EmailSubject, FromName, FromEmail, ReplyTo, CreatedUserID, GroupID, ContentID, LayoutID, BlastID, FilterID, StatusCode, CompletedStep)     
			values    
				(@WizardName, @EmailSubject, @FromName, @FromEmail, @ReplyTo, @UserID, @GroupID, @ContentID, @LayoutID, @BlastID, 0, 'pending', 0)    

			set @WizardID = @@IDENTITY    

	End
	else if @StepNo = 1    
	Begin    
		if @WizardID = 0    
		Begin    
			Insert into Wizard     
				(WizardName, EmailSubject, FromName, FromEmail, ReplyTo, CreatedUserID, GroupID, ContentID, LayoutID, BlastID, FilterID, StatusCode, CompletedStep)     
			values    
				(@WizardName, @EmailSubject, @FromName, @FromEmail, @ReplyTo, @UserID, 0, 0, 0, 0, 0, 'pending', @StepNo)    

			set @WizardID = @@IDENTITY    
		End    
		Else    
		Begin    
			Update	Wizard    
			Set		WizardName=@WizardName,     
					EmailSubject=@EmailSubject,     
					FromName=@FromName,     
					FromEmail=@FromEmail,     
					ReplyTo=@ReplyTo,     
					UpdatedDate=getdate(),
					UpdatedUserID = @UserID
			where     
					WizardID = @WizardID    
		End    
	End    
	else if @StepNo = 2    
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
	else if @StepNo = 3    
	Begin    
		Update	Wizard    
		Set		ContentID = @ContentID,    
				LayoutID = @LayoutID,     
				CompletedStep = (case when completedStep > @StepNo then completedStep else @StepNo end),    
				UpdatedDate=getdate() ,
				UpdatedUserID = @UserID    
		where     
				WizardID = @WizardID      
	End    
	else if @StepNo = 4    
	Begin    
		Update	Wizard    
		Set		CompletedStep = (case when completedStep > @StepNo then completedStep else @StepNo end),    
				UpdatedDate=getdate(),
				UpdateduserID = @UserID     
		where     
				WizardID = @WizardID      
		End    
	else if @StepNo = 5    
	Begin 
 
		Update	Wizard    
		Set		BlastID			= @BlastID,
				StatusCode		= @status,
				CompletedStep	= (case when  completedStep > @StepNo then completedStep else @StepNo end),    
				UpdatedDate		= getdate(),
				UpdateduserID = @UserID
		where     
				WizardID = @WizardID      
	End    

	Select @WizardID    
End
