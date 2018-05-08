CREATE PROCEDURE [dbo].[spEmailDataVaulesUpdateDataValueByGroupDatafieldsID]
@GroupDatafieldsID int,
@DataValue varchar(500)
AS
UPDATE [EmailDataValues]
   SET [DataValue] = @DataValue
WHERE [GroupDatafieldsID] = @GroupDatafieldsID
