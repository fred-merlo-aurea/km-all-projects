CREATE PROCEDURE [dbo].[e_OverwritedataPostValue_Save]
	@Control_ID int,
	@Rule_Seq_ID int,
	@Value varchar(30)
AS
	INSERT into OverwritedataPostValue(Control_ID, Rule_Seq_ID, [Value])
	VALUES(@Control_ID, @Rule_Seq_ID, @Value)
	select @@IDENTITY;