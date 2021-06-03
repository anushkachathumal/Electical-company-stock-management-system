Imports System.Data.SqlClient
Imports Infragistics.Win.UltraWinGrid
Imports DBLotVbnet.common
Imports System.Net
Imports DBLotVbnet.DBEngin
Imports DBLotVbnet.DAL_InterLocation
Imports DBLotVbnet.DAL_Distributors
Imports System.Configuration
Imports CrystalDecisions.CrystalReports.Engine
Public Class frmLoading_cnt
    Dim dsUser As DataSet
    Dim Clicked As String
    Dim c_dataCustomer1 As DataTable
    Dim _Location As String
    Dim _Logstatus As Boolean
    Dim _AppUser As String

    Private Sub ExitToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem1.Click
        Me.Close()
    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Function Load_EntryNo()
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet

        Try
            Sql = "select * from P01Parameter where  P01Code='LD'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                If M01.Tables(0).Rows(0)("P01No") >= 1 And M01.Tables(0).Rows(0)("P01No") < 10 Then
                    txtEntry.Text = "LD-00" & M01.Tables(0).Rows(0)("P01No")
                ElseIf M01.Tables(0).Rows(0)("P01No") >= 10 And M01.Tables(0).Rows(0)("P01No") < 100 Then
                    txtEntry.Text = "LD-0" & M01.Tables(0).Rows(0)("P01No")
                Else
                    txtEntry.Text = "LD-" & M01.Tables(0).Rows(0)("P01No")
                End If
            End If

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
            Sql = "select M06Vehi_No as [##] from M06Vehicle_Master where  M06Status='A'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboVehicle
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 212
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

    Function Load_Item_Name()
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet

        Try
            Sql = "select M11Part_Name as [##] from M11Product_Item where  M11Status='A' and M11Type='PRODUCT ITEM'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboItemName
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 470
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

    Function Search_Records()
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Dim Value As Double
        Dim i As Integer
        Dim _ST As String

        Try
            Sql = "select *   from T08Loading_Header inner join M03New_Location on M03Loc_Code=T08From_Loc INNER JOIN T09Loading_Flutter  ON T08Tr_No=T09Tr_No inner join M11Product_Item on M11Part_No=T09Item_Code where T08Tr_Type='LOADING' AND T08Tr_No='" & Trim(txtEntry.Text) & "'  "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                cboLocation.Text = Trim(M01.Tables(0).Rows(0)("M03Loc_Name"))
                cboVehicle.Text = Trim(M01.Tables(0).Rows(0)("T08To_Loc"))
                txtDate.Text = Trim(M01.Tables(0).Rows(0)("T08Date"))
                txtRemark.Text = Trim(M01.Tables(0).Rows(0)("T08Remark"))
                cmdDelete.Enabled = True
                cmdEdit.Enabled = True
                Call Load_Gride2()
                i = 0
                For Each DTRow2 As DataRow In M01.Tables(0).Rows

                    Dim newRow As DataRow = c_dataCustomer1.NewRow
                    newRow("Item Code") = Trim(M01.Tables(0).Rows(i)("T09Item_Code"))
                    newRow("Item Name") = Trim(M01.Tables(0).Rows(i)("M11Part_Name"))
                    Value = Trim(M01.Tables(0).Rows(i)("T09Cost"))
                    _ST = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                    _ST = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
                    newRow("Cost Price") = _ST

                    Value = Trim(M01.Tables(0).Rows(i)("T09Rate"))
                    _ST = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                    _ST = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
                    newRow("Retail Price") = _ST

                    newRow("Qty") = Trim(M01.Tables(0).Rows(i)("T09Qty"))
                    'If IsDate(Trim(M01.Tables(0).Rows(i)("T02Ex_Date"))) Then
                    '    newRow("Ex Date") = Trim(M01.Tables(0).Rows(i)("T02Ex_Date"))
                    'End If
                    Value = Trim(M01.Tables(0).Rows(i)("T09Discount"))
                    _ST = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                    _ST = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
                    newRow("Discount %") = _ST
                    'newRow("Free Issue") = Trim(M01.Tables(0).Rows(i)("T02Free_Issue"))
                    Value = CDbl(M01.Tables(0).Rows(i)("T09Qty")) * CDbl(M01.Tables(0).Rows(i)("T09Rate"))
                    Value = Value - (Value * (CDbl(M01.Tables(0).Rows(i)("T09Discount")) / 100))
                    _ST = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                    _ST = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
                    newRow("Total") = _ST

                    c_dataCustomer1.Rows.Add(newRow)


                    i = i + 1
                Next
                txtCount.Text = UltraGrid1.Rows.Count

            End If
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
            Sql = "select M11Part_No as [##],M11Part_Name as [Item Name] from M11Product_Item where  M11Status='A' and M11Type='PRODUCT ITEM'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboCode
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 212
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

    Function Load_TO_Location()
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet

        Try
            Sql = "select M03Loc_Name as [From Location] from M03New_Location where  M03Status='A'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboLocation
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 212
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

    Private Sub frmLoading_cnt_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        frmView_Loading.Close()
    End Sub

    Private Sub frmLoading_cnt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call Load_EntryNo()
        txtCount.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtNet.Appearance.TextHAlign = Infragistics.Win.HAlign.Right
        txtNet.ReadOnly = True
        txtCount.ReadOnly = True
        txtTotal.Appearance.TextHAlign = Infragistics.Win.HAlign.Right
        txtTotal.ReadOnly = True
        txtEntry.ReadOnly = True
        txtEntry.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtDate.Text = Today
        txtRate.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtRetail.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtRate.ReadOnly = True
        txtRetail.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtRetail.ReadOnly = True
        ' txtL_Dis.ReadOnly = True
        txtL_Dis.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtQty.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        Call Load_Gride2()
        Call Load_TO_Location()
        Call Load_Vehicle()
        Call Load_Item()
        Call Load_Item_Name()
    End Sub

    Function Calculate_Total()
        On Error Resume Next
        Dim Value As Double
        If IsNumeric(txtRate.Text) And IsNumeric(txtQty.Text) Then
            Value = CDbl(txtRate.Text) * CDbl(txtQty.Text)
            If IsNumeric(txtL_Dis.Text) Then
                Value = Value - (Value * CDbl(txtL_Dis.Text) / 100)
            End If
            txtTotal.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
            txtTotal.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
        End If
    End Function

    Function Load_Gride2()
        Dim CustomerDataClass As New DAL_InterLocation()
        c_dataCustomer1 = CustomerDataClass.MakeDataTable_LOADING_CNT
        UltraGrid1.DataSource = c_dataCustomer1
        With UltraGrid1
            .DisplayLayout.Bands(0).Columns(0).Width = 90
            .DisplayLayout.Bands(0).Columns(1).AutoEdit = False
            .DisplayLayout.Bands(0).Columns(1).Width = 210
            .DisplayLayout.Bands(0).Columns(1).AutoEdit = False

            .DisplayLayout.Bands(0).Columns(2).Width = 80
            .DisplayLayout.Bands(0).Columns(3).Width = 80
            .DisplayLayout.Bands(0).Columns(4).Width = 70
            .DisplayLayout.Bands(0).Columns(5).Width = 80
            .DisplayLayout.Bands(0).Columns(6).Width = 110
            '.DisplayLayout.Bands(0).Columns(7).Width = 90
            ' .DisplayLayout.Bands(0).Columns(8).Width = 90

            .DisplayLayout.Bands(0).Columns(2).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            .DisplayLayout.Bands(0).Columns(3).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            .DisplayLayout.Bands(0).Columns(4).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            .DisplayLayout.Bands(0).Columns(5).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            '.DisplayLayout.Bands(0).Columns(7).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            .DisplayLayout.Bands(0).Columns(6).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right


            .DisplayLayout.Bands(0).Columns(0).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(1).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(2).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(3).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(4).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(5).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(6).CellActivation = Activation.NoEdit
            ' .DisplayLayout.Bands(0).Columns(7).CellActivation = Activation.NoEdit


            .DisplayLayout.Bands(0).Columns(0).CellActivation = Activation.NoEdit
            '  .DisplayLayout.Bands(0).Columns(6).Width = 90
            ' .DisplayLayout.Bands(0).Columns(7).Width = 90

            ' .DisplayLayout.Bands(0).Columns(3).Width = 300
            '.DisplayLayout.Bands(0).Columns(4).Width = 300
        End With
    End Function

    Function Clear_text()
        Me.cboCode.Text = ""
        Me.cboItemName.Text = ""
        Me.cboLocation.Text = ""
        Me.cboVehicle.Text = ""
        Me.txtRetail.Text = ""
        Me.txtRemark.Text = ""
        Me.txtRate.Text = ""
        Me.txtL_Dis.Text = ""
        Me.txtQty.Text = ""
        Me.txtCount.Text = ""
        Me.txtTotal.Text = ""
        Me.txtNet.Text = ""
        cmdDelete.Enabled = False
        cmdEdit.Enabled = False
        OPRUser.Visible = False
        _Logstatus = False
        txtDate.Text = Today
        Call Load_EntryNo()
        Call Load_Gride2()
    End Function

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        Call Clear_text()
    End Sub

    Private Sub RefreshToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshToolStripMenuItem.Click
        Call Clear_text()
    End Sub

    Private Sub cboLocation_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboLocation.KeyUp
        If e.KeyCode = 13 Then
            cboVehicle.ToggleDropdown()
        End If
    End Sub

    Private Sub cboVehicle_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboVehicle.KeyUp
        If e.KeyCode = 13 Then
            txtRemark.Focus()
        End If
    End Sub

    Private Sub txtRemark_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRemark.KeyUp
        If e.KeyCode = Keys.F1 Then
            cboCode.ToggleDropdown()
        End If
    End Sub

    Function Search_Item() As Boolean
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Dim vALUE As Double
        Dim _st As String
        Dim i As Integer
        Try
            Sql = "SELECT * FROM M11Product_Item WHERE M11Part_No='" & Trim(cboCode.Text) & "' AND M11Status='A' and M11Type='PRODUCT ITEM' "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                Search_Item = True
                cboItemName.Text = Trim(M01.Tables(0).Rows(0)("M11Part_Name"))
                vALUE = Trim(M01.Tables(0).Rows(0)("M11Retail_price"))
                txtRetail.Text = (vALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtRetail.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", vALUE))
            End If
            'AVG cost
            Sql = "select sum(M23Total/M23Qty) as Cost from M23Cost_Analysis where M23Item_Code='" & Trim(cboCode.Text) & "' and M23Status='A' group by M23Item_Code"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                vALUE = M01.Tables(0).Rows(0)("Cost")
                txtRate.Text = (vALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtRate.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", vALUE))
            Else
                txtRate.Text = "0"
            End If
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

    Private Sub cboCode_AfterDropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboCode.AfterDropDown
        Call Search_Item()
    End Sub

    Private Sub cboCode_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboCode.KeyUp
        If e.KeyCode = 13 Then
            If Trim(cboCode.Text) <> "" Then
                Call Search_Item()
                txtL_Dis.Focus()
            Else
                cboItemName.ToggleDropdown()
            End If
        End If
    End Sub

    Private Sub txtL_Dis_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtL_Dis.KeyUp
        If e.KeyCode = 13 Then
            Call Calculate_Total()
            txtQty.Focus()
        End If
    End Sub

    Private Sub txtL_Dis_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtL_Dis.TextChanged
        Call Calculate_Total()
    End Sub

    Private Sub txtQty_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtQty.KeyUp
        Dim Value As Double
        Dim _St As String
        Try
            If e.KeyCode = 13 Then
                If Search_Item() = True Then
                Else
                    MsgBox("Please enter the correct item code", MsgBoxStyle.Information, "Information .......")
                    Exit Sub
                End If

                If IsNumeric(txtQty.Text) Then
                Else
                    MsgBox("Please enter the correct Qty", MsgBoxStyle.Information, "Information ........")
                    Exit Sub
                End If

                If txtQty.Text <> "" Then
                Else
                    MsgBox("Please enter the Qty", MsgBoxStyle.Information, "Information .........")
                    Exit Sub
                End If
                If IsNumeric(txtL_Dis.Text) Then
                Else
                    txtL_Dis.Text = "0"
                End If

                If txtL_Dis.Text <> "" Then
                Else
                    txtL_Dis.Text = "0"
                End If
                If Search_Location() = True Then
                Else
                    MsgBox("Please select the from location", MsgBoxStyle.Information, "Information ..........")
                    Exit Sub
                End If
                If CALCULATING_AVAILABLE_STOCK() = True Then
                    MsgBox("Available stock less than a qty", MsgBoxStyle.Information, "Information ......")
                    Exit Sub
                End If

                Call Calculate_Total()
                Dim newRow As DataRow = c_dataCustomer1.NewRow
                newRow("Item Code") = Trim(cboCode.Text)
                newRow("Item Name") = Trim(cboItemName.Text)
                Value = txtRate.Text
                _St = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                _St = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
                newRow("Cost Price") = _St
                Value = txtRetail.Text
                _St = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                _St = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
                newRow("Retail Price") = _St

                Value = txtL_Dis.Text
                _St = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                _St = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
                newRow("Discount %") = _St

                newRow("Qty") = txtQty.Text

                Value = txtTotal.Text
                _St = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                _St = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
                newRow("Total") = _St

                c_dataCustomer1.Rows.Add(newRow)
                txtCount.Text = UltraGrid1.Rows.Count
                Call Calculate_Net()
                Me.txtTotal.Text = ""
                Me.txtRate.Text = ""
                Me.txtRetail.Text = ""
                Me.txtQty.Text = ""
                Me.cboCode.Text = ""
                Me.cboItemName.Text = ""
                Me.txtL_Dis.Text = ""
                cboCode.ToggleDropdown()
              
            End If
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
               
            End If
        End Try

    End Sub
    Function CALCULATING_AVAILABLE_STOCK() As Boolean
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "SELECT SUM(S01Qty) AS QTY FROM S01Stock_Product_Items WHERE S01Part_No='" & Trim(cboCode.Text) & "' AND S01Status IN('A','TMP')  GROUP  BY S01Part_No"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                If CDbl(M01.Tables(0).Rows(0)("QTY")) <= CDbl(txtQty.Text) Then
                    CALCULATING_AVAILABLE_STOCK = True
                End If
            End If

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
    Function Search_Location() As Boolean
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Dim vALUE As Double
        Dim _st As String
        Dim i As Integer
        Try
            Sql = "SELECT * FROM M03New_Location WHERE M03Loc_Name='" & Trim(cboLocation.Text) & "' AND M03Status='A' "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                Search_Location = True
                _Location = Trim(M01.Tables(0).Rows(0)("M03Loc_Code"))
               
            End If
           
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

    Function Search_Vehicle() As Boolean
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Dim vALUE As Double
        Dim _st As String
        Dim i As Integer
        Try
            Sql = "SELECT M06Vehi_No FROM M06Vehicle_Master WHERE M06Vehi_No='" & Trim(cboVehicle.Text) & "' AND M06Status='A' "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                Search_Vehicle = True
             
            End If

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

    Function Calculate_Net()
        On Error Resume Next
        Dim Value As Integer
        Dim I As Integer

        For Each uRow As UltraGridRow In UltraGrid1.Rows
            If IsNumeric(UltraGrid1.Rows(I).Cells(6).Text) Then
                Value = Value + CDbl(UltraGrid1.Rows(I).Cells(6).Text)
            End If
            I = I + 1
        Next

        txtNet.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
        txtNet.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
    End Function
    Private Sub txtQty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtQty.TextChanged
        Call Calculate_Total()
    End Sub

    Private Sub UltraGrid1_AfterRowsDeleted(ByVal sender As Object, ByVal e As System.EventArgs) Handles UltraGrid1.AfterRowsDeleted
        Call Calculate_Net()
        txtCount.Text = UltraGrid1.Rows.Count
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        If Search_Location() = True Then
        Else
            MsgBox("Please select the Location", MsgBoxStyle.Information, "Information ........")
            Exit Sub
        End If

        If Search_Vehicle() = True Then
        Else
            MsgBox("Please select the correct Vehicle No", MsgBoxStyle.Information, "Information ......")
            Exit Sub
        End If

        If Trim(txtRemark.Text) <> "" Then
        Else
            txtRemark.Text = "-"
        End If

        
        Call Save_Data()
    End Sub

    Function Save_Data()
        Dim nvcFieldList1 As String

        Dim connection As SqlClient.SqlConnection
        Dim transaction As SqlClient.SqlTransaction
        Dim transactionCreated As Boolean
        Dim connectionCreated As Boolean
        'connection.ClearAllPools()
        connection = DBEngin.GetConnection(True)
        connectionCreated = True
        transaction = connection.BeginTransaction()
        transactionCreated = True
        Dim MB51 As DataSet
        Dim _GetDate As DateTime
        Dim _Get_Time As DateTime
        Dim I As Integer
        Dim A As String
        Dim B As New ReportDocument
        Try
            _GetDate = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text)
            _Get_Time = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text) & " " & Hour(Now) & ":" & Minute(Now)

            nvcFieldList1 = "SELECT * FROM T08Loading_Header WHERE T08Tr_No='" & Trim(txtEntry.Text) & "' "
            MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
            If isValidDataset(MB51) Then
                MsgBox("This Loading no alrady exsist", MsgBoxStyle.Information, "Information ......")
                connection.ClearAllPools()
                connection.Close()
                Exit Function

            Else
                nvcFieldList1 = "update P01Parameter set P01No=P01No+ " & 1 & " where P01Code='LD' "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into T08Loading_Header(T08Tr_No,T08Tr_Type,T08Date,T08Remark,T08From_Loc,T08To_Loc,T08Status)" & _
                                                                  " values('" & Trim(txtEntry.Text) & "','LOADING', '" & _GetDate & "','" & txtRemark.Text & "','" & _Location & "','" & Trim(cboVehicle.Text) & "','A')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into tmpMaster_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                                                                " values('LOADING','SAVE', '" & Now & "','" & strDisname & "','" & txtEntry.Text & "')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                I = 0
                For Each uRow As UltraGridRow In UltraGrid1.Rows
                    nvcFieldList1 = "Insert Into T09Loading_Flutter(T09Tr_No,T09Item_Code,T09Cost,T09Rate,T09Discount,T09Qty,T09Status)" & _
                                                             " values('" & Trim(txtEntry.Text) & "','" & Trim(UltraGrid1.Rows(I).Cells(0).Text) & "', '" & Trim(UltraGrid1.Rows(I).Cells(2).Text) & "','" & Trim(UltraGrid1.Rows(I).Cells(3).Text) & "','" & Trim(UltraGrid1.Rows(I).Cells(4).Text) & "','" & Trim(UltraGrid1.Rows(I).Cells(5).Text) & "','A')"
                    ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                    '==================================================================
                    'UPDATE FROM LOCATION
                    nvcFieldList1 = "Insert Into S01Stock_Product_Items(S01Part_No,S01Loc_Code,S01Date,S01Tr_Type,S01Ref_No,S01Qty,S01Status)" & _
                                                          " values('" & Trim(UltraGrid1.Rows(I).Cells(0).Text) & "', '" & _Location & "','" & _GetDate & "','LOADING','" & Trim(txtEntry.Text) & "','" & -(UltraGrid1.Rows(I).Cells(5).Text) & "','A')"
                    ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                    '==================================================================
                    'UPDATE TO LOCATION 
                    nvcFieldList1 = "Insert Into S01Stock_Product_Items(S01Part_No,S01Loc_Code,S01Date,S01Tr_Type,S01Ref_No,S01Qty,S01Status)" & _
                                                      " values('" & Trim(UltraGrid1.Rows(I).Cells(0).Text) & "', '" & Trim(cboVehicle.Text) & "','" & _GetDate & "','LOADING','" & Trim(txtEntry.Text) & "','" & Trim(UltraGrid1.Rows(I).Cells(5).Text) & "','A')"
                    ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                    I = I + 1
                Next
            End If
            ' MsgBox("Record Update successfully", MsgBoxStyle.Information, "Information ..........")
            transaction.Commit()
            connection.ClearAllPools()
            connection.Close()
            A = MsgBox("Are you sure you want to print this Loading Report", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Information .........")
            If A = vbYes Then
                A = ConfigurationManager.AppSettings("ReportPath") + "\Loading.rpt"
                B.Load(A.ToString)
                B.SetDatabaseLogon("sa", "sainfinity")

                '  frmReport.CrystalReportViewer1.SelectionFormula = "{T01Transaction_Header.T01RefNo} =" & P01 & ""
                frmReport.CrystalReportViewer1.ReportSource = B 'intanance System\CrystalReport1" 'B ' "f:\salesinvo1.rpt" 'A.ToString '"F:\Stock Maintanance System\Report\salesinvo1.rpt"
                frmReport.CrystalReportViewer1.DisplayToolbar = True
                frmReport.CrystalReportViewer1.SelectionFormula = "{T08Loading_Header.T08Tr_No}='" & Trim(txtEntry.Text) & "'"
                frmReport.Refresh()
                ' frmReport.CrystalReportViewer1.PrintReport()
                ' B.PrintToPrinter(1, True, 0, 0)
                frmReport.MdiParent = MDIMain
                frmReport.Show()
            End If
            Call Clear_text()
           
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                connection.ClearAllPools()
                connection.Close()
            End If
        End Try
    End Function

    Private Sub DeactivateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeactivateToolStripMenuItem.Click
        frmView_Loading.Close()
        frmView_Loading.Show()
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        If _Logstatus = True Then
            If Search_Location() = True Then
            Else
                MsgBox("Please select the Location", MsgBoxStyle.Information, "Information ........")
                Exit Sub
            End If

            If Search_Vehicle() = True Then
            Else
                MsgBox("Please select the correct Vehicle No", MsgBoxStyle.Information, "Information ......")
                Exit Sub
            End If

            If Trim(txtRemark.Text) <> "" Then
            Else
                txtRemark.Text = "-"
            End If

            Call EDIT_DATA()

        Else
            OPRUser.Visible = True
            txtUserName.Text = ""
            txtPassword.Text = ""
            txtUserName.Focus()
        End If
    End Sub
    Function EDIT_DATA()
        Dim nvcFieldList1 As String

        Dim connection As SqlClient.SqlConnection
        Dim transaction As SqlClient.SqlTransaction
        Dim transactionCreated As Boolean
        Dim connectionCreated As Boolean
        'connection.ClearAllPools()
        connection = DBEngin.GetConnection(True)
        connectionCreated = True
        transaction = connection.BeginTransaction()
        transactionCreated = True
        Dim MB51 As DataSet
        Dim _GetDate As DateTime
        Dim _Get_Time As DateTime
        Dim I As Integer
        Dim A As String
        Dim B As New ReportDocument
        Try
            _GetDate = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text)
            _Get_Time = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text) & " " & Hour(Now) & ":" & Minute(Now)

            nvcFieldList1 = "SELECT * FROM T08Loading_Header WHERE T08Tr_No='" & Trim(txtEntry.Text) & "' "
            MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
            If isValidDataset(MB51) Then
                nvcFieldList1 = "UPDATE T08Loading_Header SET T08Date='" & _GetDate & "',T08Remark='" & Trim(txtRemark.Text) & "',T08From_Loc='" & _Location & "',T08To_Loc='" & Trim(cboVehicle.Text) & "',T08Status='A' WHERE T08Tr_No='" & Trim(txtEntry.Text) & "' AND T08Tr_Type='LOADING' "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into tmpMaster_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                                                       " values('LOADING','EDIT', '" & Now & "','" & _AppUser & "','" & txtEntry.Text & "')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "DELETE FROM T09Loading_Flutter  WHERE T09Tr_No='" & Trim(txtEntry.Text) & "'  "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "UPDATE S01Stock_Product_Items SET S01Status='CHANGE' WHERE S01Ref_No='" & Trim(txtEntry.Text) & "'  "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                I = 0
                For Each uRow As UltraGridRow In UltraGrid1.Rows
                    nvcFieldList1 = "Insert Into T09Loading_Flutter(T09Tr_No,T09Item_Code,T09Cost,T09Rate,T09Discount,T09Qty,T09Status)" & _
                                                             " values('" & Trim(txtEntry.Text) & "','" & Trim(UltraGrid1.Rows(I).Cells(0).Text) & "', '" & Trim(UltraGrid1.Rows(I).Cells(2).Text) & "','" & Trim(UltraGrid1.Rows(I).Cells(3).Text) & "','" & Trim(UltraGrid1.Rows(I).Cells(4).Text) & "','" & Trim(UltraGrid1.Rows(I).Cells(5).Text) & "','A')"
                    ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                    '==================================================================
                    'UPDATE FROM LOCATION
                    nvcFieldList1 = "Insert Into S01Stock_Product_Items(S01Part_No,S01Loc_Code,S01Date,S01Tr_Type,S01Ref_No,S01Qty,S01Status)" & _
                                                          " values('" & Trim(UltraGrid1.Rows(I).Cells(0).Text) & "', '" & _Location & "','" & _GetDate & "','LOADING','" & Trim(txtEntry.Text) & "','" & -(UltraGrid1.Rows(I).Cells(5).Text) & "','A')"
                    ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                    '==================================================================
                    'UPDATE TO LOCATION 
                    nvcFieldList1 = "Insert Into S01Stock_Product_Items(S01Part_No,S01Loc_Code,S01Date,S01Tr_Type,S01Ref_No,S01Qty,S01Status)" & _
                                                      " values('" & Trim(UltraGrid1.Rows(I).Cells(0).Text) & "', '" & Trim(cboVehicle.Text) & "','" & _GetDate & "','LOADING','" & Trim(txtEntry.Text) & "','" & Trim(UltraGrid1.Rows(I).Cells(5).Text) & "','A')"
                    ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                    I = I + 1
                Next

                MsgBox("Loading Note Change successfully", MsgBoxStyle.Information, "Information ..........")
            End If
            ' MsgBox("Record Update successfully", MsgBoxStyle.Information, "Information ..........")
            transaction.Commit()
            connection.ClearAllPools()
            connection.Close()
           
            Call Clear_text()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                connection.ClearAllPools()
                connection.Close()
            End If
        End Try
    End Function


    Function Delete_DATA()
        Dim nvcFieldList1 As String

        Dim connection As SqlClient.SqlConnection
        Dim transaction As SqlClient.SqlTransaction
        Dim transactionCreated As Boolean
        Dim connectionCreated As Boolean
        'connection.ClearAllPools()
        connection = DBEngin.GetConnection(True)
        connectionCreated = True
        transaction = connection.BeginTransaction()
        transactionCreated = True
        Dim MB51 As DataSet
        Dim _GetDate As DateTime
        Dim _Get_Time As DateTime
        Dim I As Integer
        Dim A As String
        Dim B As New ReportDocument
        Try
            _GetDate = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text)
            _Get_Time = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text) & " " & Hour(Now) & ":" & Minute(Now)

            nvcFieldList1 = "SELECT * FROM T08Loading_Header WHERE T08Tr_No='" & Trim(txtEntry.Text) & "' "
            MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
            If isValidDataset(MB51) Then
                nvcFieldList1 = "UPDATE T08Loading_Header SET T08Status='CANCEL' WHERE T08Tr_No='" & Trim(txtEntry.Text) & "' AND T08Tr_Type='LOADING' AND T08STATUS='A' "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into tmpMaster_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                                                       " values('LOADING','CANCEL', '" & Now & "','" & _AppUser & "','" & txtEntry.Text & "')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "UPDATE T09Loading_Flutter SET T09Status='CANCEL' WHERE T09Tr_No='" & Trim(txtEntry.Text) & "' AND T09STATUS='A'"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "UPDATE S01Stock_Product_Items SET S01Status='CANCEL' WHERE S01Ref_No='" & Trim(txtEntry.Text) & "' AND S01STATUS='A' "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                MsgBox("Loading Note Canceled successfully", MsgBoxStyle.Information, "Information ..........")
            End If
            ' MsgBox("Record Update successfully", MsgBoxStyle.Information, "Information ..........")
            transaction.Commit()
            connection.ClearAllPools()
            connection.Close()

            Call Clear_text()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                connection.ClearAllPools()
                connection.Close()
            End If
        End Try
    End Function

    Private Sub txtUserName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtUserName.KeyUp
        If e.KeyCode = 13 Then
            txtPassword.Focus()
        End If
    End Sub

    Private Sub txtPassword_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPassword.KeyUp
        If e.KeyCode = 13 Then
            OK.Focus()
        End If
    End Sub

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        Dim SQL As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        Dim M01 As DataSet

        Try
            con = DBEngin.GetConnection()
            SQL = "SELECT * FROM X02User_Loging WHERE (X02User_Name ='" & txtUserName.Text & "')and X02Password='" & txtPassword.Text & "' and X02User_Level in ('ADMIN','MANEGER','DIRECTOR') "
            M01 = DBEngin.ExecuteDataset(con, Nothing, SQL)
            If isValidDataset(M01) Then
                _Logstatus = True
                _AppUser = Trim(txtUserName.Text)
                OPRUser.Visible = False
            Else
                MsgBox("User name and pasword combination not found", MsgBoxStyle.Information, "Information ......")
                txtUserName.Focus()
                con.ClearAllPools()
                con.close()
                Exit Sub
            End If
            con.ClearAllPools()
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.ClearAllPools()
                con.close()
            End If

        End Try

    End Sub

    Private Sub cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cancel.Click
        OPRUser.Visible = False
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim A As String
        If _Logstatus = True Then

            A = MsgBox("Are you sure you want to cancel this Loading Note", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Information ........")
            If A = vbYes Then
                Call Delete_DATA()
            End If
        Else
            OPRUser.Visible = True
            txtUserName.Text = ""
            txtPassword.Text = ""
            txtUserName.Focus()
        End If
    End Sub

    Private Sub UltraButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton1.Click
        Dim A As String
        Dim B As New ReportDocument
        Try
            A = ConfigurationManager.AppSettings("ReportPath") + "\Loading.rpt"
            B.Load(A.ToString)
            B.SetDatabaseLogon("sa", "sainfinity")

            '  frmReport.CrystalReportViewer1.SelectionFormula = "{T01Transaction_Header.T01RefNo} =" & P01 & ""
            frmReport.CrystalReportViewer1.ReportSource = B 'intanance System\CrystalReport1" 'B ' "f:\salesinvo1.rpt" 'A.ToString '"F:\Stock Maintanance System\Report\salesinvo1.rpt"
            frmReport.CrystalReportViewer1.DisplayToolbar = True
            frmReport.CrystalReportViewer1.SelectionFormula = "{T08Loading_Header.T08Tr_No}='" & Trim(txtEntry.Text) & "'"
            frmReport.Refresh()
            ' frmReport.CrystalReportViewer1.PrintReport()
            ' B.PrintToPrinter(1, True, 0, 0)
            frmReport.MdiParent = MDIMain
            frmReport.Show()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                'con.ClearAllPools()
                'con.close()
            End If

        End Try
    End Sub

    Private Sub cboCode_InitializeLayout(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles cboCode.InitializeLayout

    End Sub

    Private Sub cboLocation_InitializeLayout(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles cboLocation.InitializeLayout

    End Sub
End Class