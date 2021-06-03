Imports System.Data.SqlClient
Imports Infragistics.Win.UltraWinGrid
Imports DBLotVbnet.common
Imports System.Net
Imports DBLotVbnet.DBEngin
Imports DBLotVbnet.DAL_InterLocation
Imports DBLotVbnet.DAL_Distributors
Imports System.Configuration
Public Class frmView_Items_cnt
    Function Load_Grid_PRODUCT()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "select  ROW_NUMBER() OVER (ORDER BY M11ID ) as  ##,M11Part_No as [Item Code],M11Part_Name as [Item Name]  from M11Product_Item where M11Type='PRODUCT ITEM' and M11Status='A' order by M11ID"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid2.DataSource = M01
            UltraGrid2.Rows.Band.Columns(0).Width = 30
            UltraGrid2.Rows.Band.Columns(1).Width = 90
            UltraGrid2.Rows.Band.Columns(2).Width = 280
            ' UltraGrid1.Rows.Band.Columns(0).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center

            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function


    Function sEARCH_Grid_PRODUCT()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "select  ROW_NUMBER() OVER (ORDER BY M11ID ) as  ##,M11Part_No as [Item Code],M11Part_Name as [Item Name]  from M11Product_Item where M11Type='PRODUCT ITEM' and M11Status='A' AND M11Part_Name LIKE '%" & Trim(txtSearch.Text) & "%' order by M11ID"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid2.DataSource = M01
            UltraGrid2.Rows.Band.Columns(0).Width = 30
            UltraGrid2.Rows.Band.Columns(1).Width = 90
            UltraGrid2.Rows.Band.Columns(2).Width = 280
            ' UltraGrid1.Rows.Band.Columns(0).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center

            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Function SEARCH_Grid_ROW()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "select  ROW_NUMBER() OVER (ORDER BY M11ID ) as  ##,M11Part_No as [Item Code],M11Part_Name as [Item Name]  from M11Product_Item where M11Type='ROW ITEMS' and M11Status='A' AND M11Part_Name LIKE '%" & Trim(txtSearch.Text) & "%' order by M11ID"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid2.DataSource = M01
            UltraGrid2.Rows.Band.Columns(0).Width = 30

            UltraGrid2.Rows.Band.Columns(1).Width = 90
            UltraGrid2.Rows.Band.Columns(2).Width = 280
            ' UltraGrid1.Rows.Band.Columns(0).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center

            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Function Load_Grid_ROW()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "select  ROW_NUMBER() OVER (ORDER BY M11ID ) as  ##,M11Part_No as [Item Code],M11Part_Name as [Item Name]  from M11Product_Item where M11Type='ROW ITEMS' and M11Status='A' order by M11ID"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid2.DataSource = M01
            UltraGrid2.Rows.Band.Columns(0).Width = 30

            UltraGrid2.Rows.Band.Columns(1).Width = 90
            UltraGrid2.Rows.Band.Columns(2).Width = 280
            ' UltraGrid1.Rows.Band.Columns(0).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center

            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Private Sub UltraGrid2_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles UltraGrid2.DoubleClickRow
        On Error Resume Next
        Dim _Row As Integer
        _Row = UltraGrid2.ActiveRow.Index
        If strWindowName = "frmBom_Creation" Then
            If strWinStatus = "PRODUCT" Then
                frmBom_Creation.cboCode.Text = UltraGrid2.Rows(_Row).Cells(1).Text
                frmBom_Creation.Search_Itemcode()
                Me.Close()
            ElseIf strWinStatus = "ROW" Then
                frmBom_Creation.cboRow_Code.Text = UltraGrid2.Rows(_Row).Cells(1).Text
                frmBom_Creation.Search_Rowcode()
                Me.Close()
            End If
        ElseIf strWindowName = "frmStockBalance_Product" Then
            frmStockBalance_Product.cboCode.Text = UltraGrid2.Rows(_Row).Cells(1).Text
            frmStockBalance_Product.Search_Itemcode()
            Me.Close()
        End If
    End Sub

    Private Sub txtSearch_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearch.ValueChanged
        If strWinStatus = "PRODUCT" Then
            Call sEARCH_Grid_PRODUCT()
        ElseIf strWinStatus = "ROW" Then
            Call SEARCH_Grid_ROW()
        End If
    End Sub
End Class