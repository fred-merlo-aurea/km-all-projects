CREATE PROCEDURE [dbo].[spTemplateSelectByTemplateID]
@TemplateID int
AS
SELECT [TemplateID]
      ,[baseChannelID]
      ,[TemplateStyleCode]
      ,[TemplateName]
      ,[TemplateImage]
      ,[TemplateDescription]
      ,[SortOrder]
      ,[SlotsTotal]
      ,IsActive
      ,[TemplateSource]
      ,[TemplateText]
      ,[TemplateSubject]
  FROM [TEMPLATE]
  WHERE TemplateID = @TemplateID
