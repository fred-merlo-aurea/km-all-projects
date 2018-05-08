CREATE VIEW dbo.vw_ServiceFeatureAccess
AS
SELECT     TOP (100) PERCENT dbo.Service.ServiceID, dbo.Service.ServiceName, dbo.Service.ServiceCode, dbo.Service.IsEnabled AS S_Enabled, dbo.Service.DisplayOrder, 
                      dbo.ServiceFeature.ServiceFeatureID, dbo.ServiceFeature.SFName, dbo.ServiceFeature.SFCode, dbo.ServiceFeature.DisplayOrder AS sf_DisplayOrder, 
                      dbo.ServiceFeature.IsAdditionalCost, dbo.ServiceFeatureAccessMap.ServiceFeatureAccessMapID, dbo.Access.AccessName, dbo.Access.AccessCode, 
                      dbo.Access.IsActive AS A_IsActive
FROM         dbo.Service INNER JOIN
                      dbo.ServiceFeature ON dbo.Service.ServiceID = dbo.ServiceFeature.ServiceID INNER JOIN
                      dbo.ServiceFeatureAccessMap ON dbo.ServiceFeature.ServiceFeatureID = dbo.ServiceFeatureAccessMap.ServiceFeatureID INNER JOIN
                      dbo.Access ON dbo.ServiceFeatureAccessMap.AccessID = dbo.Access.AccessID
WHERE     (dbo.Service.IsEnabled = 1) AND (dbo.ServiceFeature.IsEnabled = 1)
ORDER BY dbo.Service.DisplayOrder, dbo.ServiceFeature.DisplayOrder, dbo.Access.AccessCode
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_ServiceFeatureAccess';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'pe = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_ServiceFeatureAccess';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[25] 4[43] 2[12] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = -96
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Service"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 236
               Right = 248
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ServiceFeature"
            Begin Extent = 
               Top = 6
               Left = 286
               Bottom = 240
               Right = 496
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ServiceFeatureAccessMap"
            Begin Extent = 
               Top = 6
               Left = 534
               Bottom = 235
               Right = 760
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Access"
            Begin Extent = 
               Top = 6
               Left = 798
               Bottom = 237
               Right = 973
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 15
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 2505
         Alias = 900
         Table = 1905
         Output = 720
         Append = 1400
         NewValue = 1170
         SortTy', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_ServiceFeatureAccess';

