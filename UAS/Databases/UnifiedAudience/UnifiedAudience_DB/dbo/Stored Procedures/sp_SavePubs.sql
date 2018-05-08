CREATE PROCEDURE [dbo].[sp_SavePubs]
@PubID int, 
@PubName varchar(100), 
@PubCode varchar(50), 
@PubTypeID int,
@EnableSearching bit,
@Score int=null,
@HasPaidRecords bit,
@CreatedByUserID int = 0,
@DateCreated datetime = null,
@UpdatedByUserID int = 0,
@DateUpdated datetime = null,
@IsActive bit,
@IsUAD bit,
@IsCirc bit,
@UseSubGen bit,
@FrequencyID int = 0,
@YearStartDate varchar(5),
@YearEndDate varchar(5)
AS
BEGIN

	SET NOCOUNT ON

	if @PubID > 0
		begin
			UPDATE Pubs 
			SET PubName = @PubName, 
				PubCode = @PubCode, 
				PubTypeID = @PubTypeID, 
				EnableSearching = @EnableSearching, 
				score = @Score, 
				HasPaidRecords = @HasPaidRecords, 
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID,
				IsActive = @IsActive,
				IsUAD = @IsUAD,
				IsCirc = @IsCirc,
				UseSubGen = @UseSubGen,
				FrequencyID = @FrequencyID,
				YearStartDate = @YearStartDate,
				YearEndDate = @YearEndDate				
			WHERE PubID = @PubID
		
			SELECT @PubID
		end
	else
		begin
			--DECLARE @SortOrder int
	       
			--SELECT @SortOrder = MAX(sortOrder)+1 from Pubs where PubTypeID = @PubTypeID
	       
			INSERT INTO Pubs (PubName, PubCode, PubTypeID, EnableSearching, Score, SortOrder, HasPaidRecords, DateCreated, CreatedByUserID, IsActive, IsUAD, IsCirc, UseSubGen, FrequencyID, YearStartDate, YearEndDate) 
			VALUES (@PubName, @PubCode, @PubTypeID, @EnableSearching, @Score, null, @HasPaidRecords, @DateCreated, @CreatedByUserID,  @IsActive, @IsUAD, @IsCirc, @UseSubGen, @FrequencyID, @YearStartDate, @YearEndDate)

			SELECT @@IDENTITY
		end
END