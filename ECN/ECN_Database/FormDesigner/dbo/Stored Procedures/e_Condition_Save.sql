CREATE PROCEDURE [dbo].[e_Condition_Save]
	@Control_ID int,
	@ConditionGroup_Seq_ID int,
	@Operation_ID int,
	@Value varchar(50)
AS
	Insert into Condition(Control_ID, ConditionGroup_Seq_ID, Operation_ID, [Value])
	VALUES(@Control_ID, @ConditionGroup_Seq_ID, @Operation_ID, @Value)
	select @@IDENTITY;
