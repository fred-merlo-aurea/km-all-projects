CREATE  PROC [dbo].[e_Layout_Save] 
(
	@LayoutID int = NULL,
    @TemplateID int = NULL,
    @CustomerID int = NULL,
    @FolderID int = NULL,
    @LayoutName varchar(50) = NULL,
    @ContentSlot1 int = NULL,
    @ContentSlot2 int = NULL,
    @ContentSlot3 int = NULL,
    @ContentSlot4 int = NULL,
    @ContentSlot5 int = NULL,
    @ContentSlot6 int = NULL,
    @ContentSlot7 int = NULL,
    @ContentSlot8 int = NULL,
    @ContentSlot9 int = NULL,
    @UserID int = NULL,
    @TableOptions varchar(255) = NULL,
    @DisplayAddress varchar(255) = NULL,
    @SetupCost varchar(50) = NULL,
    @OutboundCost varchar(50) = NULL,
    @InboundCost varchar(50) = NULL,
    @DesignCost varchar(50) = NULL,
    @OtherCost varchar(50) = NULL,
    @MessageTypeID int = NULL,
	@Archived bit = null
)
AS 
BEGIN
	IF @LayoutID is NULL or @LayoutID <= 0
	BEGIN
		INSERT INTO Layout
		(
			TemplateID,CustomerID,FolderID,LayoutName,ContentSlot1,ContentSlot2,ContentSlot3,ContentSlot4,ContentSlot5,ContentSlot6,ContentSlot7,ContentSlot8,ContentSlot9,
            TableOptions,DisplayAddress,SetupCost,OutboundCost,InboundCost,DesignCost,OtherCost,MessageTypeID, UpdatedUserID,UpdatedDate, CreatedUserID,CreatedDate,IsDeleted,Archived
		)
		VALUES
		(
			@TemplateID,@CustomerID,@FolderID,@LayoutName,@ContentSlot1,@ContentSlot2,@ContentSlot3,@ContentSlot4,@ContentSlot5,@ContentSlot6,@ContentSlot7,@ContentSlot8,@ContentSlot9,
            @TableOptions,@DisplayAddress,@SetupCost,@OutboundCost,@InboundCost,@DesignCost,@OtherCost,@MessageTypeID, @UserID,GetDate(), @UserID,GetDate(),0,@Archived
		)
		SET @LayoutID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE Layout
			SET TemplateID=@TemplateID,CustomerID=@CustomerID,FolderID=@FolderID,LayoutName=@LayoutName,ContentSlot1=@ContentSlot1,ContentSlot2=@ContentSlot2,
				ContentSlot3=@ContentSlot3,ContentSlot4=@ContentSlot4,ContentSlot5=@ContentSlot5,ContentSlot6=@ContentSlot6,ContentSlot7=@ContentSlot7,ContentSlot8=@ContentSlot8,
				ContentSlot9=@ContentSlot9,TableOptions=@TableOptions,DisplayAddress=@DisplayAddress,SetupCost=@SetupCost,OutboundCost=@OutboundCost,
				InboundCost=@InboundCost,DesignCost=@DesignCost,OtherCost=@OtherCost,MessageTypeID=@MessageTypeID,UpdatedUserID=@UserID,UpdatedDate=GETDATE(),Archived = @Archived
		WHERE
			LayoutID = @LayoutID
	END

	SELECT @LayoutID
END