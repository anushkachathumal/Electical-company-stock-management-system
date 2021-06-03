Imports System.Data.SqlClient
Imports Infragistics.Win.UltraWinGrid
Imports DBLotVbnet.common
Imports System.Net
Imports DBLotVbnet.DBEngin
Imports DBLotVbnet.DAL_InterLocation
Imports DBLotVbnet.DAL_Distributors
Imports System.Configuration
Public Class frmview_GRN
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
            Sql = "select  ROW_NUMBER() OVER (ORDER BY T01ID  ) as  ##,T01GRN_No as [GRN No],T01Date as [Date],T01Com_Invoice as [Com.Inv.No],M07Sup_Name as [Supplier Name],CAST(T01Total_USD AS DECIMAL(16,2)) as [Total Amount USD],CAST(T01Total_Discount AS DECIMAL(16,2)) as [Discount USD],CAST(T01Total_USD-T01Total_Discount AS DECIMAL(16,2)) as [Invoice Value USD]  from T01GRN_Header inner join  M07Supplier on M07Sup_Code=T01Supp_Code where T01Status='A' order by T01ID"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid1.DataSource = M01
            UltraGrid1.Rows.Band.Columns(0).Width = 30
            UltraGrid1.Rows.Band.Columns(1).Width = 80
            UltraGrid1.Rows.Band.Columns(2).Width = 80
            UltraGrid1.Rows.Band.Columns(3).Width = 80
            UltraGrid1.Rows.Band.Columns(4).Width = 160
            UltraGrid1.Rows.Band.Columns(5).Width = 80
            UltraGrid1.Rows.Band.Columns(6).Width = 80
            UltraGrid1.Rows.Band.Columns(7).Width = 80
            UltraGrid1.Rows.Band.Columns(0).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(1).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(2).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(6).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(7).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(5).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function


    Function Load_Grid_DATE(ByVal strCode As String)
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "select  ROW_NUMBER() OVER (ORDER BY T01ID  ) as  ##,T01GRN_No as [GRN No],T01Date as [Date],T01Com_Invoice as [Com.Inv.No],M07Sup_Name as [Supplier Name],CAST(T01Total_USD AS DECIMAL(16,2)) as [Total Amount USD],CAST(T01Total_Discount AS DECIMAL(16,2)) as [Discount USD],CAST(T01Total_USD-T01Total_Discount AS DECIMAL(16,2)) as [Invoice Value USD]  from T01GRN_Header inner join  M07Supplier on M07Sup_Code=T01Supp_Code where T01Status='" & strCode & "' and  T01Date between '" & txtDate5.Text & "' and '" & txtDate6.Text & "' order by T01ID"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid1.DataSource = M01
            UltraGrid1.Rows.Band.Columns(0).Width = 30
            UltraGrid1.Rows.Band.Columns(1).Width = 80
            UltraGrid1.Rows.Band.Columns(2).Width = 80
            UltraGrid1.Rows.Band.Columns(3).Width = 80
            UltraGrid1.Rows.Band.Columns(4).Width = 160
            UltraGrid1.Rows.Band.Columns(5).Width = 80
            UltraGrid1.Rows.Band.Columns(6).Width = 80
            UltraGrid1.Rows.Band.Columns(7).Width = 80
            UltraGrid1.Rows.Band.Columns(0).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(1).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(2).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(6).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(7).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(5).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Function Load_Grid_sUPPLIER(ByVal strCode As String)
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "select  ROW_NUMBER() OVER (ORDER BY T01ID  ) as  ##,T01GRN_No as [GRN No],T01Date as [Date],T01Com_Invoice as [Com.Inv.No],M07Sup_Name as [Supplier Name],CAST(T01Total_USD AS DECIMAL(16,2)) as [Total Amount USD],CAST(T01Total_Discount AS DECIMAL(16,2)) as [Discount USD],CAST(T01Total_USD-T01Total_Discount AS DECIMAL(16,2)) as [Invoice Value USD]  from T01GRN_Header inner join  M07Supplier on M07Sup_Code=T01Supp_Code where T01Status='" & strCode & "' and  T01Date between '" & txtDate3.Text & "' and '" & txtDate4.Text & "' AND M07Sup_Name='" & Trim(cboSupplier.Text) & "' order by T01ID"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid1.DataSource = M01
            UltraGrid1.Rows.Band.Columns(0).Width = 30
            UltraGrid1.Rows.Band.Columns(1).Width = 80
            UltraGrid1.Rows.Band.Columns(2).Width = 80
            UltraGrid1.Rows.Band.Columns(3).Width = 80
            UltraGrid1.Rows.Band.Columns(4).Width = 160
            UltraGrid1.Rows.Band.Columns(5).Width = 80
            UltraGrid1.Rows.Band.Columns(6).Width = 80
            UltraGrid1.Rows.Band.Columns(7).Width = 80
            UltraGrid1.Rows.Band.Columns(0).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(1).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(2).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(6).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(7).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(5).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Private Sub frmview_GRN_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call Load_Grid()
        Call Load_Supplier()
    End Sub

    Private Sub RefreshToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshToolStripMenuItem.Click
        Panel3.Visible = False
        Panel2.Visible = False
        cboSupplier.Text = ""
        Call Load_Grid()
    End Sub

    Private Sub ByDateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ByDateToolStripMenuItem.Click
        Panel3.Visible = True
        Panel2.Visible = False
        txtDate5.Text = Today
        txtDate6.Text = Today
        _Print = "A1"
    End Sub

    Private Sub UltraButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton3.Click
        If _Print = "A1" Then
            Call Load_Grid_DATE("A")
            Panel3.Visible = False
        ElseIf _Print = "B1" Then
            Call Load_Grid_DATE("CONFIRM")
            Panel3.Visible = False
        ElseIf _Print = "C1" Then
            Call Load_Grid_DATE("CANCEL")
            Panel3.Visible = False
        End If
    End Sub

    Private Sub ByDateToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ByDateToolStripMenuItem1.Click
        Panel3.Visible = True
        Panel2.Visible = False
        txtDate5.Text = Today
        txtDate6.Text = Today
        _Print = "B1"
    End Sub

    Private Sub ByDateToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ByDateToolStripMenuItem2.Click
        Panel3.Visible = True
        Panel2.Visible = False
        txtDate5.Text = Today
        txtDate6.Text = Today
        _Print = "C1"
    End Sub

    Private Sub BySupplierToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BySupplierToolStripMenuItem.Click
        Panel3.Visible = False
        Panel2.Visible = True
        txtDate3.Text = Today
        txtDate4.Text = Today
        Call Load_Supplier()
        _Print = "A2"
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

    Private Sub UltraButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton1.Click
        If _Print = "A2" Then
            Call Load_Grid_sUPPLIER("A")
            Panel2.Visible = False
            cboSupplier.Text = ""
        ElseIf _Print = "B2" Then
            Call Load_Grid_sUPPLIER("CONFIRM")
            Panel2.Visible = False
            cboSupplier.Text = ""
        ElseIf _Print = "B3" Then
            Call Load_Grid_sUPPLIER("CANCEL")
            Panel2.Visible = False
            cboSupplier.Text = ""
        End If
    End Sub

    Private Sub BySupplierToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BySupplierToolStripMenuItem1.Click
        Panel3.Visible = False
        Panel2.Visible = True
        txtDate3.Text = Today
        txtDate4.Text = Today
        Call Load_Supplier()
        _Print = "B2"
    End Sub

    Private Sub BySupplierToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BySupplierToolStripMenuItem2.Click
        Panel3.Visible = False
        Panel2.Visible = True
        txtDate3.Text = Today
        txtDate4.Text = Today
        Call Load_Supplier()
        _Print = "B3"
    End Sub

    Private Sub UltraGrid1_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles UltraGrid1.DoubleClickRow
        On Error Resume Next
        Dim _ROW As Integer

        _ROW = UltraGrid1.ActiveRow.Index
        frmGRN_connect.txtEntry.Text = Trim(UltraGrid1.Rows(_ROW).Cells(1).Text)
        frmGRN_connect.Search_Records()
        Me.Close()
    End Sub

   
End Class