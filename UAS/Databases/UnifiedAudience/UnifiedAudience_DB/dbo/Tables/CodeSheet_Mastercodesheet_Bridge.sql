CREATE TABLE [dbo].[CodeSheet_Mastercodesheet_Bridge] (
    [CodeSheetID] INT NOT NULL,
    [MasterID]    INT NOT NULL,
    CONSTRAINT [PK_CodeSheet_Mastercodesheet_Bridge] PRIMARY KEY CLUSTERED ([CodeSheetID] ASC, [MasterID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_CodeSheet_Mastercodesheet_Bridge_CodeSheet] FOREIGN KEY ([CodeSheetID]) REFERENCES [dbo].[CodeSheet] ([CodeSheetID]),
    CONSTRAINT [FK_CodeSheet_Mastercodesheet_Bridge_Mastercodesheet] FOREIGN KEY ([MasterID]) REFERENCES [dbo].[Mastercodesheet] ([MasterID])
);
GO
CREATE NONCLUSTERED INDEX [IX_CodeSheet_Mastercodesheet_Bridge_MasterID]
    ON [dbo].[CodeSheet_Mastercodesheet_Bridge]([MasterID] ASC) WITH (FILLFACTOR = 90);
GO
