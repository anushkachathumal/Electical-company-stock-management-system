Imports System.Data.SqlClient
Imports Infragistics.Win.UltraWinGrid
Imports DBLotVbnet.common
Imports System.Net
Imports DBLotVbnet.DBEngin
Imports DBLotVbnet.DAL_InterLocation
Imports DBLotVbnet.DAL_Distributors
Imports System.Configuration
Public Class frmView_Loading
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
            Sql = "select  ROW_NUMBER() OVER (ORDER BY  T08Tr_No  ) as  ##, T08Tr_No as [Loading No],MAX(T08Date) as [Date],MAX(M03Loc_Name) as [From Location],MAX(T08To_Loc) as [Vehicle No],CAST(SUM(T09QTY*T09cOST) AS DECIMAL(16,2)) as [Total Amount]   from T08Loading_Header inner join M03New_Location on M03Loc_Code=T08From_Loc INNER JOIN T09Loading_Flutter  ON T08Tr_No=T09Tr_No  where T08Tr_Type='LOADING' AND T08Status='A' GROUP BY T08Tr_No ORDER BY max(T08ID)  "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid1.DataSource = M01
            UltraGrid1.Rows.Band.Columns(0).Width = 30
            UltraGrid1.Rows.Band.Columns(1).Width = 80
            UltraGrid1.Rows.Band.Columns(2).Width = 80
            UltraGrid1.Rows.Band.Columns(3).Width = 90
            UltraGrid1.Rows.Band.Columns(4).Width = 90
            UltraGrid1.Rows.Band.Columns(5).Width = 110
            
            UltraGrid1.Rows.Band.Columns(1).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(2).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            ' UltraGrid1.Rows.Band.Columns(3).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            'UltraGrid1.Rows.Band.Columns(6).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            'UltraGrid1.Rows.Band.Columns(7).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(5).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
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

    Function Load_Grid_Date(ByVal strCode As String)
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "select  ROW_NUMBER() OVER (ORDER BY  T08Tr_No  ) as  ##, T08Tr_No as [Loading No],MAX(T08Date) as [Date],MAX(M03Loc_Name) as [From Location],MAX(T08To_Loc) as [Vehicle No],CAST(SUM(T09QTY*T09cOST) AS DECIMAL(16,2)) as [Total Amount]   from T08Loading_Header inner join M03New_Location on M03Loc_Code=T08From_Loc INNER JOIN T09Loading_Flutter  ON T08Tr_No=T09Tr_No  where T08Tr_Type='LOADING' AND T08Status='" & strCode & "' and T08Date between '" & txtDate5.Text & "' and '" & txtDate6.Text & "' GROUP BY T08Tr_No ORDER BY max(T08ID)  "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid1.DataSource = M01
            UltraGrid1.Rows.Band.Columns(0).Width = 30
            UltraGrid1.Rows.Band.Columns(1).Width = 80
            UltraGrid1.Rows.Band.Columns(2).Width = 80
            UltraGrid1.Rows.Band.Columns(3).Width = 90
            UltraGrid1.Rows.Band.Columns(4).Width = 90
            UltraGrid1.Rows.Band.Columns(5).Width = 110

            UltraGrid1.Rows.Band.Columns(1).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(2).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            ' UltraGrid1.Rows.Band.Columns(3).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            'UltraGrid1.Rows.Band.Columns(6).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            'UltraGrid1.Rows.Band.Columns(7).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(5).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
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

    Function Load_Grid_Vehicle(ByVal strCode As String)
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "select  ROW_NUMBER() OVER (ORDER BY  T08Tr_No  ) as  ##, T08Tr_No as [Loading No],MAX(T08Date) as [Date],MAX(M03Loc_Name) as [From Location],MAX(T08To_Loc) as [Vehicle No],CAST(SUM(T09QTY*T09cOST) AS DECIMAL(16,2)) as [Total Amount]   from T08Loading_Header inner join M03New_Location on M03Loc_Code=T08From_Loc INNER JOIN T09Loading_Flutter  ON T08Tr_No=T09Tr_No  where T08Tr_Type='LOADING' AND T08Status='" & strCode & "' and T08Date between '" & txtA1.Text & "' and '" & txtA2.Text & "'  and T08To_Loc='" & Trim(cboVehicle.Text) & "' GROUP BY T08Tr_No ORDER BY max(T08ID)  "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid1.DataSource = M01
            UltraGrid1.Rows.Band.Columns(0).Width = 30
            UltraGrid1.Rows.Band.Columns(1).Width = 80
            UltraGrid1.Rows.Band.Columns(2).Width = 80
            UltraGrid1.Rows.Band.Columns(3).Width = 90
            UltraGrid1.Rows.Band.Columns(4).Width = 90
            UltraGrid1.Rows.Band.Columns(5).Width = 110

            UltraGrid1.Rows.Band.Columns(1).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(2).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            ' UltraGrid1.Rows.Band.Columns(3).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            'UltraGrid1.Rows.Band.Columns(6).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            'UltraGrid1.Rows.Band.Columns(7).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid1.Rows.Band.Columns(5).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
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

    Function Load_Grid_Item(ByVal strCode As String)
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "select  ROW_NUMBER() OVER (ORDER BY  T08Tr_No  ) as  ##, T08Tr_No as [Loading No],T08Date as [Date],M03Loc_Name as [From Location],T08To_Loc as [Vehicle No],T09Item_Code as [Item Code],M11Part_Name as [Item Name], T09Qty as [Qty]   from T08Loading_Header inner join M03New_Location on M03Loc_Code=T08From_Loc INNER JOIN T09Loading_Flutter  ON T08Tr_No=T09Tr_No inner join M11Product_Item on M11Part_No=T09Item_Code where T08Tr_Type='LOADING' AND T08Status='" & strCode & "' and T08Date between '" & txtC1.Text & "' and '" & txtC2.Text & "'  and T09Item_Code='" & Trim(cboItem.Text) & "'  ORDER BY T08ID  "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid1.DataSource = M01
            UltraGrid1.Rows.Band.Columns(0).Width = 30
            UltraGrid1.Rows.Band.Columns(1).Width = 80
            UltraGrid1.Rows.Band.Columns(2).Width = 80
            UltraGrid1.Rows.Band.Columns(3).Width = 90
            UltraGrid1.Rows.Band.Columns(4).Width = 90
            UltraGrid1.Rows.Band.Columns(5).Width = 110
            UltraGrid1.Rows.Band.Columns(6).Width = 210
            UltraGrid1.Rows.Band.Columns(7).Width = 80
            UltraGrid1.Rows.Band.Columns(1).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(2).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            ' UltraGrid1.Rows.Band.Columns(3).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            'UltraGrid1.Rows.Band.Columns(6).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            'UltraGrid1.Rows.Band.Columns(7).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
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


    Function Load_Item()
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet

        Try
            Sql = "select M11Part_No as [##],M11Part_Name as [Item Name] from M11Product_Item where  M11Status='A' and M11Type='PRODUCT ITEM' "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboItem
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 252
                .Rows.Band.Columns(1).Width = 320


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

    Private Sub frmView_Loading_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call Load_Grid()
    End Sub

    Private Sub RefreshToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshToolStripMenuItem.Click
        Call Load_Grid()
        Panel3.Visible = False
        Panel1.Visible = False
        Panel2.Visible = False
    End Sub

    Private Sub DeactivateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeactivateToolStripMenuItem.Click
        Panel3.Visible = True
        Panel2.Visible = False
        _Print = "A"
        txtDate5.Text = Today
        txtDate6.Text = Today

    End Sub

    Private Sub UltraButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton3.Click
        If _Print = "A" Then
            Call Load_Grid_Date("A")
            Panel3.Visible = False
        ElseIf _Print = "D" Then
            Call Load_Grid_Date("CANCEL")
            Panel3.Visible = False
        End If
    End Sub

    Private Sub ConfirmGRNToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConfirmGRNToolStripMenuItem.Click
        Panel3.Visible = False
        Panel1.Visible = True
        Panel2.Visible = False
        Call Load_Vehicle()
        cboVehicle.Text = ""
        _Print = "B"
        txtA1.Text = Today
        txtA2.Text = Today

    End Sub

    Private Sub UltraButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton1.Click
        Call Load_Grid_Vehicle("A")
        Panel1.Visible = False
    End Sub

    Private Sub CancelGRNToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelGRNToolStripMenuItem.Click
        Panel1.Visible = False
        Panel2.Visible = True
        Panel3.Visible = False
        txtC1.Text = Today
        txtC2.Text = Today
        Call Load_Item()
        cboItem.Text = ""
        cboItem.ToggleDropdown()
    End Sub

    Private Sub UltraButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton2.Click
        Call Load_Grid_Item("A")
        Panel2.Visible = False
    End Sub

    Private Sub UltraGrid1_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles UltraGrid1.DoubleClickRow
        On Error Resume Next
        Dim _Row As Integer

        _Row = UltraGrid1.ActiveRow.Index
        frmLoading_cnt.txtEntry.Text = Trim(UltraGrid1.Rows(_Row).Cells(1).Text)
        Call frmLoading_cnt.Search_Records()
        Call frmUnloading_cnt.Calculate_Net()
        Me.Close()
    End Sub

    Private Sub CancelLoadingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelLoadingToolStripMenuItem.Click
        Panel3.Visible = True
        Panel2.Visible = False
        _Print = "D"
        txtDate5.Text = Today
        txtDate6.Text = Today

    End Sub
End Class