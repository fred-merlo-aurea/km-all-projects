CREATE TABLE [dbo].[SMTPMessage_LogFileData]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [EmailID] INT NOT NULL, 
    [BlastID] INT NOT NULL, 
    [SMTPMessage] VARCHAR(255) NOT NULL, 
    [SourceIP] VARCHAR(50) NOT NULL, 
    [SendID] BIGINT NULL
)

GO

CREATE INDEX [IX_SMTPMessage_LogFileData_BlastID_EmailID] ON [dbo].[SMTPMessage_LogFileData] ([BlastID], [EmailID])
