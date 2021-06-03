Imports System.Data.SqlClient
Imports Infragistics.Win.UltraWinGrid
Imports DBLotVbnet.common
Imports System.Net
Imports DBLotVbnet.DBEngin
Imports DBLotVbnet.DAL_InterLocation
Imports DBLotVbnet.DAL_Distributors
Imports System.Configuration
Imports CrystalDecisions.CrystalReports.Engine
Public Class frmGRN_connect
    Inherits System.Windows.Forms.Form
    Dim dsUser As DataSet
    Dim Clicked As String
    Dim c_dataCustomer1 As DataTable
    Dim _Category As String
    Dim _Comcode As String
    Dim _Loccode As String
    Dim _SupCode As String
    Dim _SupCode1 As String
    Dim _ItemLoc As String
    Dim _RefNo As Integer
    Dim _LogStaus As Boolean
    Dim _UserLevel As String
    Dim _AthzUser As String

    Function Search_Records()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet
        Dim vALUE As Double
        Dim _st As String
        Dim i As Integer


        Try
            Sql = "select * from T01GRN_Header inner join M07Supplier on M07Sup_Code=T01Supp_Code inner join M03New_Location on M03Loc_Code=T01To_Loc where  T01Status<>'CONFIRM' AND T01GRN_No='" & Trim(txtEntry.Text) & "'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                txtCom_Invoice.Text = Trim(M01.Tables(0).Rows(0)("T01Com_Invoice"))
                txtDate.Text = Trim(M01.Tables(0).Rows(0)("T01Date"))
                txtPO.Text = Trim(M01.Tables(0).Rows(0)("T01Shipping_No"))
                cboLocation.Text = Trim(M01.Tables(0).Rows(0)("M07Sup_Name"))
                cboTo.Text = Trim(M01.Tables(0).Rows(0)("M03Loc_Name"))
                vALUE = Trim(M01.Tables(0).Rows(0)("T01Total_Discount"))
                txtDiscount.Text = (vALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtDiscount.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", vALUE))
            End If

            Sql = "SELECT * FROM T02GRN_Flutter INNER JOIN M11Product_Item ON M11Part_No=T02Item_Code WHERE T02GRN_No='" & Trim(txtEntry.Text) & "' "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            i = 0
            For Each DTRow2 As DataRow In M01.Tables(0).Rows

                Dim newRow As DataRow = c_dataCustomer1.NewRow
                newRow("Item Code") = Trim(M01.Tables(0).Rows(i)("T02Item_Code"))
                newRow("Item Name") = Trim(M01.Tables(0).Rows(i)("M11Part_Name"))
                vALUE = Trim(M01.Tables(0).Rows(i)("T02Cost"))
                _st = (vALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                _st = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", vALUE))
                newRow("Cost Price") = _st
                newRow("Qty") = Trim(M01.Tables(0).Rows(i)("T02Qty"))
                'If IsDate(Trim(M01.Tables(0).Rows(i)("T02Ex_Date"))) Then
                '    newRow("Ex Date") = Trim(M01.Tables(0).Rows(i)("T02Ex_Date"))
                'End If
                vALUE = Trim(M01.Tables(0).Rows(i)("T02Discount"))
                _st = (vALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                _st = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", vALUE))
                newRow("Discount %") = _st
                newRow("Free Issue") = Trim(M01.Tables(0).Rows(i)("T02Free_Issue"))
                vALUE = CDbl(M01.Tables(0).Rows(i)("T02Qty")) * CDbl(M01.Tables(0).Rows(i)("T02Cost"))
                _st = (vALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                _st = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", vALUE))
                newRow("Total") = _st
              
                c_dataCustomer1.Rows.Add(newRow)


                i = i + 1
            Next
            txtCount.Text = UltraGrid1.Rows.Count

            Call Calculation_Net()
            cmdEdit.Enabled = True
            cmdDelete.Enabled = True
            con.CLOSE()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.CLOSE()
            End If
        End Try


    End Function
    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Function Load_Combo()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select M07Sup_Name as [From Location] from M07Supplier where  M07Status='A'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboLocation
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 296
                '  .Rows.Band.Columns(1).Width = 160


            End With
            con.CLOSE()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.CLOSE()
            End If
        End Try
    End Function

    Function Load_TO_Location()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select M03Loc_Name as [From Location] from M03New_Location where  M03Status='A'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboTo
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 283
                '  .Rows.Band.Columns(1).Width = 160


            End With
            con.CLOSE()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.CLOSE()
            End If
        End Try
    End Function

    Function Load_Gride2()
        Dim CustomerDataClass As New DAL_InterLocation()
        c_dataCustomer1 = CustomerDataClass.MakeDataTableGRN
        UltraGrid1.DataSource = c_dataCustomer1
        With UltraGrid1
            .DisplayLayout.Bands(0).Columns(0).Width = 90
            .DisplayLayout.Bands(0).Columns(1).AutoEdit = False
            .DisplayLayout.Bands(0).Columns(1).Width = 210
            .DisplayLayout.Bands(0).Columns(1).AutoEdit = False

            .DisplayLayout.Bands(0).Columns(2).Width = 70
            .DisplayLayout.Bands(0).Columns(3).Width = 70
            .DisplayLayout.Bands(0).Columns(4).Width = 70
            .DisplayLayout.Bands(0).Columns(5).Width = 70
            .DisplayLayout.Bands(0).Columns(6).Width = 80
            .DisplayLayout.Bands(0).Columns(7).Width = 90
            ' .DisplayLayout.Bands(0).Columns(8).Width = 90

            .DisplayLayout.Bands(0).Columns(2).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            .DisplayLayout.Bands(0).Columns(3).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            .DisplayLayout.Bands(0).Columns(4).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            .DisplayLayout.Bands(0).Columns(5).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            .DisplayLayout.Bands(0).Columns(7).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            .DisplayLayout.Bands(0).Columns(6).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right


            .DisplayLayout.Bands(0).Columns(0).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(1).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(2).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(3).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(4).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(5).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(6).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(7).CellActivation = Activation.NoEdit


            .DisplayLayout.Bands(0).Columns(0).CellActivation = Activation.NoEdit
            '  .DisplayLayout.Bands(0).Columns(6).Width = 90
            ' .DisplayLayout.Bands(0).Columns(7).Width = 90

            ' .DisplayLayout.Bands(0).Columns(3).Width = 300
            '.DisplayLayout.Bands(0).Columns(4).Width = 300
        End With
    End Function

    Private Sub frmGRN_connect_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        frmview_GRN.Close()
    End Sub
    Private Sub frmGRN_connect_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call Load_Combo()
        txtDate.Text = Today
        txtEntry.ReadOnly = True
        txtEntry.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtMRP.ReadOnly = True
        txtMRP.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtRate.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtQty.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtL_Dis.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtTotal.ReadOnly = True
        txtTotal.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtFree.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        ' txtFree.ReadOnly = True

        Call Load_EntryNo()
        Call Load_TO_Location()
        Call Load_Item_Code()
        Call Load_Item_Name()
        Call Load_Gride2()
        txtL_Dis.Text = "0"
        txtNBT.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtCount.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtCount.ReadOnly = True
        txtVAT.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtDiscount.Appearance.TextHAlign = Infragistics.Win.HAlign.Right
        txtNett.Appearance.TextHAlign = Infragistics.Win.HAlign.Right
        txtNett.ReadOnly = True
        txtGross.Appearance.TextHAlign = Infragistics.Win.HAlign.Right
        txtGross.ReadOnly = True
    End Sub

    Function Calculation_Net()
        On Error Resume Next
        Dim i As Integer
        Dim Value As Double

        For Each uRow As UltraGridRow In UltraGrid1.Rows
            ' MsgBox(Double.TryParse(txtNett.Text, value))
            Value = Value + CDbl((UltraGrid1.Rows(i).Cells(7).Value))
            i = i + 1
        Next
        txtNett.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
        txtNett.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
        If IsNumeric(txtDiscount.Text) Then
            Value = Value - CDbl(txtDiscount.Text)
        End If
        txtGross.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
        txtGross.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))

    End Function
    Function Load_EntryNo()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select * from P01Parameter where  P01Code='GRN'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                If M01.Tables(0).Rows(0)("P01No") >= 1 And M01.Tables(0).Rows(0)("P01No") < 10 Then
                    txtEntry.Text = "GRN-00" & M01.Tables(0).Rows(0)("P01No")
                ElseIf M01.Tables(0).Rows(0)("P01No") >= 10 And M01.Tables(0).Rows(0)("P01No") < 100 Then
                    txtEntry.Text = "GRN-0" & M01.Tables(0).Rows(0)("P01No")
                Else
                    txtEntry.Text = "GRN-" & M01.Tables(0).Rows(0)("P01No")
                End If
            End If

            'Sql = "select * from M04Location"
            'M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            'If isValidDataset(M01) Then
            '    cboTo.Text = M01.Tables(0).Rows(0)("M04Loc_Name")
            'End If
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.close()
            End If
        End Try
    End Function

    Function Load_Item_Code()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select M11Part_No as [##],M11Part_Name as [Item Name] from M11Product_Item where M11Status='A' order by M11ID "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboCode
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 212
                .Rows.Band.Columns(1).Width = 310


            End With
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.close()
            End If
        End Try
    End Function

    Function Load_Item_Name()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select M11Part_Name as [##] from M11Product_Item where M11Status='A' order by M11ID "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboItemName
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 522
                '  .Rows.Band.Columns(1).Width = 160


            End With
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.close()
            End If
        End Try
    End Function

    Private Sub cboLocation_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboLocation.KeyUp
        If e.KeyCode = 13 Then
            txtCom_Invoice.Focus()
        End If
    End Sub

    Private Sub txtCom_Invoice_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCom_Invoice.KeyUp
        If e.KeyCode = 13 Then
            txtPO.Focus()
        End If
    End Sub

    Private Sub txtPO_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPO.KeyUp
        If e.KeyCode = 13 Then
            cboTo.ToggleDropdown()
        End If
    End Sub

    Private Sub cboTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboTo.KeyUp
        If e.KeyCode = 13 Then
            cboCode.ToggleDropdown()
        End If
    End Sub

    Function Search_ItemName() As Boolean
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet
        Dim Value As Double
        Try
            Sql = "select * from M11Product_Item where  M11Part_No='" & Trim(cboCode.Text) & "' and M11Status='A' "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                Search_ItemName = True
                cboItemName.Text = Trim(M01.Tables(0).Rows(0)("M11Part_Name"))
               

            End If
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.close()
            End If
        End Try
    End Function

    Function Search_Itemcode() As Boolean
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet
        Dim Value As Double
        Try
            Sql = "select * from M11Product_Item where  M11Part_Name='" & Trim(cboItemName.Text) & "' and M11Status='A' "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                Search_Itemcode = True
                cboCode.Text = Trim(M01.Tables(0).Rows(0)("M11Part_No"))


            End If
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.close()
            End If
        End Try
    End Function

    Private Sub cboCode_AfterCloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboCode.AfterCloseUp
        Call Search_ItemName()
    End Sub

    Private Sub cboCode_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboCode.KeyUp
        If e.KeyCode = 13 Then
            If Trim(cboCode.Text) <> "" Then
                Call Search_ItemName()
                txtRate.Focus()
            Else
                cmdAdd.Focus()
            End If
        End If
    End Sub

    Private Sub txtRate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRate.KeyUp
        Dim Value As Double
        If e.KeyCode = 13 Then
            If IsNumeric(txtRate.Text) Then
                Value = txtRate.Text
                txtRate.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtRate.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
            End If
            txtQty.Focus()
        End If
    End Sub

    Function Calculation_Total()
        On Error Resume Next
        Dim Value As Double
        If IsNumeric(txtQty.Text) And IsNumeric(txtRate.Text) Then
            Value = CDbl(txtQty.Text) * CDbl(txtRate.Text)
        End If
        If IsNumeric(txtL_Dis.Text) Then
            Value = Value - (Value * CDbl(txtL_Dis.Text) / 100)

        End If
        txtTotal.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
        txtTotal.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
    End Function

    Private Sub txtRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRate.TextChanged
        Call Calculation_Total()
    End Sub

    Private Sub txtQty_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtQty.KeyUp
        If e.KeyCode = 13 Then
            txtL_Dis.Focus()
        End If
    End Sub


    Private Sub txtQty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtQty.TextChanged
        Call Calculation_Total()
    End Sub

    Private Sub txtFree_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFree.KeyUp
        If e.KeyCode = 13 Then
            txtTotal.Focus()
        End If
    End Sub

    Private Sub txtL_Dis_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtL_Dis.KeyUp
        If e.KeyCode = 13 Then
            txtFree.Focus()
        End If
    End Sub

    Private Sub txtTotal_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTotal.KeyUp
        Try
            If e.KeyCode = 13 Then
                If Search_ItemName() = True Then
                Else
                    MsgBox("Please enter the Correct Item Code", MsgBoxStyle.Information, "Information ........")
                    cboCode.ToggleDropdown()
                    Exit Sub
                End If

                If txtRate.Text <> "" Then
                Else
                    txtRate.Text = "0"
                End If
                If IsNumeric(txtRate.Text) Then
                Else
                    MsgBox("Please enter the correct cost price", MsgBoxStyle.Information, "Information ........")
                    txtRate.Focus()
                    Exit Sub
                End If
                '-----------------------------------------------------------------------------------------
                If txtQty.Text <> "" Then
                Else
                    txtQty.Text = "0"
                End If
                If IsNumeric(txtQty.Text) Then
                Else
                    MsgBox("Please enter the correct Qty", MsgBoxStyle.Information, "Information ........")
                    txtQty.Focus()
                    Exit Sub
                End If
                '------------------------------------------------------------------------------------------
                If txtL_Dis.Text <> "" Then
                Else
                    txtL_Dis.Text = "0"
                End If
                If IsNumeric(txtL_Dis.Text) Then
                Else
                    MsgBox("Please enter the correct Discount", MsgBoxStyle.Information, "Information ........")
                    txtL_Dis.Focus()
                    Exit Sub
                End If

                If txtFree.Text <> "" Then
                Else
                    txtFree.Text = "0"
                End If

                If IsNumeric(txtFree.Text) Then
                Else
                    txtFree.Text = "0"
                End If

                Call Calculation_Total()
                Dim newRow As DataRow = c_dataCustomer1.NewRow
                newRow("Item Code") = Trim(cboCode.Text)
                newRow("Item Name") = cboItemName.Text
                newRow("Cost Price") = txtRate.Text
                newRow("Qty") = txtQty.Text
                newRow("Discount %") = txtL_Dis.Text
                ' newRow("Rec.Qty") = txtRe_Qty.Text
                newRow("Free Issue") = txtFree.Text
                newRow("Total") = txtTotal.Text
                c_dataCustomer1.Rows.Add(newRow)
                txtCount.Text = UltraGrid1.Rows.Count
                Call Calculation_Net()
                Me.txtFree.Text = ""
                Me.txtTotal.Text = "00.00"
                Me.txtL_Dis.Text = "0"
                Me.txtQty.Text = ""
                Me.txtRate.Text = ""
                Me.cboCode.Text = ""
                Me.cboItemName.Text = ""
                cboCode.ToggleDropdown()
            End If

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)

            End If
        End Try
    End Sub

    Function SEARCH_SUPPLIER() As Boolean
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet
        Dim Value As Double
        Try
            Sql = "select M07Sup_Code from M07Supplier where  M07Sup_Name='" & Trim(cboLocation.Text) & "' and M07Status='A' "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                SEARCH_SUPPLIER = True
                _SupCode = Trim(M01.Tables(0).Rows(0)("M07Sup_Code"))


            End If
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.close()
            End If
        End Try
    End Function

    Function SEARCH_LOCATION() As Boolean
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet
        Dim Value As Double
        Try
            Sql = "select M03Loc_Code from M03New_Location where  M03Loc_Name='" & Trim(cboTo.Text) & "' and M03Status='A' "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                SEARCH_LOCATION = True
                _Loccode = Trim(M01.Tables(0).Rows(0)("M03Loc_Code"))


            End If
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.close()
            End If
        End Try
    End Function

    Private Sub UltraGrid1_AfterRowsDeleted(ByVal sender As Object, ByVal e As System.EventArgs) Handles UltraGrid1.AfterRowsDeleted
        Call Calculation_Net()
        txtCount.Text = UltraGrid1.Rows.Count
    End Sub

    Private Sub txtDiscount_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDiscount.ValueChanged
        Call Calculation_Net()
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        If SEARCH_SUPPLIER() = True Then
        Else
            MsgBox("Please select the supplier", MsgBoxStyle.Information, "Information ........")
            cboLocation.ToggleDropdown()
            Exit Sub
        End If

        If SEARCH_LOCATION() = True Then
        Else
            MsgBox("Please select the location", MsgBoxStyle.Information, "Information ........")
            cboTo.ToggleDropdown()
            Exit Sub
        End If

        If Trim(txtCom_Invoice.Text) <> "" Then
        Else
            txtCom_Invoice.Text = "-"
        End If

        If txtPO.Text <> "" Then
        Else
            txtPO.Text = "-"
        End If

        If txtRemark.Text <> "" Then
        Else
            txtRemark.Text = "-"
        End If

        Call SAVE_Data()

    End Sub

    Function EDIT_DATA()
        Dim nvcFieldList1 As String

        Dim connection As SqlClient.SqlConnection
        Dim transaction As SqlClient.SqlTransaction
        Dim transactionCreated As Boolean
        Dim connectionCreated As Boolean

        connection = DBEngin.GetConnection(True)
        connectionCreated = True
        transaction = connection.BeginTransaction()
        transactionCreated = True
        Dim MB51 As DataSet
        Dim M01 As DataSet

        Dim i As Integer
        Dim _GETDATE As DateTime
        Dim _GET_TIME As DateTime
        Dim A As String
        Dim _sTRING As String
        Dim B As New ReportDocument
        Try
            _sTRING = Trim(txtEntry.Text)
            Call Calculation_Net()

            _GETDATE = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text)

            _GET_TIME = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text) & " " & Hour(Now) & ":" & Minute(Now)

            nvcFieldList1 = "select * from T01GRN_Header where  T01GRN_No='" & Trim(txtEntry.Text) & "'  AND T01Status IN ('A','CANCEL')"
            MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
            If isValidDataset(MB51) Then
                nvcFieldList1 = "SELECT * FROM T20Supplier_Payment WHERE T20Ref_No='" & txtEntry.Text & "' AND T20Status='PAID'"
                M01 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
                If isValidDataset(M01) Then
                    MsgBox("You can not edit this GRN.alrady paid for this GRN", MsgBoxStyle.Information, "Information .........")
                    connection.Close()
                    Exit Function
                End If

                nvcFieldList1 = "UPDATE T01GRN_Header SET T01Status='A',T01Date='" & _GETDATE & "',T01Time='" & _GET_TIME & "',T01Com_Invoice='" & Trim(txtCom_Invoice.Text) & "',T01Supp_Code='" & _SupCode & "',T01To_Loc='" & _Loccode & "',T01Shipping_No='" & Trim(txtPO.Text) & "',T01Total_USD='" & txtNett.Text & "',T01Total_Discount='" & txtDiscount.Text & "',T01Remark='" & Trim(txtRemark.Text) & "' WHERE T01GRN_No='" & txtEntry.Text & "'"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "UPDATE T20Supplier_Payment SET T20Supplier='" & _SupCode & "',T20Date='" & _GETDATE & "',T20CR='" & txtGross.Text & "',T20Status='A' WHERE T20Ref_No='" & txtEntry.Text & "'"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "delete from T02GRN_Flutter where T02GRN_No='" & Trim(txtEntry.Text) & "'"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into tmpTransaction_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                                                               " values('GRN', 'EDIT','" & _GET_TIME & "','" & strDisname & "','" & Trim(txtEntry.Text) & "')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                i = 0
                For Each uRow As UltraGridRow In UltraGrid1.Rows
                    nvcFieldList1 = "Insert Into T02GRN_Flutter(T02GRN_No,T02Item_Code,T02Cost,T02Qty,T02Discount,T02Free_Issue,T02Con_Qty,T02Status,T02Act_Cost)" & _
                                                                 " values('" & Trim(txtEntry.Text) & "', '" & (UltraGrid1.Rows(i).Cells(0).Value) & "','" & (UltraGrid1.Rows(i).Cells(2).Value) & "','" & (UltraGrid1.Rows(i).Cells(3).Value) & "','" & (UltraGrid1.Rows(i).Cells(4).Value) & "','" & (UltraGrid1.Rows(i).Cells(5).Value) & "','0','A','0')"
                    ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                   
                    i = i + 1
                Next
                MsgBox("Records change successfully", MsgBoxStyle.Information, "Information ......")
                transaction.Commit()
                connection.Close()
            Else
                MsgBox("This GRN can't change", MsgBoxStyle.Information, "Information ........")
            End If
            Call Clear_text()
            ' Call Load_EntryNo()
            cboLocation.ToggleDropdown()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                connection.Close()
            End If
        End Try
    End Function
    Function SAVE_Data()
        Dim nvcFieldList1 As String

        Dim connection As SqlClient.SqlConnection
        Dim transaction As SqlClient.SqlTransaction
        Dim transactionCreated As Boolean
        Dim connectionCreated As Boolean

        connection = DBEngin.GetConnection(True)
        connectionCreated = True
        transaction = connection.BeginTransaction()
        transactionCreated = True
        Dim MB51 As DataSet
        Dim i As Integer
        Dim _GETDATE As DateTime
        Dim _GET_TIME As DateTime
        Dim A As String
        Dim _sTRING As String
        Dim B As New ReportDocument
        Try
            _sTRING = Trim(txtEntry.Text)
            Call Calculation_Net()

            _GETDATE = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text)

            _GET_TIME = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text) & " " & Hour(Now) & ":" & Minute(Now)

            nvcFieldList1 = "select * from T01GRN_Header where T01Status='A' and T01GRN_No='" & Trim(txtEntry.Text) & "'"
            MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
            If isValidDataset(MB51) Then
                MsgBox("This GRN no alrady exsist", MsgBoxStyle.Information, "Information ........")
                connection.Close()
                Exit Function
            Else
                Call Load_EntryNo()
                nvcFieldList1 = "update P01Parameter set P01No=P01No+ " & 1 & " where P01Code='GRN' "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into T01GRN_Header(T01GRN_No,T01Date,T01Com_Invoice,T01Supp_Code,T01Shipping_No,T01Total_USD,T01Total_Tax,T01Total_Discount,T01Shipping_Chargers,T01Status,T01Time,T01Remark,T01To_Loc,T01Cost_Status)" & _
                                                                 " values('" & Trim(txtEntry.Text) & "', '" & _GETDATE & "','" & txtCom_Invoice.Text & "','" & _SupCode & "','" & Trim(txtPO.Text) & "','" & txtNett.Text & "','0','" & txtDiscount.Text & "','0','A','" & _GET_TIME & "','" & txtRemark.Text & "','" & _Loccode & "','YES')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into T20Supplier_Payment(T20Ref_No,T20Supplier,T20Tr_Type,T20Date,T20Remark,T20CR,T20DR,T20Status)" & _
                                                               " values('" & Trim(txtEntry.Text) & "', '" & _SupCode & "','GRN','" & _GETDATE & "','" & _sTRING & "','" & CDbl(txtGross.Text) & "','0','A')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into tmpTransaction_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                                                               " values('GRN', 'SAVE','" & _GET_TIME & "','" & strDisname & "','" & Trim(txtEntry.Text) & "')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                i = 0
                For Each uRow As UltraGridRow In UltraGrid1.Rows
                    nvcFieldList1 = "Insert Into T02GRN_Flutter(T02GRN_No,T02Item_Code,T02Cost,T02Qty,T02Discount,T02Free_Issue,T02Con_Qty,T02Status,T02Act_Cost)" & _
                                                                 " values('" & Trim(txtEntry.Text) & "', '" & (UltraGrid1.Rows(i).Cells(0).Value) & "','" & (UltraGrid1.Rows(i).Cells(2).Value) & "','" & (UltraGrid1.Rows(i).Cells(3).Value) & "','" & (UltraGrid1.Rows(i).Cells(4).Value) & "','" & (UltraGrid1.Rows(i).Cells(5).Value) & "','0','A','0')"
                    ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                    'nvcFieldList1 = "SELECT * FROM M11Product_Item WHERE M11Part_No='" & Trim(UltraGrid1.Rows(i).Cells(0).Value) & "' AND M11Status='A'"
                    'MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
                    'If isValidDataset(MB51) Then
                    '    If Trim(MB51.Tables(0).Rows(0)("M11Type")) = "ROW ITEMS" Then
                    '        nvcFieldList1 = "Insert Into S02Row_Stock_Items(S02Part_No,S02Loc_Code,S02Date,S02Tr_Type,S02Ref_No,S02Qty,S02Status)" & _
                    '                                        " values( '" & (UltraGrid1.Rows(i).Cells(0).Value) & "','" & _Loccode & "','" & _GETDATE & "','GRN','-','" & CDbl(UltraGrid1.Rows(i).Cells(0).Value) + CDbl(UltraGrid1.Rows(i).Cells(0).Value) & "','A')"
                    '        ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                    '    ElseIf Trim(MB51.Tables(0).Rows(0)("M11Type")) = "PRODUCT ITEM" Then
                    '        nvcFieldList1 = "Insert Into S01Stock_Product_Items(S01Part_No,S01Loc_Code,S01Date,S01Tr_Type,S01Ref_No,S01Qty,S01Status)" & _
                    '                                          " values( '" & (UltraGrid1.Rows(i).Cells(0).Value) & "','" & _Loccode & "','" & _GETDATE & "','GRN','-','" & CDbl(UltraGrid1.Rows(i).Cells(0).Value) + CDbl(UltraGrid1.Rows(i).Cells(0).Value) & "','A')"
                    '        ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                    '    End If
                    'End If
                    i = i + 1
                Next
                transaction.Commit()
                connection.Close()
                A = MsgBox("Are you sure you want to print GRN Dispatch Note", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Information .......")
                If A = vbYes Then
                    A = ConfigurationManager.AppSettings("ReportPath") + "\GRN_Dispatchnote.rpt"
                    B.Load(A.ToString)
                    B.SetDatabaseLogon("sa", "sainfinity")
                    
                    '  frmReport.CrystalReportViewer1.SelectionFormula = "{T01Transaction_Header.T01RefNo} =" & P01 & ""
                    frmReport.CrystalReportViewer1.ReportSource = B 'intanance System\CrystalReport1" 'B ' "f:\salesinvo1.rpt" 'A.ToString '"F:\Stock Maintanance System\Report\salesinvo1.rpt"
                    frmReport.CrystalReportViewer1.DisplayToolbar = True
                    frmReport.CrystalReportViewer1.SelectionFormula = "{T01GRN_Header.T01GRN_No}='" & txtEntry.Text & "'"
                    frmReport.Refresh()
                    ' frmReport.CrystalReportViewer1.PrintReport()
                    ' B.PrintToPrinter(1, True, 0, 0)
                    frmReport.MdiParent = MDIMain
                    frmReport.Show()
                End If
                Call Clear_text()
            End If
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                connection.Close()
            End If
        End Try
    End Function

    Function Clear_text()
        Call Load_EntryNo()
        Call Load_Gride2()
        Me.cboCode.Text = ""
        Me.cboItemName.Text = ""
        Me.txtFree.Text = ""
        Me.txtCount.Text = ""
        Me.txtRate.Text = "0"
        Me.txtQty.Text = "0"
        Me.txtDiscount.Text = "0"
        Me.txtTotal.Text = "00.00"
        Me.txtNett.Text = ""
        Me.txtGross.Text = ""
        Me.txtDiscount.Text = ""
        Me.txtRemark.Text = ""
        Me.txtCom_Invoice.Text = ""
        Me.txtPO.Text = ""
        Me.txtVAT.Text = ""
        Me.txtNBT.Text = ""
        Me.cboLocation.Text = ""
        Me.cboTo.Text = ""
        Me.cmdEdit.Enabled = False
        cmdDelete.Enabled = False

    End Function

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        Call Clear_text()
    End Sub

    Private Sub ExitToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem1.Click
        Me.Close()
    End Sub

    Private Sub RefreshToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshToolStripMenuItem.Click
        Call Clear_text()
    End Sub

    Private Sub DeactivateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeactivateToolStripMenuItem.Click
        frmview_GRN.Close()
        frmview_GRN.Show()
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        If SEARCH_SUPPLIER() = True Then
        Else
            MsgBox("Please select the supplier", MsgBoxStyle.Information, "Information ........")
            cboLocation.ToggleDropdown()
            Exit Sub
        End If

        If SEARCH_LOCATION() = True Then
        Else
            MsgBox("Please select the location", MsgBoxStyle.Information, "Information ........")
            cboTo.ToggleDropdown()
            Exit Sub
        End If

        If Trim(txtCom_Invoice.Text) <> "" Then
        Else
            txtCom_Invoice.Text = "-"
        End If

        If txtPO.Text <> "" Then
        Else
            txtPO.Text = "-"
        End If

        If txtRemark.Text <> "" Then
        Else
            txtRemark.Text = "-"
        End If

        Call EDIT_DATA()

    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim nvcFieldList1 As String

        Dim connection As SqlClient.SqlConnection
        Dim transaction As SqlClient.SqlTransaction
        Dim transactionCreated As Boolean
        Dim connectionCreated As Boolean

        connection = DBEngin.GetConnection(True)
        connectionCreated = True
        transaction = connection.BeginTransaction()
        transactionCreated = True
        Dim MB51 As DataSet

        Dim i As Integer
        Dim _GETDATE As DateTime
        Dim _GET_TIME As DateTime
        Dim A As String
        Dim _sTRING As String
        Try
            A = MsgBox("Are you sure you want to delete this GRN", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Delete ........")
            If A = vbYes Then
                nvcFieldList1 = "select * from T01GRN_Header where T01Status='A' and T01GRN_No='" & Trim(txtEntry.Text) & "'  AND T01Status in ('CONFIRM','CANCEL')"
                MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
                If isValidDataset(MB51) Then
                    MsgBox("This GRN Can't be delete", MsgBoxStyle.Information, "Information .......")
                    connection.Close()
                    Exit Sub
                End If

                nvcFieldList1 = "SELECT * FROM T20Supplier_Payment WHERE T20Ref_No='" & txtEntry.Text & "' AND T20Status='PAID'"
                MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
                If isValidDataset(MB51) Then
                    MsgBox("You can not delete this GRN.alrady paid for this GRN", MsgBoxStyle.Information, "Information .........")
                    connection.Close()
                    Exit Sub
                End If

                MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
                nvcFieldList1 = "UPDATE T02GRN_Flutter SET T02Status='CANCEL' WHERE T02GRN_No='" & Trim(txtEntry.Text) & "'"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "UPDATE T01GRN_Header SET T01Status='CANCEL' WHERE T01GRN_No='" & Trim(txtEntry.Text) & "'"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "UPDATE T20Supplier_Payment SET T20Status='CANCEL' WHERE T20Ref_No='" & Trim(txtEntry.Text) & "'"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)


                nvcFieldList1 = "Insert Into tmpTransaction_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                                                               " values('GRN', 'DELETE','" & _GET_TIME & "','" & strDisname & "','" & Trim(txtEntry.Text) & "')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                MsgBox("Records deleted successfully", MsgBoxStyle.Information, "Information .......")
                transaction.Commit()
                connection.Close()
                Call Clear_text()
                ' Call Load_EntryNo()
                cboLocation.ToggleDropdown()

            End If

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                connection.Close()
            End If
        End Try
    End Sub

    Private Sub UltraButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton1.Click
        Dim A As String
        Dim B As New ReportDocument
        Try
            A = ConfigurationManager.AppSettings("ReportPath") + "\GRN_Dispatchnote.rpt"
            B.Load(A.ToString)
            B.SetDatabaseLogon("sa", "sainfinity")

            '  frmReport.CrystalReportViewer1.SelectionFormula = "{T01Transaction_Header.T01RefNo} =" & P01 & ""
            frmReport.CrystalReportViewer1.ReportSource = B 'intanance System\CrystalReport1" 'B ' "f:\salesinvo1.rpt" 'A.ToString '"F:\Stock Maintanance System\Report\salesinvo1.rpt"
            frmReport.CrystalReportViewer1.DisplayToolbar = True
            frmReport.CrystalReportViewer1.SelectionFormula = "{T01GRN_Header.T01GRN_No}='" & txtEntry.Text & "'"
            frmReport.Refresh()
            ' frmReport.CrystalReportViewer1.PrintReport()
            ' B.PrintToPrinter(1, True, 0, 0)
            frmReport.MdiParent = MDIMain
            frmReport.Show()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                'connection.Close()
            End If
        End Try
    End Sub

    Private Sub cboItemName_AfterCloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboItemName.AfterCloseUp
        Call Search_Itemcode()
    End Sub
End Class