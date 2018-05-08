CREATE proc [dbo].[e_Config_Save](
@ConfigID int,
@Name varchar(50),
@Value varchar(100)
)
as
BEGIN

	SET NOCOUNT ON

	if(@ConfigID > 0)
		begin
			update Config set Name = @Name, Value = @Value where ConfigID=@ConfigID
		end
	else
		begin
			insert into Config (Name, Value) values (@Name, @Value)
		end	
End