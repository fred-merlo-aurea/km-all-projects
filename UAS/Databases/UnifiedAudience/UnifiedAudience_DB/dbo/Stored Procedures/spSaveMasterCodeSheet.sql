CREATE proc [dbo].[spSaveMasterCodeSheet]
@MasterID int = 0, 
@MasterGroupID int, 
@MasterValue varchar(100), 
@MasterDesc varchar(255),
@MasterDesc1 varchar(255),
@EnableSearching bit,
@CreatedByUserID int = 0,
@DateCreated datetime = null,
@UpdatedByUserID int = 0,
@DateUpdated datetime = null	
as
BEGIN

	SET NOCOUNT ON    
	
	if (@MasterID =0)
		Begin
			
			DECLARE @SortOrder int
			Select @SortOrder = max(SortOrder) + 1 
			from Mastercodesheet 
			where MasterGroupID = @MasterGroupID

			INSERT INTO [Mastercodesheet]([MasterGroupID], [MasterValue], [MasterDesc], [MasterDesc1], [EnableSearching], [DateCreated] , [CreatedByUserID], [SortOrder])
			VALUES (@MasterGroupID, @MasterValue, @MasterDesc, @MasterDesc1, @EnableSearching, @DateCreated, @CreatedByUserID, @SortOrder)
		End
	Else
		Begin
			update Mastercodesheet
			set MasterGroupID = @MasterGroupID, 
				MasterValue = @MasterValue, 
				MasterDesc = @MasterDesc,
				MasterDesc1 = @MasterDesc1,
				EnableSearching = @EnableSearching,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID				
			where MasterID = @MasterID						
		End	
End