CREATE PROCEDURE [dbo].[spInsertEmailActivityLog_SeedClick]
	@EmailID int,
	@BlastID int,
	@ActionTypeCode varchar(50),
	@ActionValue varchar(500),
	@ActionNotes varchar(255),
	@Processed char(10)
AS
	SET NOCOUNT ON

INSERT INTO EmailActivityLog(
	EmailID, 
	BlastID, 
	ActionTypeCode, 
	ActionDate, 
	ActionValue, 
	ActionNotes, 
	Processed)
VALUES(
	@EmailID,
	@BlastID ,
	@ActionTypeCode, 
	GetDate(),
	@ActionValue, 
	@ActionNotes, 
	@Processed ) 

SELECT @@IDENTITY;
GO