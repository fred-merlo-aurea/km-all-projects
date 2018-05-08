CREATE PROCEDURE e_PersonalizedContent_Select_PersonalizedContentIDs
	-- Add the parameters for the stored procedure here
	@PersonalizedContentIDs varchar(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @hDoc AS INT

	declare @IDs table (ID bigint Primary Key)

	EXEC sp_xml_preparedocument @hDoc OUTPUT, @PersonalizedContentIDs

		insert into @IDs
		SELECT pcID
		FROM OPENXML(@hDoc, 'xml/id')
		WITH 
		(
		pcID bigint '.'
		)
	
		EXEC sp_xml_removedocument @hDoc

	SELECT pc.* from PersonalizedContent pc with (NOLOCK)  join @IDs t on pc.PersonalizedContentID = t.ID

END