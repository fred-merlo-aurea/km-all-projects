CREATE  PROC [dbo].[e_BlastFields_Save] 
(
	@BlastID int,
	@Field1 varchar(255) = NULL,
	@Field2 varchar(255) = NULL,
	@Field3 varchar(255) = NULL,
	@Field4 varchar(255) = NULL,
	@Field5 varchar(255) = NULL,
    @UpdatedUserID int = NULL,
    @CreatedUserID int = NULL
)
AS 
BEGIN
	DECLARE @exist int
	select @exist = IsNull(COUNT(BlastID),0) FROM BlastFields with (nolock) WHERE BlastID = @BlastID	

	IF @exist <= 0
	BEGIN
		INSERT INTO BlastFields
		(
			BlastID,Field1,Field2,Field3,Field4,Field5,CreatedDate,IsDeleted,CreatedUserID
		)
		VALUES
		(
			@BlastID,@Field1,@Field2,@Field3,@Field4,@Field5,GETDATE(),0,@CreatedUserID
		)
	END
	ELSE
	BEGIN
		UPDATE BlastFields
			SET Field1=@Field1,Field2=@Field2,Field3=@Field3,Field4=@Field4,Field5=@Field5,UpdatedUserID=@UpdatedUserID,UpdatedDate=GETDATE()
		WHERE
			BlastID = @BlastID
	END
END