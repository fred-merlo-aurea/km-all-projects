CREATE TABLE [dbo].[Mastercodesheet] (
    [MasterID]        INT           IDENTITY (1, 1) NOT NULL,
    [MasterValue]     VARCHAR (100) NULL,
    [MasterDesc]      VARCHAR (255) NULL,
    [MasterGroupID]   INT           NULL,
    [MasterDesc1]     VARCHAR (255) NULL,
    [EnableSearching] BIT           NULL,
    [SortOrder]       INT           NULL,
    [DateCreated]     DATETIME      CONSTRAINT [DF_Mastercodesheet_DateCreated] DEFAULT (getdate()) NULL,
    [DateUpdated]     DATETIME      NULL,
    [CreatedByUserID] INT           NULL,
    [UpdatedByUserID] INT           NULL,
    CONSTRAINT [PK_Mastercodesheet] PRIMARY KEY CLUSTERED ([MasterID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_Mastercodesheet_MasterGroups] FOREIGN KEY ([MasterGroupID]) REFERENCES [dbo].[MasterGroups] ([MasterGroupID])
);


GO
CREATE NONCLUSTERED INDEX [IDX_mastercodesheet_MasterGroupID]
    ON [dbo].[Mastercodesheet]([MasterGroupID] ASC)
    INCLUDE([MasterID], [MasterValue], [MasterDesc]) WITH (FILLFACTOR = 90);
GO
CREATE NONCLUSTERED INDEX [IDX_MasterCodeSheet_MasterGroupID_MasterDesc_MasterID]
    ON [dbo].[Mastercodesheet]([MasterGroupID] ASC, [MasterDesc] ASC, [MasterID] ASC)
    INCLUDE([MasterValue], [MasterDesc1]) WITH (FILLFACTOR = 90);
GO
CREATE STATISTICS [STAT_Mastercodesheet_MasterID_MasterGroupID_MasterDesc]
    ON [dbo].[Mastercodesheet]([MasterID], [MasterGroupID], [MasterDesc]);
GO
CREATE TRIGGER [dbo].[TR_MasterCodeSheet_Insert]
   ON [dbo].[Mastercodesheet] 
   AFTER INSERT 
AS  
BEGIN 
   SET NOCOUNT ON;  
   
   create table #ins (id int identity(1,1), mastergroupID int, sortorder int)
   
   insert into #ins (mastergroupID, sortorder)
   Select i.MasterGroupID, isnull(MAX(mc.sortOrder),0) 
   From 
		inserted i left outer join 
		mastercodesheet mc on i.mastergroupID = mc.mastergroupID
	where 
		isnull(i.SortOrder, 0) = 0
   group by 
		i.mastergroupID
   
   UPDATE mc
   SET SortOrder = i1.SortOrder + ID
   FROM 
		Mastercodesheet mc join 
		INSERTED I on mc.masterID = i.masterID join 
		#ins i1 on i.MasterGroupID = i1.MasterGroupID
   
   drop table #ins
END