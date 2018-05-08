CREATE PROCEDURE [dbo].[e_RequestQueryValue_Save]
	@Control_ID int,
	@Rule_Seq_ID int,
	@Name varchar(30)
AS
	insert into RequestQueryValue(Control_ID, Rule_Seq_ID, Name)
	VALUES(@Control_ID, @Rule_Seq_ID, @Name)
	select @@IDENTITY;
