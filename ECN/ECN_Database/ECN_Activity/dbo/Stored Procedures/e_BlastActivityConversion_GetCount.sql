CREATE PROCEDURE [dbo].[e_BlastActivityConversion_GetCount] 
(
	@BlastID int,
	@CustomerID int,
	@URL varchar(255),
	@Length int,
	@Distinct bit
)
AS
BEGIN
	IF @Distinct = 1
	BEGIN
		SELECT 
			COUNT(DISTINCT(bac.EmailID)) 
		FROM 
			BlastActivityConversion bac WITH (NOLOCK) 
			JOIN ecn5_communicator..[Blast] b on bac.BlastID = b.BlastID AND b.CustomerID = @CustomerID
		WHERE 
			bac.BlastID = @BlastID AND 
			SUBSTRING(URL, 0,  @Length ) = @URL
	END
	ELSE
	BEGIN
		SELECT 
			COUNT(*) 
		FROM 
			BlastActivityConversion bac WITH (NOLOCK) 
			JOIN ecn5_communicator..[Blast] b on bac.BlastID = b.BlastID AND b.CustomerID = @CustomerID
		WHERE 
			bac.BlastID = @BlastID AND 
			SUBSTRING(URL, 0,  @Length ) = @URL
	END
END