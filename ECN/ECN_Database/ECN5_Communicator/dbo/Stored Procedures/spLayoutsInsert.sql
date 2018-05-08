CREATE PROCEDURE dbo.spLayoutsInsert
@TemplateID int,
@CustomerID int,
@FolderID int,
@LayoutName varchar(50),
@ContentSlot1 int,
@ContentSlot2 int,
@ContentSlot3 int,
@ContentSlot4 int,
@ContentSlot5 int,
@ContentSlot6 int,
@ContentSlot7 int,
@ContentSlot8 int,
@ContentSlot9 int,
@UserID int,
@ModifyDate datetime,
@TableOptions varchar(255),
@DisplayAddress varchar(255),
@SetupCost varchar(50),
@OutboundCost varchar(50),
@InboundCost varchar(50),
@DesignCost varchar(50),
@OtherCost varchar(50)
AS
INSERT INTO [LAYOUT]
           ([TemplateID]
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
           ,[OtherCost])
     VALUES
           (@TemplateID,@CustomerID,@FolderID,@LayoutName,@ContentSlot1,@ContentSlot2,@ContentSlot3,@ContentSlot4,@ContentSlot5,@ContentSlot6,@ContentSlot7,
			@ContentSlot8,@ContentSlot9,@UserID,@ModifyDate,@TableOptions,@DisplayAddress,@SetupCost,@OutboundCost,@InboundCost,@DesignCost,@OtherCost);SELECT @@IDENTITY;