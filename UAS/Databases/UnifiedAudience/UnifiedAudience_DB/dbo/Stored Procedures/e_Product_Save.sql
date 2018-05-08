CREATE PROCEDURE [dbo].[e_Product_Save]
	@PubID int,
	@PubName varchar(100),
	@istradeshow bit,
	@PubCode varchar(50),
	@PubTypeID int,
	@GroupID int,
	@EnableSearching bit,
	@score int,
	@SortOrder int,
	@DateCreated datetime,
	@DateUpdated datetime,
	@CreatedByUserID int,
	@UpdatedByUserID int,
	@ClientID int,
	@YearStartDate varchar(5),
	@YearEndDate varchar(5),
	@IssueDate datetime,
	@IsImported bit,
	@IsActive bit,
	@AllowDataEntry bit,
	@FrequencyID int,
	@KMImportAllowed bit,
	@ClientImportAllowed bit,
	@AddRemoveAllowed bit,
	@AcsMailerInfoId int,
	@IsUAD bit,
	@IsCirc bit,
	@IsOpenCloseLocked bit = 0,
	@HasPaidRecords bit = 0,
	@UseSubGen bit = 0
AS
BEGIN

	set nocount on

	IF @PubID > 0
		BEGIN
			UPDATE Pubs
				SET PubName = @PubName,
					istradeshow = @istradeshow, 
					PubCode = @PubCode, 
					PubTypeID = @PubTypeID, 
					GroupID = @GroupID, 
					EnableSearching = @EnableSearching, 
					score = @score, 
					SortOrder = @SortOrder,
					DateUpdated = @DateUpdated,
					UpdatedByUserID = @UpdatedByUserID,
					ClientID = @ClientID,
					YearStartDate = @YearStartDate,
					YearEndDate = @YearEndDate,
					IssueDate = @IssueDate,
					IsImported = @IsImported,
					IsActive = @IsActive,
					AllowDataEntry = @AllowDataEntry,
					FrequencyID = @FrequencyID,
					KMImportAllowed = @KMImportAllowed,
					ClientImportAllowed = @ClientImportAllowed,
					AddRemoveAllowed = @AddRemoveAllowed,
					AcsMailerInfoId = @AcsMailerInfoId,
					IsUAD = @IsUAD,
					IsCirc = @IsCirc,
					IsOpenCloseLocked = @IsOpenCloseLocked,
					HasPaidRecords = @HasPaidRecords,
					UseSubGen = @UseSubGen
			WHERE PubID = @PubID
			SELECT @PubID;
		END
	ELSE
		BEGIN
			if len(@PubCode) > 0 and not exists(select pubid from Pubs with(nolock) where PubCode = @PubCode)
				begin
					INSERT INTO Pubs (PubName, istradeshow, PubCode, PubTypeID, GroupID, EnableSearching, score, SortOrder, DateCreated, CreatedByUserID, ClientID, YearStartDate, YearEndDate, IssueDate, IsImported, IsActive, AllowDataEntry, FrequencyID, KMImportAllowed, ClientImportAllowed, AddRemoveAllowed, AcsMailerInfoId, IsUAD, IsCirc, IsOpenCloseLocked, HasPaidRecords, UseSubGen)
					VALUES (@PubName, @istradeshow, @PubCode, @PubTypeID, @GroupID, @EnableSearching, @score, @SortOrder, @DateCreated, @CreatedByUserID, @ClientID, @YearStartDate, @YearEndDate, @IssueDate, @IsImported, @IsActive, @AllowDataEntry, @FrequencyID, @KMImportAllowed, @ClientImportAllowed, @AddRemoveAllowed, @AcsMailerInfoId, @IsUAD, @IsCirc,@IsOpenCloseLocked, @HasPaidRecords, @UseSubGen)
					SELECT @@IDENTITY
				end
			else
				begin
					select 0
				end
		END
		
END	