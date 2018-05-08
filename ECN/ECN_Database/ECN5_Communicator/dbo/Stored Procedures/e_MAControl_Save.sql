CREATE PROCEDURE [dbo].[e_MAControl_Save]
	@ControlID varchar(255),
	@ECNID int,
	@Text varchar(MAX),
	@ExtraText varchar(MAX),
	@MarketingAutomationID int,
	@xPosition decimal(15,2),
	@yPosition decimal(15,2),
	@ControlType varchar(255),
	@MAControlID int
AS
	if @MAControlID > 0--update
	BEGIN
		UPDATE MAControl
		set ECNID = @ECNID,
			Text = @Text,
			ExtraText = @ExtraText,
			MarketingAutomationID = @MarketingAutomationID,
			xPosition = @xPosition,
			yPosition = @yPosition,
			ControlType = @ControlType,
			ControlID = @ControlID
		WHERE MAControlID = @MAControlID
		SELECT @MAControlID
	END
	ELSE if @MAControlID <= 0--insert
	BEGIN
		INSERT INTO MAControl(ControlID,ECNID, Text, ExtraText, MarketingAutomationID, xPosition,yPosition,ControlType)
		VALUES(@ControlID,@ECNID, @Text, @ExtraText, @MarketingAutomationID, @xPosition, @yPosition, @ControlType)
		SELECT @@IDENTITY;
	END
