CREATE PROCEDURE [dbo].[e_MarketingAutomation_Save]
	@MarketingAutomationID int,
	@Name varchar(500),
	@State varchar(100),
	@IsDeleted bit = 0,
	@Goal varchar(MAX),
	@StartDate datetime,
	@EndDate datetime,
	@JSONDiagram varchar(MAX),
	@CreatedDate datetime = null,
	@CreatedUserID int = null,
	@UpdatedDate datetime = null,
	@UpdatedUserID int = null,
	@CustomerID int
	AS
	BEGIN
		if @MarketingAutomationID > 0--Update
		BEGIN
			UPDATE MarketingAutomation
			SET Name = @Name,
				State = @State,
				IsDeleted = @IsDeleted,
				Goal = @Goal,
				StartDate = @StartDate,
				EndDate = @EndDate,
				JSONDiagram = @JSONDiagram,
				UpdatedDate = GETDATE(),
				UpdatedUserID = @UpdatedUserID,
				CustomerID = @CustomerID
			WHERE MarketingAutomationID = @MarketingAutomationID
			Select @MarketingAutomationID
		END
		else if @MarketingAutomationID <= 0--Insert
		BEGIN
			INSERT INTO MarketingAutomation(Name, State,CustomerID, IsDeleted, Goal, StartDate, EndDate, JSONDiagram, CreatedDate, CreatedUserID)
			VALUES(@Name, @State, @CustomerID, @IsDeleted, @Goal, @StartDate, @EndDate, @JSONDiagram, GETDATE(), @CreatedUserID)
			Select @@IDENTITY;
		END
	
END