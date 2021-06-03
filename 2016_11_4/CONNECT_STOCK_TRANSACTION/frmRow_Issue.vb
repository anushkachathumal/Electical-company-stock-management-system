Imports System.Data.SqlClient
Imports Infragistics.Win.UltraWinGrid
Imports DBLotVbnet.common
Imports System.Net
Imports DBLotVbnet.DBEngin
Imports DBLotVbnet.DAL_InterLocation
Imports DBLotVbnet.DAL_Distributors
Imports System.Configuration
Imports CrystalDecisions.CrystalReports.Engine
Public Class frmRow_Issue
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
   
    Function Load_TO_Location()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select M03Loc_Name as [From Location] from M03New_Location where  M03Status='A'"
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

    Function Load_Item_Code()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select M11Part_No as [##],M11Part_Name as [Item Name] from M11Product_Item where M11Status='A' and M11Type='ROW ITEMS' order by M11ID "
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
            Sql = "select M11Part_Name as [##] from M11Product_Item where M11Status='A' and M11Type='ROW ITEMS' order by M11ID "
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

    Function Calculation_Total()
        On Error Resume Next
        Dim Value As Double

        If IsNumeric(txtQty.Text) And IsNumeric(txtRate.Text) Then
            Value = CDbl(txtQty.Text) * CDbl(txtRate.Text)
            txtTotal.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
            txtTotal.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))

        End If

    End Function

    Private Sub frmRow_Issue_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtCount.ReadOnly = True
        txtCount.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtNet.Appearance.TextHAlign = Infragistics.Win.HAlign.Right
        txtNet.ReadOnly = True
        txtTotal.ReadOnly = True
        txtTotal.Appearance.TextHAlign = Infragistics.Win.HAlign.Right
        txtQty.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtRate.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtDate.Text = Today
        Call Load_EntryNo()
        Call Load_Gride2()
        Call Load_TO_Location()
        Call Load_Item_Code()
        Call Load_Item_Name()
        txtRate.ReadOnly = True
    End Sub


    Function Search_Records()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet
        Dim i As Integer
        Dim Value As Double
        Dim _ST As String

        Try
            Sql = "select * from T05Row_Transfer_Header inner join M03New_Location on M03Loc_Code=T05Loc inner join T06Row_Transfer_Flutter on T05Tr_No=T06Tr_No  inner join M11Product_Item on M11Part_No=T06Item_Code where T05Tr_No='" & Trim(txtEntry.Text) & "'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            i = 0
            For Each DTRow2 As DataRow In M01.Tables(0).Rows
                cboLocation.Text = Trim(M01.Tables(0).Rows(0)("M03Loc_Name"))
                txtRemark.Text = Trim(M01.Tables(0).Rows(0)("T05Remark"))
                Dim newRow As DataRow = c_dataCustomer1.NewRow
                newRow("Item Code") = Trim(M01.Tables(0).Rows(i)("T06Item_Code"))
                newRow("Item Name") = Trim(M01.Tables(0).Rows(i)("M11Part_Name"))
                Value = Trim(M01.Tables(0).Rows(i)("T06Cost"))
                _st = (vALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                _st = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", vALUE))
                newRow("Cost Price") = _ST
                newRow("Qty") = Trim(M01.Tables(0).Rows(i)("T06Qty"))

                Value = CDbl(M01.Tables(0).Rows(i)("T06Qty")) * CDbl(M01.Tables(0).Rows(i)("T06Cost"))
                _st = (vALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                _st = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", vALUE))
                newRow("Total") = _st

                c_dataCustomer1.Rows.Add(newRow)

                cmdEdit.Enabled = True
                cmdDelete.Enabled = True
                i = i + 1
            Next
            txtCount.Text = UltraGrid1.Rows.Count
            Call Calculation_Sub()
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.close()
            End If
        End Try
    End Function
    Function Load_EntryNo()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select * from P01Parameter where  P01Code='RI'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                If M01.Tables(0).Rows(0)("P01No") >= 1 And M01.Tables(0).Rows(0)("P01No") < 10 Then
                    txtEntry.Text = "RM-00" & M01.Tables(0).Rows(0)("P01No")
                ElseIf M01.Tables(0).Rows(0)("P01No") >= 10 And M01.Tables(0).Rows(0)("P01No") < 100 Then
                    txtEntry.Text = "RM-0" & M01.Tables(0).Rows(0)("P01No")
                Else
                    txtEntry.Text = "RM-" & M01.Tables(0).Rows(0)("P01No")
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

    Function Search_Item() As Boolean
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet
        Dim vALUE As Double
        Dim _st As String
        Dim i As Integer
        Try
            Sql = "SELECT M11Part_Name FROM M11Product_Item WHERE M11Part_No='" & Trim(cboCode.Text) & "' AND M11Status='A' and M11Type='ROW ITEMS' "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                Search_Item = True
                cboItemName.Text = Trim(M01.Tables(0).Rows(0)("M11Part_Name"))

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
            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.CLOSE()
            End If
        End Try

    End Function

    Function Load_Gride2()
        Dim CustomerDataClass As New DAL_InterLocation()
        c_dataCustomer1 = CustomerDataClass.MakeDataTable_Wastage_cnt
        UltraGrid1.DataSource = c_dataCustomer1
        With UltraGrid1
            .DisplayLayout.Bands(0).Columns(0).Width = 90
            .DisplayLayout.Bands(0).Columns(1).AutoEdit = False
            .DisplayLayout.Bands(0).Columns(1).Width = 210
            .DisplayLayout.Bands(0).Columns(1).AutoEdit = False

            .DisplayLayout.Bands(0).Columns(2).Width = 80
            .DisplayLayout.Bands(0).Columns(3).Width = 90
            ' .DisplayLayout.Bands(0).Columns(4).Width = 90
            '.DisplayLayout.Bands(0).Columns(5).Width = 70
            '.DisplayLayout.Bands(0).Columns(6).Width = 80
            '.DisplayLayout.Bands(0).Columns(7).Width = 90
            '' .DisplayLayout.Bands(0).Columns(8).Width = 90

            .DisplayLayout.Bands(0).Columns(2).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            .DisplayLayout.Bands(0).Columns(3).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            .DisplayLayout.Bands(0).Columns(4).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            '.DisplayLayout.Bands(0).Columns(5).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            '.DisplayLayout.Bands(0).Columns(7).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            '.DisplayLayout.Bands(0).Columns(6).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right


            .DisplayLayout.Bands(0).Columns(0).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(1).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(2).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(3).CellActivation = Activation.NoEdit
            '.DisplayLayout.Bands(0).Columns(4).CellActivation = Activation.NoEdit
            '.DisplayLayout.Bands(0).Columns(5).CellActivation = Activation.NoEdit
            '.DisplayLayout.Bands(0).Columns(6).CellActivation = Activation.NoEdit
            '.DisplayLayout.Bands(0).Columns(7).CellActivation = Activation.NoEdit


            .DisplayLayout.Bands(0).Columns(0).CellActivation = Activation.NoEdit
            '  .DisplayLayout.Bands(0).Columns(6).Width = 90
            ' .DisplayLayout.Bands(0).Columns(7).Width = 90

            ' .DisplayLayout.Bands(0).Columns(3).Width = 300
            '.DisplayLayout.Bands(0).Columns(4).Width = 300
        End With
    End Function

    Private Sub cboLocation_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboLocation.KeyUp
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
            Sql = "select * from M11Product_Item where  M11Part_Name='" & Trim(cboItemName.Text) & "' and M11Status='A' and M11Type='ROW ITEMS' "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                Search_ItemName = True
                cboCode.Text = Trim(M01.Tables(0).Rows(0)("M11Part_No"))
                'Value = Trim(M01.Tables(0).Rows(0)("M11Part_Name"))

            End If

            'AVG cost
            Sql = "select sum(M23Total/M23Qty) as Cost from M23Cost_Analysis where M23Item_Code='" & Trim(cboCode.Text) & "' and M23Status='A' group by M23Item_Code"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                Value = M01.Tables(0).Rows(0)("Cost")
                txtRate.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtRate.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
            Else
                txtRate.Text = "0"
            End If

            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.close()
            End If
        End Try
    End Function
    Private Sub cboItemName_AfterCloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboItemName.AfterCloseUp
        Call Search_ItemName()
    End Sub

    Private Sub cboCode_AfterCloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboCode.AfterCloseUp
        Call Search_Item()
    End Sub

    Private Sub cboCode_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboCode.KeyUp
        If e.KeyCode = 13 Then
            txtQty.Focus()
        End If
    End Sub

    Private Sub txtQty_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtQty.KeyUp
        Try
            If e.KeyCode = 13 Then
                If Search_Item() = True Then
                Else
                    MsgBox("Please select the correct Item code", MsgBoxStyle.Information, "Information ......")
                    Exit Sub
                End If
                '==============================================================================
                If txtQty.Text <> "" Then
                Else
                    MsgBox("Please enter the correct qty ", MsgBoxStyle.Information, "Information ......")
                    Exit Sub
                End If
                '===============================================================================
                If IsNumeric(txtQty.Text) Then
                Else
                    MsgBox("Please enter the correct qty ", MsgBoxStyle.Information, "Information ......")
                    Exit Sub
                End If

                Call Calculation_Total()
                Dim newRow As DataRow = c_dataCustomer1.NewRow
                newRow("Item Code") = Trim(cboCode.Text)
                newRow("Item Name") = cboItemName.Text
                newRow("Cost Price") = txtRate.Text
                newRow("Qty") = txtQty.Text
                newRow("Total") = txtTotal.Text
                c_dataCustomer1.Rows.Add(newRow)
                txtCount.Text = UltraGrid1.Rows.Count
                Call Calculation_Sub()
                txtTotal.Text = ""
                txtQty.Text = ""
                txtRate.Text = ""
                cboCode.Text = ""
                cboItemName.Text = ""
                cboCode.ToggleDropdown()
            End If

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                '  con.close()
            End If
        End Try
    End Sub

    Function Calculation_Sub()
        Dim i As Integer
        Dim Value As Double


        For Each uRow As UltraGridRow In UltraGrid1.Rows
            ' MsgBox(Double.TryParse(txtNett.Text, value))
            Value = Value + CDbl((UltraGrid1.Rows(i).Cells(4).Value))
            i = i + 1
        Next
        txtNet.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
        txtNet.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
    End Function
    Private Sub txtQty_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtQty.ValueChanged
        Call Calculation_Total()
    End Sub
    Private Sub UltraGrid1_AfterRowsDeleted(ByVal sender As Object, ByVal e As System.EventArgs) Handles UltraGrid1.AfterRowsDeleted
        Call Calculation_Sub()
        txtCount.Text = UltraGrid1.Rows.Count
    End Sub

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        Call Clear_Text()
    End Sub

    Function Clear_Text()
        Me.txtRate.Text = ""
        Me.txtTotal.Text = ""
        Me.txtNet.Text = ""
        Me.txtCount.Text = ""
        Me.txtQty.Text = ""
        Me.cboCode.Text = ""
        Me.cboItemName.Text = ""
        Me.txtRemark.Text = ""
        Me.cboLocation.Text = ""
        cmdDelete.Enabled = False
        cmdEdit.Enabled = False
        Call Load_Gride2()
        Call Load_EntryNo()
    End Function

    Private Sub ExitToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem1.Click
        Me.Close()
    End Sub

    Private Sub RefreshToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshToolStripMenuItem.Click
        Call Clear_Text()
    End Sub

    Function SEARCH_LOCATION() As Boolean
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet
        Dim Value As Double
        Try
            Sql = "select M03Loc_Code from M03New_Location where  M03Loc_Name='" & Trim(cboLocation.Text) & "' and M03Status='A' "
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

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        If txtRemark.Text <> "" Then
        Else
            txtRemark.Text = "-"
        End If

        If SEARCH_LOCATION() = True Then
        Else
            MsgBox("Please select the Location Name", MsgBoxStyle.Information, "Information ..........")
            cboLocation.ToggleDropdown()
            Exit Sub
        End If

        If UltraGrid1.Rows.Count > 0 Then
        Else
            MsgBox("Please enter the Item detailes", MsgBoxStyle.Information, "Information .......")
            cboCode.ToggleDropdown()
            Exit Sub
        End If

        Call Save_Date()

    End Sub

    Function Save_Date()
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
        Dim M01 As DataSet
        Dim A As String
        Dim B As New ReportDocument
        Try
            'nvcFieldList1 = "SELECT * FROM T21GRN_Confirm_Header where T21Ref_No='" & Trim(txtRef.Text) & "' and T01Cost_Status='NO'"
            'MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
            'If isValidDataset(MB51) Then
            '    MsgBox("Can't Confirm ")
            'End If
            '===================================================================================================
            nvcFieldList1 = "SELECT * FROM T05Row_Transfer_Header where T05Tr_No='" & Trim(txtEntry.Text) & "'"
            MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
            If isValidDataset(MB51) Then
                MsgBox("This Row Issue Note alrady exsist", MsgBoxStyle.Information, "Information ........")
                connection.Close()
                Exit Function
            Else
                _GETDATE = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text)

                _GET_TIME = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text) & " " & Hour(Now) & ":" & Minute(Now)

                Call Load_EntryNo()

                nvcFieldList1 = "update P01Parameter set P01No=P01No+ " & 1 & " where P01Code='RI' "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into T05Row_Transfer_Header(T05Tr_No,T05Loc,T05Date,T05Remark,T05Status)" & _
                                                                  " values('" & Trim(txtEntry.Text) & "','" & _Loccode & "', '" & _GETDATE & "','" & Trim(txtRemark.Text) & "','A')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into tmpTransaction_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                                                              " values('ROW_ISSUE', 'SAVE','" & _GET_TIME & "','" & strDisname & "','" & Trim(txtEntry.Text) & "')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)


                i = 0
                For Each uRow As UltraGridRow In UltraGrid1.Rows
                    If IsNumeric(Trim(UltraGrid1.Rows(i).Cells(3).Text)) Then

                        nvcFieldList1 = "Insert Into T06Row_Transfer_Flutter(T06Tr_No,T06Item_Code,T06Cost,T06Qty,T06Status)" & _
                                                                          " values('" & Trim(txtEntry.Text) & "','" & Trim(UltraGrid1.Rows(i).Cells(0).Text) & "','" & Trim(UltraGrid1.Rows(i).Cells(2).Text) & "','" & Trim(UltraGrid1.Rows(i).Cells(3).Text) & "','A')"
                        ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                        nvcFieldList1 = "SELECT * FROM M11Product_Item WHERE M11Part_No='" & Trim(UltraGrid1.Rows(i).Cells(0).Text) & "'"
                        M01 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
                        If isValidDataset(M01) Then
                            If Trim(M01.Tables(0).Rows(0)("M11Type")) = "PRODUCT ITEM" Then
                                nvcFieldList1 = "Insert Into S01Stock_Product_Items(S01Part_No,S01Loc_Code,S01Date,S01Tr_Type,S01Ref_No,S01Qty,S01Status)" & _
                                                                               " values('" & Trim(UltraGrid1.Rows(i).Cells(0).Text) & "', '" & _Loccode & "','" & _GETDATE & "','ROW_ISSUE','" & Trim(txtEntry.Text) & "','" & -(UltraGrid1.Rows(i).Cells(3).Text) & "','A')"
                                ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                            ElseIf Trim(M01.Tables(0).Rows(0)("M11Type")) = "ROW ITEMS" Then
                                nvcFieldList1 = "Insert Into S02Row_Stock(S02Part_no,S02Loc_Code,S02Date,S02Tr_Type,S02Ref_No,S02Qty,S02Status)" & _
                                                                             " values('" & Trim(UltraGrid1.Rows(i).Cells(0).Text) & "', '" & _Loccode & "','" & _GETDATE & "','ROW_ISSUE','" & Trim(txtEntry.Text) & "','" & -(UltraGrid1.Rows(i).Cells(3).Text) & "','A')"
                                ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                            End If
                        End If




                    End If
                    i = i + 1
                Next

                transaction.Commit()
                connection.Close()
                A = MsgBox("Are you sure you want to print Row material issue note", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Informtion ........")
                If A = vbYes Then
                    A = ConfigurationManager.AppSettings("ReportPath") + "\Rowmaterial_Out.rpt"
                    B.Load(A.ToString)
                    B.SetDatabaseLogon("sa", "sainfinity")

                    '  frmReport.CrystalReportViewer1.SelectionFormula = "{T01Transaction_Header.T01RefNo} =" & P01 & ""
                    frmReport.CrystalReportViewer1.ReportSource = B 'intanance System\CrystalReport1" 'B ' "f:\salesinvo1.rpt" 'A.ToString '"F:\Stock Maintanance System\Report\salesinvo1.rpt"
                    frmReport.CrystalReportViewer1.DisplayToolbar = True
                    frmReport.CrystalReportViewer1.SelectionFormula = "{T05Row_Transfer_Header.T05Tr_No}='" & txtEntry.Text & "'"
                    frmReport.Refresh()
                    ' frmReport.CrystalReportViewer1.PrintReport()
                    ' B.PrintToPrinter(1, True, 0, 0)
                    frmReport.MdiParent = MDIMain
                    frmReport.Show()
                End If
                Call Clear_Text()
                ' Call Load_EntryNo()
                Call Load_EntryNo()
            End If
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                connection.Close()
            End If
        End Try
    End Function

    Private Sub DeactivateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeactivateToolStripMenuItem.Click
        frmview_Rowissue.Close()
        frmview_Rowissue.Show()
    End Sub

    Private Sub UltraButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton1.Click
        Dim A As String
        Dim B As New ReportDocument
        Try
            A = ConfigurationManager.AppSettings("ReportPath") + "\Rowmaterial_Out.rpt"
            B.Load(A.ToString)
            B.SetDatabaseLogon("sa", "sainfinity")

            '  frmReport.CrystalReportViewer1.SelectionFormula = "{T01Transaction_Header.T01RefNo} =" & P01 & ""
            frmReport.CrystalReportViewer1.ReportSource = B 'intanance System\CrystalReport1" 'B ' "f:\salesinvo1.rpt" 'A.ToString '"F:\Stock Maintanance System\Report\salesinvo1.rpt"
            frmReport.CrystalReportViewer1.DisplayToolbar = True
            frmReport.CrystalReportViewer1.SelectionFormula = "{T05Row_Transfer_Header.T05Tr_No}='" & txtEntry.Text & "'"
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

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        If txtRemark.Text <> "" Then
        Else
            txtRemark.Text = "-"
        End If

        If SEARCH_LOCATION() = True Then
        Else
            MsgBox("Please select the Location Name", MsgBoxStyle.Information, "Information ..........")
            cboLocation.ToggleDropdown()
            Exit Sub
        End If

        If UltraGrid1.Rows.Count > 0 Then
        Else
            MsgBox("Please enter the Item detailes", MsgBoxStyle.Information, "Information .......")
            cboCode.ToggleDropdown()
            Exit Sub
        End If

        Call Edit_Save()
    End Sub

    Function Edit_Save()
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
        Dim M01 As DataSet
        Dim A As String
        Dim B As New ReportDocument
        Try
            'nvcFieldList1 = "SELECT * FROM T21GRN_Confirm_Header where T21Ref_No='" & Trim(txtRef.Text) & "' and T01Cost_Status='NO'"
            'MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
            'If isValidDataset(MB51) Then
            '    MsgBox("Can't Confirm ")
            'End If
            '===================================================================================================
            nvcFieldList1 = "SELECT * FROM T05Row_Transfer_Header where T05Tr_No='" & Trim(txtEntry.Text) & "'"
            MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
            If isValidDataset(MB51) Then

                _GETDATE = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text)

                _GET_TIME = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text) & " " & Hour(Now) & ":" & Minute(Now)

                nvcFieldList1 = "UPDATE T05Row_Transfer_Header SET T05Loc='" & _Loccode & "',T05Date='" & txtDate.Text & "',T05Remark='" & txtRemark.Text & "' WHERE T05Tr_No='" & Trim(txtEntry.Text) & "' "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into tmpTransaction_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                                                              " values('ROW_ISSUE', 'EDIT','" & _GET_TIME & "','" & strDisname & "','" & Trim(txtEntry.Text) & "')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "DELETE FROM T06Row_Transfer_Flutter WHERE T06Tr_No='" & Trim(txtEntry.Text) & "'"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                'nvcFieldList1 = "UPDATE S01Stock_Product_Items SET S01Status='CANCEL' WHERE S01Ref_No='" & Trim(txtEntry.Text) & "' "
                'ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "UPDATE S02Row_Stock SET S02Status='CANCEL' WHERE S02Ref_No='" & Trim(txtEntry.Text) & "' "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                i = 0
                For Each uRow As UltraGridRow In UltraGrid1.Rows
                    If IsNumeric(Trim(UltraGrid1.Rows(i).Cells(3).Text)) Then

                        nvcFieldList1 = "Insert Into T06Row_Transfer_Flutter(T06Tr_No,T06Item_Code,T06Cost,T06Qty,T06Status)" & _
                                                                          " values('" & Trim(txtEntry.Text) & "','" & Trim(UltraGrid1.Rows(i).Cells(0).Text) & "','" & Trim(UltraGrid1.Rows(i).Cells(2).Text) & "','" & Trim(UltraGrid1.Rows(i).Cells(3).Text) & "','A')"
                        ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                        nvcFieldList1 = "SELECT * FROM M11Product_Item WHERE M11Part_No='" & Trim(UltraGrid1.Rows(i).Cells(0).Text) & "'"
                        M01 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
                        If isValidDataset(M01) Then
                            If Trim(M01.Tables(0).Rows(0)("M11Type")) = "PRODUCT ITEM" Then
                                'nvcFieldList1 = "Insert Into S01Stock_Product_Items(S01Part_No,S01Loc_Code,S01Date,S01Tr_Type,S01Ref_No,S01Qty,S01Status)" & _
                                '                                               " values('" & Trim(UltraGrid1.Rows(i).Cells(0).Text) & "', '" & _Loccode & "','" & _GETDATE & "','WASTAGE','" & Trim(txtEntry.Text) & "','" & -(UltraGrid1.Rows(i).Cells(3).Text) & "','A')"
                                'ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                            ElseIf Trim(M01.Tables(0).Rows(0)("M11Type")) = "ROW ITEMS" Then
                                nvcFieldList1 = "Insert Into S02Row_Stock(S02Part_no,S02Loc_Code,S02Date,S02Tr_Type,S02Ref_No,S02Qty,S02Status)" & _
                                                                             " values('" & Trim(UltraGrid1.Rows(i).Cells(0).Text) & "', '" & _Loccode & "','" & _GETDATE & "','ROW_ISSUE','" & Trim(txtEntry.Text) & "','" & -(UltraGrid1.Rows(i).Cells(3).Text) & "','A')"
                                ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                            End If
                        End If




                    End If
                    i = i + 1
                Next

                transaction.Commit()
                connection.Close()

                Call Clear_Text()
                ' Call Load_EntryNo()
                ' Call Load_EntryNo()
            End If
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                connection.Close()
            End If
        End Try
    End Function

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
        Dim A As String
        Dim _GET_TIME As DateTime
        Dim _GETDATE As DateTime
        Try
            _GET_TIME = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text) & " " & Hour(Now) & ":" & Minute(Now)

            A = MsgBox("Are you sure you want to delete this transaction", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Information .........")
            If A = vbYes Then
                'nvcFieldList1 = "UPDATE S01Stock_Product_Items SET S01Status='DELETE' WHERE S01Ref_No='" & Trim(txtEntry.Text) & "' "
                'ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "UPDATE S02Row_Stock SET S02Status='DELETE' WHERE S02Ref_No='" & Trim(txtEntry.Text) & "' "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "UPDATE T05Row_Transfer_Header SET T05Status='DELETE' WHERE T05Tr_No='" & Trim(txtEntry.Text) & "' "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "UPDATE T06Row_Transfer_Flutter SET T06Status='DELETE' WHERE T06Tr_No='" & Trim(txtEntry.Text) & "' "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into tmpTransaction_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                                                        " values('ROW_ISSUE', 'DELETE','" & _GET_TIME & "','" & strDisname & "','" & Trim(txtEntry.Text) & "')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                MsgBox("Record deleteted successfully", MsgBoxStyle.Information, "Information .........")
            End If
            transaction.Commit()
            connection.Close()
            Call Clear_Text()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                connection.Close()
            End If
        End Try
    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub
End Class