CREATE PROCEDURE [dbo].[spTemplateContentSelectByTemplateID]
@TemplateID int
AS
SELECT [TemplateContentID]
      ,[TemplateID]
      ,[ContentID]
      ,[SlotNumber]
      ,[TemplateSubject]
  FROM [TemplateContents]
  WHERE TemplateID = @TemplateID
