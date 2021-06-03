Imports System.Data.SqlClient
Imports Infragistics.Win.UltraWinGrid
Imports DBLotVbnet.common
Imports System.Net
Imports DBLotVbnet.DBEngin
Imports DBLotVbnet.DAL_InterLocation
Imports DBLotVbnet.DAL_Distributors
Imports System.Configuration
Public Class frmView_sales_cnt
    Dim _Print As String

    Private Sub ExitToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem1.Click
        Me.Close()
    End Sub

    Function Load_Grid()
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "select  ROW_NUMBER() OVER (ORDER BY  T12ID  ) as  ##, T12Tr_No as [Invoice No],T12Ref_No as [ref.No],T12Date as [Date],M02Root_Name as [Root Name],M05Shop_Name as [Shop Name],CAST((T12Net_Amount) AS DECIMAL(16,2)) as [Net Amount],CAST((T12Dis_rate) AS DECIMAL(16,2)) as [Discount %],CAST((T12Net_Amount-(T12Net_Amount*T12Dis_rate/100)) AS DECIMAL(16,2))  as [Gross Amount]  from T12Transaction_Header inner join M05New_Customer on M05Cus_Code=T12Cus_Code INNER JOIN M02New_Root  ON M02Root_Code=T12Root  where T12Status='A'  ORDER BY T12ID  "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid1.DataSource = M01
            UltraGrid1.Rows.Band.Columns(0).Width = 30
            UltraGrid1.Rows.Band.Columns(1).Width = 80
            UltraGrid1.Rows.Band.Columns(2).Width = 80
            UltraGrid1.Rows.Band.Columns(3).Width = 80
            UltraGrid1.Rows.Band.Columns(4).Width = 170
            UltraGrid1.Rows.Band.Columns(5).Width = 210
            UltraGrid1.Rows.Band.Columns(6).Width = 110
            UltraGrid1.Rows.Band.Columns(7).Width = 80
            UltraGrid1.Rows.Band.Columns(8).Width = 110
            UltraGrid1.Rows.Band.Columns(1).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(2).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(3).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(6).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(8).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(6).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(7).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            con.ClearAllPools()
            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.ClearAllPools()
                con.close()
            End If
        End Try
    End Function

    Function Load_Grid_Date(ByVal strcode As String)
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "select  ROW_NUMBER() OVER (ORDER BY  T12ID  ) as  ##, T12Tr_No as [Invoice No],T12Ref_No as [ref.No],T12Date as [Date],M02Root_Name as [Root Name],M05Shop_Name as [Shop Name],CAST((T12Net_Amount) AS DECIMAL(16,2)) as [Net Amount],CAST((T12Dis_rate) AS DECIMAL(16,2)) as [Discount %],CAST((T12Net_Amount-(T12Net_Amount*T12Dis_rate/100)) AS DECIMAL(16,2))  as [Gross Amount]  from T12Transaction_Header inner join M05New_Customer on M05Cus_Code=T12Cus_Code INNER JOIN M02New_Root  ON M02Root_Code=T12Root  where T12Status='" & strcode & "' and T12Date between '" & txtDate5.Text & "' and '" & txtDate6.Text & "' ORDER BY T12ID  "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid1.DataSource = M01
            UltraGrid1.Rows.Band.Columns(0).Width = 30
            UltraGrid1.Rows.Band.Columns(1).Width = 80
            UltraGrid1.Rows.Band.Columns(2).Width = 80
            UltraGrid1.Rows.Band.Columns(3).Width = 80
            UltraGrid1.Rows.Band.Columns(4).Width = 170
            UltraGrid1.Rows.Band.Columns(5).Width = 210
            UltraGrid1.Rows.Band.Columns(6).Width = 110
            UltraGrid1.Rows.Band.Columns(7).Width = 80
            UltraGrid1.Rows.Band.Columns(8).Width = 110
            UltraGrid1.Rows.Band.Columns(1).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(2).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(3).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(6).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(8).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(6).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(7).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            con.ClearAllPools()
            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.ClearAllPools()
                con.close()
            End If
        End Try
    End Function

    Function Load_Grid_Vehicle()
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "select  ROW_NUMBER() OVER (ORDER BY  T12ID  ) as  ##, T12Tr_No as [Invoice No],T12Ref_No as [ref.No],T12Date as [Date],M02Root_Name as [Root Name],M05Shop_Name as [Shop Name],CAST((T12Net_Amount) AS DECIMAL(16,2)) as [Net Amount],CAST((T12Dis_rate) AS DECIMAL(16,2)) as [Discount %],CAST((T12Net_Amount-(T12Net_Amount*T12Dis_rate/100)) AS DECIMAL(16,2))  as [Gross Amount]  from T12Transaction_Header inner join M05New_Customer on M05Cus_Code=T12Cus_Code INNER JOIN M02New_Root  ON M02Root_Code=T12Root  where T12Status='A' and T12Date between '" & txtA1.Text & "' and '" & txtA2.Text & "' and T12V_No='" & Trim(cboVehicle.Text) & "' ORDER BY T12ID  "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid1.DataSource = M01
            UltraGrid1.Rows.Band.Columns(0).Width = 30
            UltraGrid1.Rows.Band.Columns(1).Width = 80
            UltraGrid1.Rows.Band.Columns(2).Width = 80
            UltraGrid1.Rows.Band.Columns(3).Width = 80
            UltraGrid1.Rows.Band.Columns(4).Width = 170
            UltraGrid1.Rows.Band.Columns(5).Width = 210
            UltraGrid1.Rows.Band.Columns(6).Width = 110
            UltraGrid1.Rows.Band.Columns(7).Width = 80
            UltraGrid1.Rows.Band.Columns(8).Width = 110
            UltraGrid1.Rows.Band.Columns(1).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(2).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(3).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(6).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(8).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(6).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(7).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            con.ClearAllPools()
            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.ClearAllPools()
                con.close()
            End If
        End Try
    End Function

    Function Load_Grid_Customer()
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "select  ROW_NUMBER() OVER (ORDER BY  T12ID  ) as  ##, T12Tr_No as [Invoice No],T12Ref_No as [ref.No],T12Date as [Date],M02Root_Name as [Root Name],M05Shop_Name as [Shop Name],CAST((T12Net_Amount) AS DECIMAL(16,2)) as [Net Amount],CAST((T12Dis_rate) AS DECIMAL(16,2)) as [Discount %],CAST((T12Net_Amount-(T12Net_Amount*T12Dis_rate/100)) AS DECIMAL(16,2))  as [Gross Amount]  from T12Transaction_Header inner join M05New_Customer on M05Cus_Code=T12Cus_Code INNER JOIN M02New_Root  ON M02Root_Code=T12Root  where T12Status='A' and T12Date between '" & txtC1.Text & "' and '" & txtC2.Text & "' and M05Shop_Name='" & Trim(cboCustomer.Text) & "' ORDER BY T12ID  "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid1.DataSource = M01
            UltraGrid1.Rows.Band.Columns(0).Width = 30
            UltraGrid1.Rows.Band.Columns(1).Width = 80
            UltraGrid1.Rows.Band.Columns(2).Width = 80
            UltraGrid1.Rows.Band.Columns(3).Width = 80
            UltraGrid1.Rows.Band.Columns(4).Width = 170
            UltraGrid1.Rows.Band.Columns(5).Width = 210
            UltraGrid1.Rows.Band.Columns(6).Width = 110
            UltraGrid1.Rows.Band.Columns(7).Width = 80
            UltraGrid1.Rows.Band.Columns(8).Width = 110
            UltraGrid1.Rows.Band.Columns(1).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(2).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(3).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(6).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(8).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(6).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(7).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            con.ClearAllPools()
            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.ClearAllPools()
                con.close()
            End If
        End Try
    End Function


    Private Sub frmView_sales_cnt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call Load_Grid()
    End Sub

    Private Sub RefreshToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshToolStripMenuItem.Click
        Panel3.Visible = False
        Panel2.Visible = False
        Panel1.Visible = False
        Call Load_Grid()
    End Sub

    Private Sub DeactivateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeactivateToolStripMenuItem.Click
        'Panel3.Visible = True
        Panel2.Visible = False
        Panel1.Visible = False
        _Print = "A"
        txtDate5.Text = Today
        txtDate6.Text = Today

    End Sub

    Private Sub UltraButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton3.Click
        If _Print = "A" Then
            Call Load_Grid_Date("A")
            Panel3.Visible = False
        ElseIf _Print = "C" Then
            Call Load_Grid_Date("CANCEL")
            Panel3.Visible = False
        End If
    End Sub

    Private Sub CancelLoadingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelLoadingToolStripMenuItem.Click
        Call Load_Customer()
        Panel1.Visible = False
        Panel2.Visible = False
        Panel3.Visible = True
        _Print = "C"
        txtDate5.Text = Today
        txtDate6.Text = Today
    End Sub

    Private Sub ConfirmGRNToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConfirmGRNToolStripMenuItem.Click
        Panel1.Visible = True
        Panel3.Visible = False
        Call Load_Vehicle()
        cboVehicle.Text = ""
        _Print = "B"
        txtA1.Text = Today
        txtA2.Text = Today
    End Sub

    Function Load_Vehicle()
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet

        Try
            Sql = "select M06Vehi_No as [##] from M06Vehicle_Master where  M06Status='A' "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboVehicle
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 252
                '  .Rows.Band.Columns(1).Width = 320


            End With
            con.ClearAllPools()
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.ClearAllPools()
                con.close()
            End If
        End Try
    End Function

    Private Sub UltraButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton1.Click
        Call Load_Grid_Vehicle()
        Panel1.Visible = False
    End Sub

    Private Sub CancelGRNToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelGRNToolStripMenuItem.Click
        Panel2.Visible = True
        Panel1.Visible = False
        Panel3.Visible = False
        Call Load_Customer()
        txtC1.Text = Today
        txtC2.Text = Today
    End Sub


    Function Load_Customer()
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet

        Try
            Sql = "select M05Shop_Name as [##] from M05New_Customer where  M05Status='A'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboCustomer
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 302
                '  .Rows.Band.Columns(1).Width = 160


            End With
            con.ClearAllPools()
            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.ClearAllPools()
                con.close()
            End If
        End Try
    End Function

    Private Sub UltraButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton2.Click
        Call Load_Grid_Customer()
        Panel2.Visible = False
    End Sub

    Private Sub UltraGrid1_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles UltraGrid1.DoubleClickRow
        On Error Resume Next
        Dim _Row As Integer

        _Row = UltraGrid1.ActiveRow.Index
        frmsales_Cnt.txtEntry.Text = Trim(UltraGrid1.Rows(_Row).Cells(1).Text)
        frmsales_Cnt.Search_Records()
        Me.Close()
    End Sub
End Class