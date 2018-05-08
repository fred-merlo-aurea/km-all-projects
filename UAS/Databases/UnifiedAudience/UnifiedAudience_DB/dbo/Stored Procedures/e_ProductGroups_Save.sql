CREATE PROCEDURE [dbo].[e_ProductGroups_Save]
	@PubID int,
	@GroupID int,
	@Name varchar(100) = ''
AS
BEGIN

	set nocount on

	INSERT INTO PubGroups (PubID, GroupID, Name)
	VALUES (@PubID, @GroupID, @Name)

END