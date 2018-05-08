CREATE PROCEDURE [dbo].[e_ClientCustomProcedure_Save]
@ClientCustomProcedureID int,
@ClientID int,
@IsActive bit,
@ProcedureType varchar(100),
@ProcedureName varchar(50),
@ExecutionOrder int,
@ServiceID int,
@ServiceFeatureID int,
@ExecutionPointID int, 
@IsForSpecialFile bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
BEGIN

	set nocount on

	IF @ClientCustomProcedureID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE ClientCustomProcedure
			SET ClientID = @ClientID,
				IsActive = @IsActive,
				ProcedureName = @ProcedureName,			
				ExecutionOrder = @ExecutionOrder,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID,
				ProcedureType = @ProcedureType,
				ServiceID = @ServiceID,
				ServiceFeatureID = @ServiceFeatureID,
				ExecutionPointID = @ExecutionPointID,			
				IsForSpecialFile = @IsForSpecialFile
			WHERE ClientCustomProcedureID = @ClientCustomProcedureID;
		
			SELECT @ClientCustomProcedureID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END		
			--INSERT INTO ClientCustomProcedure (ClientID,IsActive,ProcedureName,ExecutionOrder,DateCreated,CreatedByUserID)
			--VALUES(@ClientID,@IsActive,@ProcedureName,@ExecutionOrder,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
			INSERT INTO ClientCustomProcedure (ClientID,IsActive,ProcedureType,ProcedureName,ExecutionOrder,ServiceID
												,ServiceFeatureID,ExecutionPointID,IsForSpecialFile,DateCreated,CreatedByUserID)
			VALUES(@ClientID,@IsActive,@ProcedureType,@ProcedureName,@ExecutionOrder,@ServiceID,@ServiceFeatureID
					,@ExecutionPointID,@IsForSpecialFile,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END

END