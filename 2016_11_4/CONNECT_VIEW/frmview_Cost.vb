Imports System.Data.SqlClient
Imports Infragistics.Win.UltraWinGrid
Imports DBLotVbnet.common
Imports System.Net
Imports DBLotVbnet.DBEngin
Imports DBLotVbnet.DAL_InterLocation
Imports DBLotVbnet.DAL_Distributors
Imports System.Configuration
Public Class frmview_Cost
    Dim _Print As String

    Private Sub ExitToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem1.Click
        Me.Close()
    End Sub

    Function Load_Grid()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "select  ROW_NUMBER() OVER (ORDER BY T01ID  ) as  ##,T01GRN_No as [GRN No],M24Ref_No as [#Ref No],M24Date as [Date],T01Com_Invoice as [Com.Inv.No],M07Sup_Name as [Supplier Name],CAST(M24USD_Rate AS DECIMAL(16,2)) as [Total Amount USD],CAST(M24Unit_Cost AS DECIMAL(16,2)) as [Unit Cost],CAST(M24Tax AS DECIMAL(16,2)) as [Tax],CAST(M24Cus_Deuty AS DECIMAL(16,2)) as [Custom Duty],CAST(M24Shp_Chargers AS DECIMAL(16,2)) as [Shipping Charges],CAST(M24Comm AS DECIMAL(16,2)) as [Commission],CAST(M24Transport AS DECIMAL(16,2)) as [Transport],CAST(M24Bank AS DECIMAL(16,2)) as [Bank Chargers],CAST(M24VAT AS DECIMAL(16,2)) as [VAT],CAST(M24NBT AS DECIMAL(16,2)) as [NBT]   from T01GRN_Header inner join  M07Supplier on M07Sup_Code=T01Supp_Code inner join M24GRN_Cost_Analysis_Header on T01GRN_No=M24GRN where M24Status='A' order by T01ID DESC"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid1.DataSource = M01
            UltraGrid1.Rows.Band.Columns(0).Width = 30
            UltraGrid1.Rows.Band.Columns(1).Width = 80
            UltraGrid1.Rows.Band.Columns(2).Width = 80
            UltraGrid1.Rows.Band.Columns(3).Width = 80
            UltraGrid1.Rows.Band.Columns(4).Width = 80
            UltraGrid1.Rows.Band.Columns(5).Width = 180
            UltraGrid1.Rows.Band.Columns(6).Width = 80
            UltraGrid1.Rows.Band.Columns(7).Width = 80
            UltraGrid1.Rows.Band.Columns(8).Width = 100
            UltraGrid1.Rows.Band.Columns(9).Width = 100
            UltraGrid1.Rows.Band.Columns(10).Width = 100
            UltraGrid1.Rows.Band.Columns(11).Width = 100
            UltraGrid1.Rows.Band.Columns(12).Width = 100
            UltraGrid1.Rows.Band.Columns(13).Width = 100
            UltraGrid1.Rows.Band.Columns(14).Width = 100
            UltraGrid1.Rows.Band.Columns(15).Width = 100
            ' UltraGrid1.Rows.Band.Columns(16).Width = 110
            UltraGrid1.Rows.Band.Columns(0).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(1).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(2).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(3).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(6).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(7).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(8).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(9).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(10).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(11).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(12).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(13).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(14).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(15).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            'UltraGrid1.Rows.Band.Columns(16).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function


    Function Load_Grid_find()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "select  ROW_NUMBER() OVER (ORDER BY T01ID  ) as  ##,T01GRN_No as [GRN No],M24Ref_No as [#Ref No],M24Date as [Date],T01Com_Invoice as [Com.Inv.No],M07Sup_Name as [Supplier Name],CAST(M24USD_Rate AS DECIMAL(16,2)) as [Total Amount USD],CAST(M24Unit_Cost AS DECIMAL(16,2)) as [Unit Cost],CAST(M24Tax AS DECIMAL(16,2)) as [Tax],CAST(M24Cus_Deuty AS DECIMAL(16,2)) as [Custom Duty],CAST(M24Shp_Chargers AS DECIMAL(16,2)) as [Shipping Charges],CAST(M24Comm AS DECIMAL(16,2)) as [Commission],CAST(M24Transport AS DECIMAL(16,2)) as [Transport],CAST(M24Bank AS DECIMAL(16,2)) as [Bank Chargers],CAST(M24VAT AS DECIMAL(16,2)) as [VAT],CAST(M24NBT AS DECIMAL(16,2)) as [NBT]   from T01GRN_Header inner join  M07Supplier on M07Sup_Code=T01Supp_Code inner join M24GRN_Cost_Analysis_Header on T01GRN_No=M24GRN where M24Status='A' and T01Com_Invoice like '%" & txtSearch.Text & "%' order by T01ID DESC"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid1.DataSource = M01
            UltraGrid1.Rows.Band.Columns(0).Width = 30
            UltraGrid1.Rows.Band.Columns(1).Width = 80
            UltraGrid1.Rows.Band.Columns(2).Width = 80
            UltraGrid1.Rows.Band.Columns(3).Width = 80
            UltraGrid1.Rows.Band.Columns(4).Width = 80
            UltraGrid1.Rows.Band.Columns(5).Width = 180
            UltraGrid1.Rows.Band.Columns(6).Width = 80
            UltraGrid1.Rows.Band.Columns(7).Width = 80
            UltraGrid1.Rows.Band.Columns(8).Width = 100
            UltraGrid1.Rows.Band.Columns(9).Width = 100
            UltraGrid1.Rows.Band.Columns(10).Width = 100
            UltraGrid1.Rows.Band.Columns(11).Width = 100
            UltraGrid1.Rows.Band.Columns(12).Width = 100
            UltraGrid1.Rows.Band.Columns(13).Width = 100
            UltraGrid1.Rows.Band.Columns(14).Width = 100
            UltraGrid1.Rows.Band.Columns(15).Width = 100
            ' UltraGrid1.Rows.Band.Columns(16).Width = 110
            UltraGrid1.Rows.Band.Columns(0).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(1).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(2).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(3).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(6).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(7).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(8).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(9).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(10).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(11).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(12).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(13).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(14).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(15).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            'UltraGrid1.Rows.Band.Columns(16).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Function Load_Grid_Date()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "select  ROW_NUMBER() OVER (ORDER BY T01ID  ) as  ##,T01GRN_No as [GRN No],M24Ref_No as [#Ref No],M24Date as [Date],T01Com_Invoice as [Com.Inv.No],M07Sup_Name as [Supplier Name],CAST(M24USD_Rate AS DECIMAL(16,2)) as [Total Amount USD],CAST(M24Unit_Cost AS DECIMAL(16,2)) as [Unit Cost],CAST(M24Tax AS DECIMAL(16,2)) as [Tax],CAST(M24Cus_Deuty AS DECIMAL(16,2)) as [Custom Duty],CAST(M24Shp_Chargers AS DECIMAL(16,2)) as [Shipping Charges],CAST(M24Comm AS DECIMAL(16,2)) as [Commission],CAST(M24Transport AS DECIMAL(16,2)) as [Transport],CAST(M24Bank AS DECIMAL(16,2)) as [Bank Chargers],CAST(M24VAT AS DECIMAL(16,2)) as [VAT],CAST(M24NBT AS DECIMAL(16,2)) as [NBT]   from T01GRN_Header inner join  M07Supplier on M07Sup_Code=T01Supp_Code inner join M24GRN_Cost_Analysis_Header on T01GRN_No=M24GRN where M24Status='A' and M24Date between '" & txtDate5.Text & "' and '" & txtDate6.Text & "' order by T01ID DESC"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid1.DataSource = M01
            UltraGrid1.Rows.Band.Columns(0).Width = 30
            UltraGrid1.Rows.Band.Columns(1).Width = 80
            UltraGrid1.Rows.Band.Columns(2).Width = 80
            UltraGrid1.Rows.Band.Columns(3).Width = 80
            UltraGrid1.Rows.Band.Columns(4).Width = 80
            UltraGrid1.Rows.Band.Columns(5).Width = 180
            UltraGrid1.Rows.Band.Columns(6).Width = 80
            UltraGrid1.Rows.Band.Columns(7).Width = 80
            UltraGrid1.Rows.Band.Columns(8).Width = 100
            UltraGrid1.Rows.Band.Columns(9).Width = 100
            UltraGrid1.Rows.Band.Columns(10).Width = 100
            UltraGrid1.Rows.Band.Columns(11).Width = 100
            UltraGrid1.Rows.Band.Columns(12).Width = 100
            UltraGrid1.Rows.Band.Columns(13).Width = 100
            UltraGrid1.Rows.Band.Columns(14).Width = 100
            UltraGrid1.Rows.Band.Columns(15).Width = 100
            ' UltraGrid1.Rows.Band.Columns(16).Width = 110
            UltraGrid1.Rows.Band.Columns(0).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(1).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(2).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(3).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(6).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(7).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(8).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(9).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(10).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(11).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(12).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(13).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(14).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(15).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            'UltraGrid1.Rows.Band.Columns(16).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Function Load_Grid_Supplier()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "select  ROW_NUMBER() OVER (ORDER BY T01ID  ) as  ##,T01GRN_No as [GRN No],M24Ref_No as [#Ref No],M24Date as [Date],T01Com_Invoice as [Com.Inv.No],M07Sup_Name as [Supplier Name],CAST(M24USD_Rate AS DECIMAL(16,2)) as [Total Amount USD],CAST(M24Unit_Cost AS DECIMAL(16,2)) as [Unit Cost],CAST(M24Tax AS DECIMAL(16,2)) as [Tax],CAST(M24Cus_Deuty AS DECIMAL(16,2)) as [Custom Duty],CAST(M24Shp_Chargers AS DECIMAL(16,2)) as [Shipping Charges],CAST(M24Comm AS DECIMAL(16,2)) as [Commission],CAST(M24Transport AS DECIMAL(16,2)) as [Transport],CAST(M24Bank AS DECIMAL(16,2)) as [Bank Chargers],CAST(M24VAT AS DECIMAL(16,2)) as [VAT],CAST(M24NBT AS DECIMAL(16,2)) as [NBT]   from T01GRN_Header inner join  M07Supplier on M07Sup_Code=T01Supp_Code inner join M24GRN_Cost_Analysis_Header on T01GRN_No=M24GRN where M24Status='A' and M24Date between '" & txtDate3.Text & "' and '" & txtDate4.Text & "' and M07Sup_Name='" & Trim(cboSupplier.Text) & "' order by T01ID DESC"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid1.DataSource = M01
            UltraGrid1.Rows.Band.Columns(0).Width = 30
            UltraGrid1.Rows.Band.Columns(1).Width = 80
            UltraGrid1.Rows.Band.Columns(2).Width = 80
            UltraGrid1.Rows.Band.Columns(3).Width = 80
            UltraGrid1.Rows.Band.Columns(4).Width = 80
            UltraGrid1.Rows.Band.Columns(5).Width = 180
            UltraGrid1.Rows.Band.Columns(6).Width = 80
            UltraGrid1.Rows.Band.Columns(7).Width = 80
            UltraGrid1.Rows.Band.Columns(8).Width = 100
            UltraGrid1.Rows.Band.Columns(9).Width = 100
            UltraGrid1.Rows.Band.Columns(10).Width = 100
            UltraGrid1.Rows.Band.Columns(11).Width = 100
            UltraGrid1.Rows.Band.Columns(12).Width = 100
            UltraGrid1.Rows.Band.Columns(13).Width = 100
            UltraGrid1.Rows.Band.Columns(14).Width = 100
            UltraGrid1.Rows.Band.Columns(15).Width = 100
            ' UltraGrid1.Rows.Band.Columns(16).Width = 110
            UltraGrid1.Rows.Band.Columns(0).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(1).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(2).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(3).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(6).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(7).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(8).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(9).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(10).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(11).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(12).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(13).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(14).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(15).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            'UltraGrid1.Rows.Band.Columns(16).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Private Sub frmview_Cost_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call Load_Grid()
        Call Load_Supplier()
    End Sub

    Private Sub RefreshToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshToolStripMenuItem.Click
        Call Load_Grid()
        Panel3.Visible = False
        Panel2.Visible = False
    End Sub

    Private Sub DeactivateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeactivateToolStripMenuItem.Click
        Panel3.Visible = True
        Panel2.Visible = False
        Panel1.Visible = False
        txtDate5.Text = Today
        txtDate6.Text = Today

    End Sub

    Private Sub UltraButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton3.Click
        Call Load_Grid_Date()
        Panel3.Visible = False
    End Sub
    Function Load_Supplier()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select M07Sup_Name as [##] from M07Supplier where M07Status='A' "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboSupplier
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 252
                ' .Rows.Band.Columns(1).Width = 180


            End With
            con.CLOSE()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Private Sub ConfirmGRNToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConfirmGRNToolStripMenuItem.Click
        Panel2.Visible = True
        Panel3.Visible = False
        Panel1.Visible = False
        cboSupplier.Text = ""
        txtDate3.Text = Today
        txtDate4.Text = Today
    End Sub

    Private Sub UltraButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton1.Click
        Call Load_Grid_Supplier()
        Panel2.Visible = False
    End Sub

    Private Sub CancelGRNToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelGRNToolStripMenuItem.Click
        Panel1.Visible = True
        Panel2.Visible = False
        Panel3.Visible = False
        txtSearch.Text = ""
    End Sub

    Private Sub txtSearch_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearch.ValueChanged
        Call Load_Grid_find()
    End Sub

    Private Sub UltraButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton2.Click
        Panel1.Visible = False
    End Sub

    Private Sub UltraGrid1_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles UltraGrid1.DoubleClickRow
        On Error Resume Next
        Dim _Row As Integer

        _Row = UltraGrid1.ActiveRow.Index

        frmGRN_Budget.txtEntry.Text = Trim(UltraGrid1.Rows(_Row).Cells(2).Text)
        frmGRN_Budget.cboGRN.Text = Trim(UltraGrid1.Rows(_Row).Cells(1).Text)
        frmGRN_Budget.Search_Records()
        Me.Close()
    End Sub
End Class