CREATE TABLE [dbo].[MasterGroups] (
    [MasterGroupID]      INT           IDENTITY (1, 1) NOT NULL,
    [Name]               VARCHAR (100) NULL,
    [Description]        VARCHAR (100) NULL,
    [DisplayName]        VARCHAR (50)  NULL,
    [ColumnReference]    VARCHAR (110) NULL,
    [SortOrder]          INT           NULL,
    [IsActive]           BIT           NULL,
    [EnableSubReporting] BIT           NULL,
    [EnableSearching]    BIT           NULL,
    [EnableAdhocSearch]  BIT           NULL,
    [DateCreated]        DATETIME      CONSTRAINT [DF_MasterGroups_DateCreated] DEFAULT (getdate()) NULL,
    [DateUpdated]        DATETIME      NULL,
    [CreatedByUserID]    INT           NULL,
    [UpdatedByUserID]    INT           NULL,
    CONSTRAINT [PK_MasterGroups] PRIMARY KEY CLUSTERED ([MasterGroupID] ASC) WITH (FILLFACTOR = 90)
);
GO

CREATE TRIGGER [dbo].[TR_MasterGroups_Insert]
   ON [dbo].[MasterGroups] 
   AFTER INSERT 
AS  
BEGIN 
   SET NOCOUNT ON;  
   
   declare @sortorder int
   
   create table #ins (id int identity(1,1), mastergroupID int)
   
   select @SortOrder = isnull(MAX(sortOrder),0) from MasterGroups

   insert into #ins (mastergroupID)
   Select 
		i.MasterGroupID From inserted i
   	where 
		isnull(i.SortOrder, 0) = 0
   order by i.Name
   
   UPDATE mg  
   SET SortOrder = @SortOrder + ID
   FROM MasterGroups mg join #ins i1 on mg.MasterGroupID = i1.MasterGroupID
   
   drop table #ins
END
GO