CREATE PROCEDURE e_PersonalizedContent_Select_BlastID_EmailAddress
	-- Add the parameters for the stored procedure here
	@BlastID Bigint, 
	@EmailAddress varchar(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * from PersonalizedContent with (NOLOCK)
	where blastID = @BlastID and EmailAddress = @EmailAddress
END