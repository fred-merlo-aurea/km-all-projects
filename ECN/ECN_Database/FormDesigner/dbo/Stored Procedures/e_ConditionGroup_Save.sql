CREATE PROCEDURE [dbo].[e_ConditionGroup_Save]
	@ConditionGroup_Seq_ID int,
	@MainGroup_ID int = null,
	@LogicGroup bit
AS
	if @ConditionGroup_Seq_ID < 0
	BEGIN
		Insert into ConditionGroup(MainGroup_ID, LogicGroup)
		VALUES(@MainGroup_ID, @LogicGroup)
		select @@IDENTITY;
	END
	ELSE
	BEGIN
		Update ConditionGroup
		set MainGroup_ID = @MainGroup_ID,
			LogicGroup = @LogicGroup
		where ConditionGroup_Seq_ID = @ConditionGroup_Seq_ID
		select @ConditionGroup_Seq_ID
	END