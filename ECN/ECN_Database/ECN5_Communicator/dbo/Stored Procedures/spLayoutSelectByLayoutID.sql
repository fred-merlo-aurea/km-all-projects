﻿CREATE PROCEDURE [dbo].[spLayoutSelectByLayoutID]
@LayoutID int
AS
SELECT [LayoutID]
      ,[TemplateID]
      ,[CustomerID]
      ,[FolderID]
      ,[LayoutName]
      ,[ContentSlot1]
      ,[ContentSlot2]
      ,[ContentSlot3]
      ,[ContentSlot4]
      ,[ContentSlot5]
      ,[ContentSlot6]
      ,[ContentSlot7]
      ,[ContentSlot8]
      ,[ContentSlot9]
      ,[CreatedUserID]
      ,[UpdatedDate]
      ,[TableOptions]
      ,[DisplayAddress]
      ,[SetupCost]
      ,[OutboundCost]
      ,[InboundCost]
      ,[DesignCost]
      ,[OtherCost]
  FROM [LAYOUT]
  WHERE [LayoutID] = @LayoutID